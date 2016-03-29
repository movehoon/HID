using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using LitJson;

public class Program : MonoBehaviour {

	public const string Key_IpAddr = "Key_IpAddr";
	const string IGN_TRIG_NAME = "e:setup-topic";

	public InputField ipaddress;
	public Button connectButton;
	public InputField inputText;

	public RectTransform TriggerView;
	public Transform UI_Trigger;
	public GameObject UI_PopupRecogInput;

	int port = 9090;
	bool mRun = false;
	string receivedMessage = @"{""topic"": ""/social_memory/request_hid_input"", ""msg"": {""msg"": ""[\""(\\uc548\\ub155|e:person-identified)\"", \""*\"", \""i:setup-topic\""]"", ""header"": {""stamp"": {""secs"": 0, ""nsecs"": 0}, ""frame_id"": """", ""seq"": 1}}, ""op"": ""publish""}";
	string parsingMessage = "";
	string rosSpeechRecog = @"{ ""op"": ""call_service"", ""service"": ""/social_memory/write_data"", ""args"": {""event_name"": ""speech_recognition"", ""event"":""{""speech_recognized"": true}"", ""data"": ""{""event_name"":""speech_recognized"", ""recognized_word"": ""hi""}"", ""by"": ""hid""} }";

	Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	void Start () {
//		connectionDialog.SetActive (true);
		ipaddress.text = GetIpAddr ();
//		socket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
//		Parse (receivedMessage);
	}

	void Update () {
		if (receivedMessage.Length > 0) {
			parsingMessage += receivedMessage;
			if (Parse (parsingMessage))
				parsingMessage = "";
			receivedMessage = "";
		}
	}

	public bool IsConnected () {
		try {
			return socket.Connected;
		}
		catch (Exception e)  {
			Debug.Log ("Program::IsConnected -> " + e.ToString ());
		}
		return false;
	}

	public void Connect () {
		try 
		{
			if (!socket.Connected)
			{
				SetIpAddr(ipaddress.text);
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipaddress.text), port);
				Debug.Log ("Connect to " + ipaddress.text);
				socket.Connect(endpoint);
				if (socket.Connected)
					connectButton.GetComponentInChildren<Text>().text = "Disconnect";
				mRun = true;
				Thread thread = new Thread (new ThreadStart (Process_Thread));
				thread.Start ();
				TopicSub ("/social_memory/request_hid_input");
			}
			else
			{
				mRun = false;
				socket.Shutdown(SocketShutdown.Both);
				socket.Disconnect (true);
				connectButton.GetComponentInChildren<Text>().text = "Connect";
			}
		}
		catch (Exception ex) 
		{
			Debug.Log (ex.ToString ());
		}
	}

	public void TopicSub(string topic) {
		RosTopicSub topicSub = new RosTopicSub ();
		topicSub.op = "subscribe";
		topicSub.topic = topic;
		Send (JsonMapper.ToJson(topicSub));
	}

	public void ReloadDialog () {
		RosCallService reload = new RosCallService ();
		reload.op = "call_service";
		reload.service = "/mhri_dialog/reload";
		reload.args = null;

		string jsonString = JsonMapper.ToJson (reload);
		Send (jsonString);
	}

	public void SendSpeechRecognized (string speech) {
		RosSpeechRecognized speechRec = new RosSpeechRecognized ();
		speechRec.speech_recognized = true;

		RosSpeechData speechData = new RosSpeechData ();
		speechData.recognized_word = speech;
		speechData.confidence = 1.0;

		RosEvent evt = new RosEvent ();
		evt.event_name = "speech_recognition";
		evt.@event = JsonMapper.ToJson(speechRec);
		evt.data = JsonMapper.ToJson(speechData);
		evt.by = "hid";

		RosCallService writeToMemory = new RosCallService ();
		writeToMemory.op = "call_service";
		writeToMemory.service = "/social_memory/write_data";
		writeToMemory.args = evt;

		string jsonString = JsonMapper.ToJson(writeToMemory);
		Send (jsonString);
	}

	public void Test () {
		SendSpeechRecognized (inputText.text);
	}

	bool Parse (string input) {
		try {
			JsonData json = JsonMapper.ToObject (input);
			if (json ["op"].ToString () == "publish") {
				if (json ["topic"].ToString () == "/social_memory/request_hid_input") {
					string trig_string = jsonRefine(json ["msg"] ["msg"].ToString ());
					JsonData json_triggers = JsonMapper.ToObject (trig_string);
					List<string> triggers = new List<string> ();
					for (int i = 0; i < json_triggers.Count; i++) {
						if (json_triggers [i].ToString ().Contains (IGN_TRIG_NAME))
							continue;
//						if (hasOR (json_triggers [i].ToString ())) {
//							string[] tmpString = separateOR (json_triggers [i].ToString ());
//							foreach (string trig in tmpString)
//								triggers.Add (trig);
//						} else {
							triggers.Add (json_triggers [i].ToString ());
//						}
						Debug.Log ("Trigger " + json_triggers [i].ToString());
					}

					RectTransform Content = TriggerView.GetComponentInChildren<ScrollRect>().content.GetComponentInChildren<RectTransform>();
					Content.sizeDelta = new Vector2 (TriggerView.sizeDelta.x, triggers.Count * 40f);


					float yStart = -20f;
					float yStep = -40f;
					for (int i = 0; i < Content.childCount; i++) {
						Destroy (Content.GetChild (i).gameObject);
					}
					Content.DetachChildren ();
					for (int i = 0; i < triggers.Count; i++) {
						Transform uiTrigger = Instantiate (UI_Trigger);
						uiTrigger.SetParent (Content);
						uiTrigger.GetComponentInChildren<Text> ().text = triggers [i];
						uiTrigger.GetComponentInChildren<RectTransform> ().offsetMin = new Vector2(20f, yStart + yStep * i + -15);
						uiTrigger.GetComponentInChildren<RectTransform> ().offsetMax = new Vector2(-20f, yStart + yStep * i + 15);
					}
				}
			}
		}
		catch (JsonException ex) {
			Debug.Log (ex.ToString ());
			return true;
		}
		catch (ArgumentOutOfRangeException ex) {
			Debug.Log (ex.ToString ());
			return true;
		}

		return true;
	}

	bool hasOR (string inputString) {
		if (inputString.Contains ("|"))
			return true;
		return false;
	}

	string[] separateOR (string inputString) {
		inputString = inputString.Replace ("(", String.Empty);
		inputString = inputString.Replace (")", String.Empty);
		return inputString.Split ('|');
	}

	string jsonRefine (string inString)
	{
		if (inString[0] == '"' && inString[inString.Length-1] == '"')
			inString = inString.Substring (1, inString.Length-2);
		return inString.Replace("\\\"", "\"");
	}

	void Send (string jsonString) {
		try {
			if (socket.Connected)
			{
				socket.Send(Encoding.Default.GetBytes(jsonString + "\r\n"));
				Debug.Log (jsonString);
			}
			else
			{
				Debug.Log ("Socket is not connected");
			}
		} catch (Exception e) {
			Debug.Log (e.ToString ());
		}
	}

	void Process_Thread () {
		byte[] bytes = new byte[2048];
		while (mRun)
		{
			if (socket.Connected)
			{
				int nRead = socket.Receive(bytes);
				if (nRead <= 0)
				{
					socket.Close ();
				}
				lock (receivedMessage)
				{
					receivedMessage = Encoding.Default.GetString (bytes);
					receivedMessage = receivedMessage.Substring(0, nRead);
					Debug.Log("length: " + nRead + ", Received: " + receivedMessage);
				}
			}
			else
			{
				Debug.Log ("Socket is disconnected");
//				Disconnect ();
			}
			Thread.Sleep(1);
		}
	}

	static public string GetIpAddr()
	{
		if (PlayerPrefs.HasKey(Key_IpAddr) == false)
		{
			return "192.168.0.2";
		}
		return PlayerPrefs.GetString(Key_IpAddr);
	}
	static public void SetIpAddr(string ipAddr)
	{
		PlayerPrefs.SetString(Key_IpAddr, ipAddr);
	}

	public void ReloadScene ()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}
}

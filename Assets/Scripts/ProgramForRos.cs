using UnityEngine;
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

public class ProgramForRos : MonoBehaviour {
	public const string Key_IpAddr = "Key_IpAddr";
	
	public GameObject connectionDialog;
	public GameObject panelMode;
	public GameObject panelEmotion;
	public UIInput ipaddress;
	public UIButton connectButton;
	int port = 9090;
	bool mRun = false;

	public UiManager uiManager;

	string rosSubscribe_RequestHidInput = @"{""op"":""subscribe"", ""topic"":""memory_monitor/request_hid_input"",""type"":""memory_monitor/RequestHIDInput""}";
	string rosReceivedMessage1 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""face_detected\""], \""query\"": [\""{\\\""detected\\\"":true}\""]}"", ""header"": {""stamp"": {""secs"": 1429181797, ""nsecs"": 859826087}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}";
	string rosReceivedMessage2 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""face_detected\"", \""motion_detected\""], \""query\"": [\""{\\\""motion_detected.detected\\\"": true, \\\""face_detected.detected\\\"": true}\\\""]}"", ""header"": {""stamp"": {""secs"": 1428656219, ""nsecs"": 865901947}, ""frame_id"": "" "", ""seq"": 42}}, ""op"": ""publish""}";
	string rosReceivedMessage3 = @" {""topic"": ""/memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""face_detected\"", \""prospect_recognized\""], \""query\"": [\""{\\\""detected\\\"":true}\"", \""{\\\""prospect\\\"":\\\""positive\\\""}\""]}"", ""header"": {""stamp"": {""secs"": 1429246568, ""nsecs"": 713021039}, ""frame_id"": "" "", ""seq"": 1}}, ""op"": ""publish""}";
	string rosReceivedMessage4 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""speech_recognized\""], \""query\"": [\""{\\\""recognized_word\\\"":[\\\""11\\\"", \\\""12\\\"", \\\""13\\\"", \\\""14\\\""], \\\""confidence\\\"":[0.8, 0.7, 0.6, 0.9]}\""]}"", ""header"": {""stamp"": {""secs"": 1429181797, ""nsecs"": 859826087}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}";
	string rosReceivedMessage5 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""speech_recognized\""], \""query\"": [\""{\\\""recognized_word\\\"":\\\""\\uc548\\ub155\\\""}\""]}"", ""header"": {""stamp"": {""secs"": 1429181797, ""nsecs"": 859826087}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}";

	string rosFaceDetectTrue  = @"{ ""op"": ""call_service"", ""service"": ""/memory_monitor/write_to_memory"", ""args"": {""data"": ""{'event_name':'face_detected', 'detected': true}"", ""by"": ""hid""} }";
	string rosFaceDetectFalse = @"{ ""op"": ""call_service"", ""service"": ""/memory_monitor/write_to_memory"", ""args"": {""data"": ""{'event_name':'face_detected', 'detected': false}"", ""by"": ""hid""} }";
	string rosFaceDetectedHeader = @"{ ""op"": ""call_service"", ""service"": ""/memory_monitor/write_to_memory"", ""args"": {""data"": ""{'event_name':'face_detected', 'detected': ";
	string rosFaceDetectedFooter = @"}"", ""by"": ""hid""} }";

	string rosProspectRecognizedHeader = @"{ ""op"": ""call_service"", ""service"": ""/memory_monitor/write_to_memory"", ""args"": {""data"": ""{'event_name':'prospect_recognized', 'prospect': '";
	string rosProspectRecognizedFooter = @"'}"", ""by"": ""hid""} }";
	
	string rosSpeechRecognizedHeader = @"{ ""op"": ""call_service"", ""service"": ""/memory_monitor/write_to_memory"", ""args"": {""data"": ""{'event_name':'speech_recognized', 'recognized_word': '";
	string rosSpeechRecognizedFooter = @"', 'confidence': 1.0}"", ""by"": ""hid""} }";

	string rosSpeechRecog = @"{ ""op"": ""call_service"", ""service"": ""/memory_monitor/write_to_memory"", ""args"": {""data"": ""{'event_name':'speech_recognized', 'recognized_word': ['hi']}"", ""by"": ""hid""} }";
	string receivedMessage = "";

	Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	bool faceDetected = true;
	
	void Start () {
		connectionDialog.SetActive (true);
		panelMode.SetActive (true);
		TweenPosition tp = panelMode.GetComponentInChildren<TweenPosition> ();
		tp.PlayReverse ();
		panelEmotion.SetActive (true);
		TweenPosition tpEmotion = panelEmotion.GetComponentInChildren<TweenPosition> ();
		tpEmotion.PlayReverse ();
		ipaddress.value = GetIpAddr ();
	}

	void Update () {
		if (receivedMessage.Length > 0) {
			string parseText = receivedMessage;
			receivedMessage = "";
//			Debug.Log ("Decode: " + parseText);
			Parsing (parseText);
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

	void Parsing (string jsonString) {
		try
		{
//			Debug.Log ("length: " + jsonString.Length.ToString () + ", " + jsonString);
			JsonData json = JsonMapper.ToObject (jsonString);
//			Debug.Log ("Json: " + jsonString);
			string topic = json ["topic"].ToString ();
			string msg = json ["msg"]["msg"].ToString ();
//			Debug.Log ("Json msg: " + msg);

			// Manage UI using event_name(s)
			string event_name = JsonMapper.ToObject(msg)["event_name"].ToString ();
			event_name = jsonRefine(event_name);
//			Debug.Log ("Json event_name: " + event_name);
			JsonData events = JsonMapper.ToObject(msg)["event_name"];
			string [] uiNames = new string[events.Count];
			for (int i = 0 ; i < events.Count ; i++) {
				Debug.Log ("Got event: " + events[i]);
				switch (events[i].ToString ())
				{
				case "face_detected":
					uiNames[i] = "FaceDetected";
					break;
				case "motion_detected":
					uiNames[i] = "MotionDetected";
					break;
				case "prospect_recognized":
					uiNames[i] = "ProspectiveDetected";
					break;
				case "speech_recognized":
					uiNames[i] = "Answer";
					break;
				}
			}
			uiManager.RemoveUnusingUI(uiNames);

			string query = JsonMapper.ToObject(msg)["query"].ToString ();
			query = jsonRefine(query);
			Debug.Log ("Json query: " + query);
			JsonData queries = JsonMapper.ToObject(msg)["query"];
			for (int i = 0 ; i < events.Count ; i++) {
				switch (events[i].ToString ())
				{
				case "face_detected":
				{
					string detected = JsonMapper.ToObject(queries[i].ToString ())["detected"].ToString ();
					Debug.Log ("face_detected/detected: " + detected);
					uiManager.SetFaceDetected ( detected.Contains("True")?true:false );
					break;
				}
				case "motion_detected":
				{
					string detected = JsonMapper.ToObject(queries[i].ToString ())["detected"].ToString ();
					Debug.Log ("motion_detected/detected: " + detected);
					uiManager.SetMotionDetected (detected.Contains("True")?true:false);
					break;
				}
				case "prospect_recognized":
				{
					string prospect = JsonMapper.ToObject(queries[i].ToString ())["prospect"].ToString ();
					Debug.Log ("prospective_detected/prospect: " + prospect);
					uiManager.SetProspectRecognized (prospect.Contains("Positives")?true:false);
					break;
				}
				case "speech_recognized":
				{
					try {
					string answer = JsonMapper.ToObject(queries[i].ToString ())["recognized_word"].ToString ();
					Debug.Log ("speech_recognized/recognized_word: " + answer);
					uiManager.SetAnswerText (answer);
					}
					catch (Exception e) {
						uiManager.SetAnswerText ("");
					}
					break;
				}
				}
			}
//			uiManager.SetSpeechRecognized (0, "13", 0.8f);
//			uiManager.SetSpeechRecognized (1, "14", 0.75f);
//			uiManager.SetSpeechRecognized (2, "15", 0.6f);
//			uiManager.UpdateEnd ();

//			bool detected = JsonMapper.ToObject(query)["detected"];
//			faceDetected = (detected) ? true : false;
//			Debug.Log ("Json Parse: " + topic + ", detected: " + detected.ToString ());

//			string detected = JsonMapper.ToObject(query)["detected"].ToString ();
//			faceDetected = (detected == "True") ? true : false;
//			Debug.Log ("Json Parse: " + topic + ", detected: " + detected);
		}
		catch (Exception ex)
		{
			Debug.Log (ex.ToString ());
		}
	}

	string jsonRefine (string inString)
	{
		if (inString[0] == '"' && inString[inString.Length-1] == '"')
			inString = inString.Substring (1, inString.Length-2);
		return inString.Replace("\\\"", "\"");
	}
	
	public void Connect () {
		try 
		{
			if (!socket.Connected)
			{
#if true
				SetIpAddr(ipaddress.value);
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipaddress.value), port);
				socket.Connect(endpoint);
				if (socket.Connected)
				{
					UILabel label = connectButton.GetComponentInChildren<UILabel>() as UILabel;
					label.text = "Disconnect";
				}
				Send (rosSubscribe_RequestHidInput);
#endif
				mRun = true;
				Thread thread = new Thread (new ThreadStart (Process_Thread));
				thread.Start ();

			}
			else
			{
				Disconnect ();
				Application.LoadLevel(0);
			}
		}
		catch (Exception ex) 
		{
			Debug.Log (ex.ToString ());
		}
	}

	void Disconnect ()
	{
		mRun = false;
//		socket.Shutdown(SocketShutdown.Both);
		socket.Close ();
	}

	bool threadWriting = false;
	void Process_Thread () {
		byte[] bytes = new byte[2048];
		while (mRun)
		{
			#if false
			Thread.Sleep (1000);
			receivedMessage = rosReceivedMessage4;
			Debug.Log("Received: " + receivedMessage);
			Parsing (receivedMessage);
			#else
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
				Disconnect ();
			}
			#endif
			Thread.Sleep(1);
		}
	}

	public void Test () {
		string jsonString = @"{""state"":""1""}";
		Send (jsonString);
	}
	
	public void ArrowUp () {
		uiManager.SetFaceDetected (true);
	}
	public void ArrowDown () {
		uiManager.SetFaceDetected (false);
	}
	public void ArrowLeft () {
		Parsing (rosReceivedMessage5);
	}
	public void ArrowRight () {
		Parsing (rosReceivedMessage3);
//		uiManager.SetFaceDetected (true);
//		uiManager.SetMotionDetected (false);
//		uiManager.UpdateEnd ();
	}

	public void SendFaceDetectTrue() {
		Send (rosFaceDetectedHeader + "true" + rosFaceDetectedFooter);
	}
	public void SendFaceDetectFalse() {
		Send (rosFaceDetectedHeader + "false" + rosFaceDetectedFooter);
	}
	public void SendProspectRecognizedPositives() {
		Send (rosProspectRecognizedHeader + "positive" + rosProspectRecognizedFooter);
	}	
	public void SendProspectRecognizedNegatives() {
		Send (rosProspectRecognizedHeader + "negative" + rosProspectRecognizedFooter);
	}
	public void SendSpeechRecognized (string speech) {
		Send (rosSpeechRecognizedHeader + speech + rosSpeechRecognizedFooter);
//		Send (rosSpeechRecognizedHeader + ConvertToUTF8String(speech) + rosSpeechRecognizedFooter);
	}

	string ConvertToUTF8String(string str) {
		byte[] bytes = Encoding.Default.GetBytes (str);
//		return Convert.ToBase64String (bytes);
		return Encoding.UTF8.GetString (bytes);
	}

	public void ChangeRobotStateN(byte state, byte substate = 1) {
		ChangeRobotState (state, substate);
		byte imageCommand = (byte)(state * 10 + substate);
		WebCamClient.instance.SetImageCommand (imageCommand);
		Debug.Log ("Change state to : " + state.ToString () + "-" + substate.ToString ());
	}
	
	public void ChangeRobotState (byte state, byte substate) {
		string jsonString = @"{""state"":""";
		jsonString += state.ToString ();
		jsonString += @""", ""substate"":""";
		jsonString += substate.ToString ();
		jsonString += @"""}";
		Send (jsonString);
	}
	
	void Behavior (string str) {
		string jsonString = @"{""behavior"":""";
		jsonString += str;
		jsonString += @"""}";
		Send (jsonString);
	}
	
	void Send (string jsonString) {
		Debug.Log ("Send" + jsonString);
		try {
//			if (socket.Connected)
//			{
			socket.Send(Encoding.Default.GetBytes(jsonString + "\r\n"));
//			}
//			else
//			{
//				Debug.Log ("Socket is not connected");
//			}
		} catch (Exception e) {
			Debug.Log (e.ToString ());
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
		Application.LoadLevel (0);
	}
}

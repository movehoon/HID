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
	public UIInput ipaddress;
	public UIButton connectButton;
	int port = 9090;
	bool mRun = false;

	public UiManager uiManager;
	public Transform uiBoolPrefab;
	public Transform uiBool = null;

	string rosSubscribe_RequestHidInput = @"{""op"":""subscribe"", ""topic"":""memory_monitor/request_hid_input"",""type"":""memory_monitor/RequestHIDInput""}";
	string rosReceivedMessage3 = @"{""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": \""face_detected\"", \""query\"": \""\\\""{\\\\\\\""face_detected.detected\\\\\\\"": true}\\\""\""}"", ""header"": {""stamp"": {""secs"": 1426508304, ""nsecs"": 791610002}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}";
	string rosReceivedMessage4 = @"{""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": \""face_detected\"", \""query\"": \""\\\""{\\\\\\\""face_detected.detected\\\\\\\"": false}\\\""\""}"", ""header"": {""stamp"": {""secs"": 1426508304, ""nsecs"": 791610002}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}";

	string rosFaceDetectTrue  = @"{""op"":""call_service"", ""service"":""/memory_monitor/write_to_memory"", ""args"":{""data"": ""{'event_name': 'face_detector/face_detected', 'detected': 'true'}"", ""by"": ""HID""}}";
	string rosFaceDetectFalse = @"{""op"":""call_service"", ""service"":""/memory_monitor/write_to_memory"", ""args"":{""data"": ""{'event_name': 'face_detector/face_detected', 'detected': 'false'}"", ""by"": ""HID""}}";

	string receivedMessage = "";

	Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	bool faceDetected = true;
	
	void Start () {
		connectionDialog.SetActive (true);
		ipaddress.value = GetIpAddr ();
	}

	void Update () {
		UIFaceDetected (faceDetected);
		if (receivedMessage.Length > 0) {
			string parseText = receivedMessage;
			receivedMessage = "";
			Debug.Log ("Decode: " + parseText);
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
			Debug.Log ("length: " + jsonString.Length.ToString () + ", " + jsonString);
			JsonData json = JsonMapper.ToObject (jsonString);
			Debug.Log ("Json: " + jsonString);
			string topic = json ["topic"].ToString ();
			string msg = json ["msg"]["msg"].ToString ();
			Debug.Log ("Json msg: " + msg);
			string query = JsonMapper.ToObject(msg)["query"].ToString ();
			query = jsonRefine(query);
			Debug.Log ("Json query: " + query);
			string detected = JsonMapper.ToObject(query)["face_detected.detected"].ToString ();
			faceDetected = (detected == "True") ? true : false;
			Debug.Log ("Json Parse: " + topic + ", detected: " + detected);
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

	void UIFaceDetected (bool detected) {
		if (uiBool == null)
			return;

		uiBool.GetComponentInChildren<UiBoolManager> ().SetState (detected);
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
		// Send JSON
		uiManager.SetFaceRec (true);
//		if (uiBool == null)
//			uiBool = Instantiate(uiBoolPrefab) as Transform;
	}
	public void ArrowDown () {
		uiManager.SetFaceRec (false);
//		Destroy (uiBool.gameObject);
//		uiBool = null;
//		ChangeRobotStateN (13);
	}
	public void ArrowLeft () {
		Parsing (rosReceivedMessage3);
//		ChangeRobotStateN (14);
	}
	public void ArrowRight () {
		Parsing (rosReceivedMessage4);
//		ChangeRobotStateN (15);
	}

	public void SendFaceDetectTrue() {
//		Send (rosConnection);
		Send (rosFaceDetectTrue);
	}

	public void SendFaceDetectFalse() {
		Send (rosFaceDetectFalse);
//		Behavior ("bow");
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
		try {
//			if (socket.Connected)
//			{
			socket.Send(Encoding.Default.GetBytes(jsonString + "\r\n"));
			Debug.Log (jsonString);
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

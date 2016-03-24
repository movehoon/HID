using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LitJson;

public class MessageServer : MonoBehaviour {

	public GameObject [] messages;
//	public UILabel labelMode;
//	public UiPopupEmotionManager uiEmotion;

	int currentHIDMode = 1;

	public TcpListener server = null;
	Thread threadListen;
	bool mRunning;

	NetworkStream stream;
	string receivedMessage = "";

	string rosReceivedMessage1 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""face_detected\""], \""query\"": [\""{\\\""detected\\\"":true}\""]}"", ""header"": {""stamp"": {""secs"": 1429181797, ""nsecs"": 859826087}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}/r/f";
	string rosReceivedMessage2 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""face_detected\"", \""motion_detected\""], \""query\"": [\""{\\\""motion_detected.detected\\\"": true, \\\""face_detected.detected\\\"": true}\\\""]}"", ""header"": {""stamp"": {""secs"": 1428656219, ""nsecs"": 865901947}, ""frame_id"": "" "", ""seq"": 42}}, ""op"": ""publish""}/r/f";
	string rosReceivedMessage3 = @" {""topic"": ""/memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""face_detected\"", \""prospect_recognized\""], \""query\"": [\""{\\\""detected\\\"":true}\"", \""{\\\""prospect\\\"":\\\""positive\\\""}\""]}"", ""header"": {""stamp"": {""secs"": 1429246568, ""nsecs"": 713021039}, ""frame_id"": "" "", ""seq"": 1}}, ""op"": ""publish""}/r/f";
//	string rosReceivedMessage4 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""speech_recognized\""], \""query\"": [\""{\\\""recognized_word\\\"":[\\\""11\\\"", \\\""12\\\"", \\\""13\\\"", \\\""14\\\""], \\\""confidence\\\"":[0.8, 0.7, 0.6, 0.9]}\""]}"", ""header"": {""stamp"": {""secs"": 1429181797, ""nsecs"": 859826087}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}/r/f";
	string rosReceivedMessage5 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""speech_recognized\""], \""query\"": [\""{\\\""recognized_word\\\"":\\\""\\uc548\\ub155\\\""}\""]}"", ""header"": {""stamp"": {""secs"": 1429181797, ""nsecs"": 859826087}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}";

	int count = 0;
	public void ClickSend () {
		if (server == null)
			return;
		TextAsset textAsset = Resources.Load ("FaceDetectedTrue") as TextAsset;
		Send (textAsset.text);
//		count++;
//		if (count > 2)
//			count = 0;
//		switch (count) {
//		case 0:
//			Send (rosReceivedMessage2);
//			break;
//		case 1:
//			TextAsset textAsset = Resources.Load ("FaceDetectedTrue") as TextAsset;
//			Send (textAsset.text);
//			break;
//		case 2:
//			Send (rosReceivedMessage4);
//			break;
//		}
	}

	const string eventNameFaceDetected = @"\""face_detected\""";
	const string eventNameProspectRecognized = @"\""prospect_recognized\""";
	const string eventNameSpeechRecognized = @"\""speech_recognized\""";
	const string eventNameCheckSpeechRecognized = @"\""check_speech_recognized\""";

	const string queryFaceDetectedTrue  = @"\""{\\\""detected\\\"":true}\""";
	const string queryFaceDetectedFalse = @"\""{\\\""detected\\\"":false}\""";

	const string queryProspectRecognizedPositive  = @"\""{\\\""prospect\\\"":\\\""Positive\\\""}\""";
	const string queryProspectRecognizedNegative = @"\""{\\\""prospect\\\"":\\\""Negative\\\""}\""";

	const string querySpeechRecognizedAnswerHeader  = @"\""{\\\""recognized_word\\\"": \\\""";
	const string querySpeechRecognizedAnswerFooter  = @"\\\""}\""";

	const string querySpeechRecognized  = @"\""{\\\""detected\\\"":true}\""";

	const string queryCheckSpeechRecognizedHeader  = @"\""{\\\""confidence\\\"": ";
	const string queryCheckSpeechRecognizedMiddle  = @", \\\""recognized_word\\\"": \\\""";
	const string queryCheckSpeechRecognizedFooter  = @"\\\""}\""";
	
	string rosReceivedMessage4 = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [\""check_speech_recognized\""], \""query\"": [\""{\\\""recognized_word\\\"":[\\\""11\\\"", \\\""12\\\"", \\\""13\\\"", \\\""14\\\""], \\\""confidence\\\"":[0.8, 0.7, 0.6, 0.9]}\""]}"", ""header"": {""stamp"": {""secs"": 1429181797, ""nsecs"": 859826087}, ""frame_id"": "" "", ""seq"": 2}}, ""op"": ""publish""}";

	public void SendStateMessage () {
//		string eventName = "";
//		string query = "";
//		UIToggle toggleFace = messages[0].GetComponentInChildren<UIToggle> ();
//		if (toggleFace.value) {
//			eventName += eventNameFaceDetected + ", ";
//			UiFaceDetectedManager face = messages[0].GetComponentInChildren<UiFaceDetectedManager> ();
//			if (face.IsDetected ())
//				query += queryFaceDetectedTrue + ", ";
//			else
//				query += queryFaceDetectedFalse + ", ";
//		}
//
//		UIToggle toggleProspect = messages[1].GetComponentInChildren<UIToggle> ();
//		if (toggleProspect.value) {
//			eventName += eventNameProspectRecognized + ", ";
//			UiProspectRecognizedManager prospect = messages[1].GetComponentInChildren<UiProspectRecognizedManager> ();
//			if (prospect.IsPositive ())
//				query += queryProspectRecognizedPositive + ", ";
//			else
//				query += queryProspectRecognizedNegative + ", ";
//		}
//		
//		UIToggle toggleAnswer = messages[2].GetComponentInChildren<UIToggle> ();
//		if (toggleAnswer.value) {
//			eventName += eventNameSpeechRecognized + ", ";
//			UiAnswer answer = messages[2].GetComponentInChildren<UiAnswer> ();
//			query += querySpeechRecognizedAnswerHeader + answer.GetText () + querySpeechRecognizedAnswerFooter + ", ";
//		}
//
//		UIToggle toggleSpeech = messages[3].GetComponentInChildren<UIToggle> ();
//		if (toggleSpeech.value) {
//			eventName += eventNameCheckSpeechRecognized + ", ";
//			UiSpeechRecognizedManager speech = messages[3].GetComponentInChildren<UiSpeechRecognizedManager> ();
//			query += queryCheckSpeechRecognizedHeader + speech.labelConfidence[0].text + queryCheckSpeechRecognizedMiddle + speech.labelSpeech[0].text + queryCheckSpeechRecognizedFooter + ", ";
//		}
//
//		eventName += @"\""reserved\""";
//		query += @"\""{\\\""reserved\\\"":false}\""";
//
//		string message = MakeMessage (eventName, query);
//		Send (message);
	}

	string messageHeader = @" {""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": [";
	string messageMiddle = @"], \""query\"": [";
	string messageFooter = @"]}"", ""header"": {""stamp"": {""secs"": 1428656219, ""nsecs"": 865901947}, ""frame_id"": "" "", ""seq"": 42}}, ""op"": ""publish""}";
//	string messageFooter = @"]}""";
	string MakeMessage (string eventName, string query) {
		string message = messageHeader + eventName + messageMiddle + query + messageFooter;
		Debug.Log (message);
		return message;
	}

	string emotionHeader = @"{""emotion"":{""pleasure"":";
	string emotionMiddle = @", ""arrousal"":";
	string emotionFooter = @"}}";
	public void SendEmotionPosition(float pleasure, float arrousal) {
		string message = emotionHeader + pleasure.ToString () + emotionMiddle + arrousal.ToString () + emotionFooter;
		Debug.Log ("emotion message: " + message);
		Send (emotionHeader + pleasure.ToString () + emotionMiddle + arrousal.ToString () + emotionFooter);
	}

	void Send (string message) {
		if (stream == null)
			return;
		byte [] bytes = Encoding.Default.GetBytes (message);
		stream.Write (bytes, 0, bytes.Length);
		Debug.Log ("Send: " + message);
	}

//	static byte[] GetBytes(string str)
//	{
//		byte[] bytes = new byte[str.Length * sizeof(char)];
//		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
//		return bytes;
//	}

	void Awake () {
		threadListen = new Thread(new ThreadStart(TcpListener_Co));
		threadListen.Start();
	}

	void Start () {
//		uiEmotion.transform.GetComponentInChildren<TweenPosition> ().PlayReverse ();
	}

	void Update () {
//		if (receivedMessage.Length > 0) {
//			receivedMessage = jsonRefine (receivedMessage);
//			try {
//				JsonData json = JsonMapper.ToObject (receivedMessage);
//				string mode = json ["mode"].ToString ();
//				switch (mode) {
//				case "0":
//					labelMode.text = "HID: Mode0";
//					currentHIDMode = 0;
//					break;
//				case "1":
//					labelMode.text = "HID: Mode1";
//					currentHIDMode = 1;
//					break;
//				case "2":
//					labelMode.text = "HID: Mode2";
//					currentHIDMode = 2;
//					break;
//				case "3":
//					labelMode.text = "HID: Mode3";
//					currentHIDMode = 3;
//					break;
//				}
//			}
//			catch (Exception e) {
//				Debug.Log ("Exception on mode: " + e.ToString ());
//			}
//
//			try {
//				JsonData json = JsonMapper.ToObject (receivedMessage);
//				string pleasure = json ["emotion"]["pleasure"].ToString ();
//				string arrousal = json ["emotion"]["arrousal"].ToString ();
//				uiEmotion.SetThumbPosition (float.Parse(pleasure), float.Parse(arrousal));
//				Debug.Log ("PA: " + pleasure + ", " + arrousal);
//			}catch (Exception e) {
//				Debug.Log ("Exception on emotion: " + e.ToString ());
//			}
//			receivedMessage = "";
//		}
//
//		if (currentHIDMode != 2) {
//			UIToggle toggleSpeech = messages[3].GetComponentInChildren<UIToggle> ();
//			if (toggleSpeech.value) {
//				toggleSpeech.value = false;
//			}
//		}
	}

	string jsonRefine (string inString)
	{
		if (inString[0] == '"' && inString[inString.Length-1] == '"')
			inString = inString.Substring (1, inString.Length-2);
		return inString.Replace("\\\"", "\"");
	}

	void TcpListener_Co () {
		mRunning = true;
		try {
			server = new TcpListener (IPAddress.Any, 9090);
			server.Start ();
		}
		catch (Exception ex) {
			Debug.Log (ex.ToString ());
		}
		while (mRunning) 
		{
			Debug.Log ("[server] Waiting for a connection... ");
			if (!server.Pending ())
			{
				Thread.Sleep (100);
			}
			else
			{
				TcpClient client = server.AcceptTcpClient ();
				Debug.Log ("[Server] Connected");
				stream = client.GetStream ();
				stream.ReadTimeout = 10;
				stream.WriteTimeout = 10;
				stream.Flush ();
				byte[] bytes = new byte[2048];
				while (client.Connected) {
					try {
						int nRead = stream.Read (bytes, 0, 2048);
						if (nRead <= 0)
						{
							client.Close ();
						}
						lock (receivedMessage)
						{
							receivedMessage = Encoding.Default.GetString (bytes);
							receivedMessage = receivedMessage.Substring(0, nRead);
							Debug.Log ("Received: " + receivedMessage);
						}
					} catch (Exception ex) {
						Debug.Log (ex.ToString ());
						continue;
					}
				}
				client.Close ();
				Debug.Log ("[server] client disconnected");
			}
			Thread.Sleep (10);
		}
	}
}

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

	public Transform uiBoolPrefab;
	public Transform uiBool = null;

	string rosConnection = @"{""op"":""subscribe"", ""topic"":""memory_monitor/request_hid_input"",""type"":""memory_monitor/RequestHIDInput""}";
	string rosCallService = @"{""op"":""call_service"", ""service"":""memory_monitor/write_to_memory"",""args"":""memory_monitor/RequestHIDInput""}";
	string rosReceivedMessage = @"{""topic"": ""memory_monitor/request_hid_input"", ""msg"": {""msg"": ""{\""event_name\"": face_detector/face_detected, \""query\"": {\""detected\"": true}"", ""header"": {""stamp"": {""secs"": 1426150585, ""nsecs"": 582812070}, ""frame_id"": "" "", ""seq"": 1}}, ""op"": ""publish""}";

	Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	
	void Start () {
		connectionDialog.SetActive (true);
		ipaddress.value = GetIpAddr ();
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
				SetIpAddr(ipaddress.value);
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipaddress.value), port);
				socket.Connect(endpoint);
				if (socket.Connected)
				{
					UILabel label = connectButton.GetComponentInChildren<UILabel>() as UILabel;
					label.text = "Disconnect";
				}
				Send (rosConnection);

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

	void Process_Thread () {
		byte[] bytes = new byte[2048];
		while (mRun)
		{
			if (socket.Connected)
			{
				int nRead = socket.Receive(bytes);
				Debug.Log(Encoding.UTF8.GetString(bytes));
				if (nRead <= 0)
				{
					socket.Close ();
				}
			}
			else
			{
				Debug.Log ("Socket is disconnected");
				Disconnect ();
			}
			Thread.Sleep(1);
		}
	}

	public void Test () {
		string jsonString = @"{""state"":""1""}";
		Send (jsonString);
	}
	
	public void ArrowUp () {
		// Send JSON
		if (uiBool == null)
			uiBool = Instantiate(uiBoolPrefab) as Transform;
	}
	public void ArrowDown () {
		Destroy (uiBool.gameObject);
		uiBool = null;
//		ChangeRobotStateN (13);
	}
	public void ArrowLeft () {
		ChangeRobotStateN (14);
	}
	public void ArrowRight () {
		ChangeRobotStateN (15);
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

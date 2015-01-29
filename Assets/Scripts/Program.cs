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

public class Program : MonoBehaviour {

	public const string Key_IpAddr = "Key_IpAddr";

	public GameObject connectionDialog;
	public UIInput ipaddress;
	public UIInput speakText;
	public UIButton connectButton;
	public UIInput nState;
	int port = 2223;

	Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	void Start () {
		connectionDialog.SetActive (true);
		ipaddress.value = GetIpAddr ();
//		socket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
		MakeXml ();
	}

	public void Connect () {
		try 
		{
			if (!socket.Connected)
			{
				SetIpAddr(ipaddress.value);
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipaddress.value), port);
				socket.Connect(endpoint);
				UILabel label = connectButton.GetComponentInChildren<UILabel>() as UILabel;
				label.text = "Disconnect";
			}
			else
			{
				socket.Shutdown(SocketShutdown.Both);
				socket.Disconnect (true);
				Application.LoadLevel(0);
			}
		}
		catch (Exception ex) 
		{
			Debug.Log (ex.ToString ());
		}
	}

	public void Speak () {
		string jsonString = @"{""dialog"":""";
		jsonString += speakText.value;
		jsonString += @"""}";
		Send (jsonString);
	}

	public void Behavior1 () {
		Behavior ("behavior1");
	}
	
	public void Behavior2 () {
		Behavior ("behavior2");
	}
	
	public void Behavior3 () {
		Behavior ("behavior3");
	}
	
	public void Behavior4 () {
		Behavior ("behavior4");
	}
	
	public void Behavior5 () {
		Behavior ("behavior5");
	}

	public void ChangeRobotStateN() {
		int n = Convert.ToInt32(nState.value);
		ChangeRobotState (n);
	}

	public void ChangeRobotState0 () {
		ChangeRobotState (0);
		WebCamClient.instance.SetImageCommand (1);
	}

	public void ChangeRobotState1 () {
		ChangeRobotState (1);
		WebCamClient.instance.SetImageCommand (2);
	}
	
	public void ChangeRobotState2 () {
		ChangeRobotState (2);
		WebCamClient.instance.SetImageCommand (1);
	}
	
	public void ChangeRobotState3 () {
		ChangeRobotState (3);
		WebCamClient.instance.SetImageCommand (1);
	}
	
	public void ChangeRobotState4 () {
		ChangeRobotState (4);
		WebCamClient.instance.SetImageCommand (3);
	}
	
	public void ChangeRobotState5 () {
		ChangeRobotState (5);
		WebCamClient.instance.SetImageCommand (1);
	}
	
	public void ChangeRobotState6 () {
		ChangeRobotState (6);
		WebCamClient.instance.SetImageCommand (1);
	}
	
	public void ChangeRobotState7 () {
		ChangeRobotState (7);
		WebCamClient.instance.SetImageCommand (1);
	}
	
	public void ChangeRobotState8 () {
		ChangeRobotState (8);
		WebCamClient.instance.SetImageCommand (1);
	}
	
	public void ChangeRobotState9 () {
		ChangeRobotState (9);
		WebCamClient.instance.SetImageCommand (1);
	}

	public void ChangeRobotState (int id) {
		string jsonString = @"{""state"":""";
		jsonString += id.ToString ();
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
		Debug.Log (jsonString);
		if (socket.Connected)
			socket.Send(Encoding.Default.GetBytes(jsonString + "\r\n"));
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

	public void MakeXml()
	{
		Dictionary<string, string> scenario = new Dictionary<string, string> ();

//		string json = @"
//			{""state"":
//				{""id"":""s0"",
//				 ""name"":""Ready""},
//			{""action"":
//				{""tts"":""hello"",
//				 ""behavior"":""behav0""},
//			[{""s1"":"".""}]
//			}";
//		JsonData jsonData = JsonMapper.ToObject (json);
//		XmlSerializer xmlSerializer = new XmlSerializer ();
	}

	public void ReloadScene ()
	{
		Application.LoadLevel (0);
	}
}

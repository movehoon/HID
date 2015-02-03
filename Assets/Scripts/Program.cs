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
//	public UIInput speakText;
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

	public void Test () {
		string jsonString = @"{""state"":""1""}";
		Send (jsonString);
	}

	public void ArrowUp () {
		ChangeRobotStateN (12);
	}
	public void ArrowDown () {
		ChangeRobotStateN (13);
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

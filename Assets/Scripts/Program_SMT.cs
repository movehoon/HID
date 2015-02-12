using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class Program_SMT : MonoBehaviour {

	public const string Key_IpAddr = "Key_IpAddr";
	public UIInput ipInput;
	public UIButton connectButton;
	int port = 6000;
	
	Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	// Use this for initialization
	void Start () {
		ipInput.value = GetIpAddr ();
		Debug.Log ("Start Program");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Ready ()
	{
		if (socket.Connected) 
		{
//			byte[] buff = System.Text.Encoding.ASCII.GetBytes("R");
			byte[] buff = new byte[1];
			buff[0] = 82;
			socket.Send (buff);
		}
	}

	public void Go ()
	{
		if (socket.Connected) 
		{
//			byte[] buff = System.Text.Encoding.ASCII.GetBytes("G");
			byte[] buff = new byte[1];
			buff[0] = 71;
			socket.Send (buff);
		}
	}

	public void Connect () {
		try 
		{
			if (!socket.Connected)
			{
				SetIpAddr(ipInput.value);
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipInput.value), port);
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
				UILabel label = connectButton.GetComponentInChildren<UILabel>() as UILabel;
				label.text = "Connect";
			}
		}
		catch (Exception ex) 
		{
			Debug.Log (ex.ToString ());
		}
	}

	public void Reset ()
	{
		Application.LoadLevel (0);
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
}

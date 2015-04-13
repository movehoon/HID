using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ConnectionManager : MonoBehaviour {

	public const string Key_IpAddr = "Key_IpAddr";
	int port = 2223;
	bool mRun = false;

	public UIInput ipaddress;
	public Transform connectionWindow;

	public UILabel labelXPos;
	public UILabel labelYPos;

	public UISprite spriteRobot;

	Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	public void OpenConnectionWindow () {
		Instantiate (connectionWindow);
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

	UcrManager ucrManager = new UcrManager ();
	public void Connect () {
		try 
		{
			if (!socket.Connected)
			{
				SetIpAddr(ipaddress.value);
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(GetIpAddr()), port);
				socket.Connect(endpoint);
				if (socket.Connected)
				{
//					UILabel label = connectButton.GetComponentInChildren<UILabel>() as UILabel;
//					label.text = "Disconnect";
					mRun = true;
					Thread thread = new Thread (new ThreadStart (Process_Thread));
					thread.Start ();
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

	public void Localize ()
	{
		byte[] buff = new byte[7];
		buff[0] = (byte)0xaa;
		buff[1] = (byte)0x05;
		buff[2] = (byte)0x55;
		buff[3] = (byte)0x00;
		buff[4] = (byte)0x01;
		buff[5] = (byte)0x00;
		buff[6] = getChecksum(buff);
		socket.Send (buff);
	}

	public void Navigate ()
	{
		byte[] buff = new byte[7];
		buff[0] = (byte)0xaa;
		buff[1] = (byte)0x05;
		buff[2] = (byte)0x55;
		buff[3] = (byte)0x00;
		buff[4] = (byte)0x02;
		buff[5] = (byte)0x00;
		buff[6] = getChecksum(buff);
		socket.Send (buff);
	}
	byte getChecksum(byte[] buff) {
		byte checksum = 0;
		for (int i = 2 ; i < buff[1]+1 ; i++)
			checksum += buff[i];
		return (byte) (0-checksum);
	}


	void Process_Thread () {
		byte[] bytes = new byte[1024];
		while (mRun)
		{
			if (socket.Connected)
			{
				int nRead = socket.Receive(bytes);
				if (nRead <= 0)
				{
					socket.Close ();
				}
				ucrManager.Decode (bytes, nRead);
//				Debug.Log("length: " + nRead + ", Received: " + BitConverter.ToString (bytes));
			}
			else
			{
				Debug.Log ("Socket is disconnected");
//				Disconnect ();
			}
			Thread.Sleep(1);
		}
	}

	// Use this for initialization
	void Start () {
		ipaddress.value = GetIpAddr ();
	}

	void SetRobotPositionX (int x)
	{
		if (x < 0 || x > 40)
			return;
		Vector3 pos = spriteRobot.transform.localPosition;
		int coordinatedPos = (x - 20) * 20;
		pos.x = coordinatedPos;
		spriteRobot.transform.localPosition = pos;
	}
	void SetRobotPositionY (int y)
	{
		if (y < 0 || y > 40)
			return;
		Vector3 pos = spriteRobot.transform.localPosition;
		int coordinatedPos = (20 - y) * 20;
		pos.y = coordinatedPos;
		spriteRobot.transform.localPosition = pos;
	}

	// Update is called once per frame
	void Update () {
		while (ucrManager.Count() > 0)
		{
			UcrManager.UcrData ucrData = ucrManager.Pop ();
			switch (ucrData.type)
			{
			case 0x51:	// Navigation message
				switch (ucrData.id)
				{
				case 0x01:
					labelXPos.text = "X : " + ucrData.data.ToString ();
					SetRobotPositionX (ucrData.data);
					break;
				case 0x02:
					labelYPos.text = "Y : " + ucrData.data.ToString ();
					SetRobotPositionY (ucrData.data);
					break;
				}
				break;
			}
			Debug.Log ("Found message");
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

}

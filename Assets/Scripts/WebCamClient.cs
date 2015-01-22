using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class WebCamClient : MonoBehaviour {
	public const string Key_CamIpAddr = "Key_CamIpAddr";
	public UIInput camIpAddr;
	public UIButton connectButton;
	int camPort = 3003;
	
	enum STATE_RECV {
		READY,
		GET_LENGTH,
		GET_IMG,
		Fail,
	};
	
	Texture2D texture;

	TcpClient client;
	byte[] imgBytes;
	bool imgReceived = false;
	bool mRun = false;
	string webCamIpAddr;
	
	// Use this for initialization
	void Start () {
		texture = new Texture2D (Webcam.WEBCAM_WIDTH/Webcam.ratio, Webcam.WEBCAM_HEIGHT/Webcam.ratio);
		camIpAddr.value = GetCamIpAddr ();
	}
	
	void Update () {
		if (imgReceived) 
		{
			DrawImg (imgBytes);
			imgReceived = false;
		}
	}
	
	public void DrawImg (byte[] img) {
		texture.LoadImage (img);
		texture.Apply ();
		UITexture sprite = GetComponentInChildren<UITexture> ();
		sprite.material.mainTexture = texture;
		sprite.MakePixelPerfect();
		//		renderer.material.mainTexture = texture;
		//		File.WriteAllBytes ("dst.png", png);
	}

	public void ConnectToServer () {
		SetCamIpAddr(camIpAddr.value);
		webCamIpAddr = GetCamIpAddr ();
		if (client == null || !client.Connected)
		{
			Debug.Log ("[client]ConnectToServer");
			mRun = true;
			Thread thread = new Thread (new ThreadStart (ConnectToServer_Thread));
			thread.Start ();
			UILabel label = connectButton.GetComponentInChildren<UILabel> ();
			label.text = "Disconnect";
		}
		else
		{
			if (Disconnect () == true)
			{
				UILabel label = connectButton.GetComponentInChildren<UILabel> ();
				label.text = "Connect";
			}
		}
	}
	
	public bool Disconnect () {
		mRun = false;
		Thread.Sleep(10);
		if (!client.Connected)
			return true;
		return false;
	}

	public bool IsConnected () {
		try {
			if (client.Connected)
				return true;
		}
		catch (Exception e)  {
			Debug.Log ("WebCamClient::IsConnected -> " + e.ToString ());
		}
		return false;
	}
	
	void ConnectToServer_Thread () {
		Debug.Log ("StartThread");
		STATE_RECV state = STATE_RECV.READY;
		int imgLength = 0;
		int nRead;
		DateTime startTime;
		
		client = new TcpClient ();
		Debug.Log ("StartThread 2 " + webCamIpAddr);
		client.Connect (IPAddress.Parse (webCamIpAddr), camPort);
		NetworkStream nNetStream = client.GetStream ();
		nNetStream.ReadTimeout = 10;
		Debug.Log ("[client]Connected");
		while (mRun && client.Connected)
		{
			if (nNetStream.CanRead)
			{
				switch (state)
				{
				case STATE_RECV.READY:
				{
					if (!imgReceived) 
					{
						Debug.Log ("[client]READY");
						nNetStream.WriteByte (100);
						nNetStream.Flush ();
						state = STATE_RECV.GET_LENGTH;
					}
					break;
				}
				case STATE_RECV.GET_LENGTH:
				{
					Debug.Log ("[client]GET_LENGTH");
					byte[] bytes = new byte[4];
					try
					{
						nRead = nNetStream.Read (bytes, 0, 4);
					}
					catch (Exception ex)
					{
						Debug.Log (ex.ToString ());
						continue;
					}
					imgLength = BitConverter.ToInt32(bytes, 0);
					Debug.Log ("[client] img length " + imgLength.ToString ());
					if (imgLength < 0 || imgLength > 1000000)
					{
						state = STATE_RECV.Fail;
					}
					else 
					{
						startTime = DateTime.Now;
						state = STATE_RECV.GET_IMG;
					}
					break;
				}
				case STATE_RECV.GET_IMG:
				{
					Debug.Log ("[client]GET_IMG");
					if (client.Available < imgLength) {
						if ((DateTime.Now - startTime).Milliseconds > 10)
							state = STATE_RECV.Fail;
						continue;
					}
					imgBytes = new byte[imgLength];
					try 
					{
						nRead = nNetStream.Read (imgBytes, 0, imgLength);
					}
					catch (IOException ex)
					{
						Debug.Log (ex.ToString ());
						continue;
					}
					if (nRead == imgLength)
						imgReceived = true;
					else
						Thread.Sleep (100);
					Debug.Log ("[client] get image " + imgLength.ToString ());
					state = STATE_RECV.READY;
					break;
				}
				case STATE_RECV.Fail:
				{
					Debug.Log ("[client] Fail");
					Thread.Sleep (100);
					int nTrash = client.Available;
					byte[] trash = new byte[nTrash];
					nNetStream.Read (trash, 0, nTrash);
					state = STATE_RECV.READY;
					break;
				}
				}
			}
			else
			{
				break;
			}
			Thread.Sleep (1);
		}
		nNetStream.Close();
		client.Close();
		client.Client.Close ();
		Debug.Log ("[client]Disconnected");
	}
	void OnApplicationQuit () {
		if (client != null && client.Connected)
			client.Close ();
	}
	static public string GetCamIpAddr()
	{
		if (PlayerPrefs.HasKey(Key_CamIpAddr) == false)
		{
			return "192.168.0.3";
		}
		return PlayerPrefs.GetString(Key_CamIpAddr);
	}
	static public void SetCamIpAddr(string ipAddr)
	{
		PlayerPrefs.SetString(Key_CamIpAddr, ipAddr);
	}
}

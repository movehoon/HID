using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ImgReceiver : MonoBehaviour {

	enum STATE_RECV {
		GET_LENGTH,
		GET_IMG,
	};

	Texture2D texture;

	TcpClient client = new TcpClient ();
//	Socket client = null;
	byte[] imgBytes;
	bool imgReceived = false;

	Thread thread;

	// Use this for initialization
	void Start () {
		texture = new Texture2D (Webcam.WEBCAM_WIDTH/Webcam.ratio, Webcam.WEBCAM_HEIGHT/Webcam.ratio);
	}

	void Update () {
		if (imgReceived) 
		{
			DrawImg (imgBytes);
		}
	}

	public void DrawImg (byte[] img) {
		texture.LoadImage (img);
		texture.Apply ();
		renderer.material.mainTexture = texture;
//		File.WriteAllBytes ("dst.png", png);
	}

	public void ConnectToServer () {
		Debug.Log ("[client]ConnectToServer");
		ThreadStart ts = new ThreadStart (ConnectToServer_Thread);
		thread = new Thread (ts);
		thread.Start ();
	}

	void ConnectToServer_Thread () {
		STATE_RECV state = STATE_RECV.GET_LENGTH;
		byte inByte;

		client.Connect (IPAddress.Parse ("127.0.0.1"), 3003);
		NetworkStream nNetStream = client.GetStream ();
		Debug.Log ("[client]Connected...");
		nNetStream.WriteByte (100);
		while (client.Connected)
		{
			if (nNetStream.CanRead)
			{
				byte[] bytes = new byte[4];
				nNetStream.Read (bytes, 0, 4);
				int imgLength = BitConverter.ToInt32(bytes, 0);
				Debug.Log ("[client] get " + imgLength.ToString ());
				imgBytes = new byte[imgLength];
				nNetStream.Read (imgBytes, 0, imgLength);
				imgReceived = true;
				break;
			}
			else
			{
				client.Close();
				nNetStream.Close();
			}
			Thread.Sleep (1);
		}
		client.Close();
	}
	void OnApplicationQuit () {
		client.Close ();
	}
}

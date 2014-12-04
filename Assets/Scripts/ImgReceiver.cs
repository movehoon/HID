using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ImgReceiver : MonoBehaviour {

	Texture2D texture;

	TcpClient client = new TcpClient ();
//	Socket client = null;

	Thread thread;

	// Use this for initialization
	void Start () {
		texture = new Texture2D (Webcam.WEBCAM_WIDTH/Webcam.ratio, Webcam.WEBCAM_HEIGHT/Webcam.ratio);
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
		byte inByte;

		client.Connect (IPAddress.Parse ("127.0.0.1"), 3003);
		NetworkStream nNetStream = client.GetStream ();
		Debug.Log ("[client]Connected...");
		nNetStream.WriteByte (0);
		while (client.Connected)
		{
			byte[] bytes = new byte[client.ReceiveBufferSize];
			if (nNetStream.CanRead)
			{
				inByte = (byte)nNetStream.ReadByte ();
				Debug.Log ("[client] get " + inByte.ToString ());
				nNetStream.WriteByte (++inByte);
			}
			else
			{
				client.Close();
				nNetStream.Close();
			}
			Thread.Sleep (10);
		}
		client.Close();
	}
	void OnApplicationQuit () {
		client.Close ();
	}
}

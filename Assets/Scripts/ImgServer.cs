using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ImgServer : MonoBehaviour {

	public const int WEBCAM_WIDTH = 1280;
	public const int WEBCAM_HEIGHT = 720;
	public const int ratio = 8;

	public const string ipAddr = "127.0.0.1";

	public GameObject receiverObject;
	
	WebCamTexture webcam = null;
	static Texture2D texture;
	static byte[] imgBytes;

	TcpListener server = null;
//	Socket server = null;
	Thread thread;
	bool mRunning;

	void Awake () {
//		StartCoroutine ("TcpListener_Co");
		mRunning = true;
		try {
			server = new TcpListener (IPAddress.Any, 3003);
			server.Start ();
		}
		catch (Exception ex) {
			Debug.Log (ex.ToString ());
		}
		thread = new Thread(new ThreadStart(TcpListener_Co));
		thread.Start();
	}

	// Use this for initialization
	void Start () {
		webcam = new WebCamTexture (WEBCAM_WIDTH, WEBCAM_HEIGHT, 10);
		webcam.Play ();
		if (webcam != null)
			texture = new Texture2D (WEBCAM_WIDTH/ratio, WEBCAM_HEIGHT/ratio);
	}

	void TcpListener_Co () {
//	IEnumerator TcpListener_Co () {
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
				NetworkStream stream = client.GetStream ();
				stream.ReadTimeout = 10;
				stream.Flush ();
				while (client.Connected) {
					byte inByte;
					try {
						inByte = (byte)stream.ReadByte ();
					} catch (Exception ex) {
						Debug.Log (ex.ToString ());
						continue;
					}
					Debug.Log ("[server] get: " + inByte.ToString ());
					if (inByte == 100) {
						lock (imgBytes)
						{
							byte[] intBytes = BitConverter.GetBytes(imgBytes.Length);
							stream.Write(intBytes, 0, intBytes.Length);
							stream.Write (imgBytes, 0, imgBytes.Length);
							stream.Flush ();
						}
						Debug.Log ("[server] send count: " + imgBytes.Length.ToString ());
						Thread.Sleep (10);
					}
				}
				Debug.Log ("[server] client disconnected");
			}
			Thread.Sleep (10);
		}
//		yield return null;
	}

	// Update is called once per frame
	void Update () {
		if (!webcam.isPlaying)
			return;
		
		renderer.material.mainTexture = webcam;
		if (webcam.width == WEBCAM_WIDTH && webcam.height == WEBCAM_HEIGHT) 
		{
			for (int y = 0 ; y < webcam.height ; y+=ratio)
			{
				for (int x = 0 ; x < webcam.width ; x+=ratio)
				{
					Color color = webcam.GetPixel(x, y);
					texture.SetPixel(x/ratio, y/ratio, color); 
				}
			}
			//			texture.SetPixels (webcam.GetPixels ());
			texture.Apply ();
			imgBytes = texture.EncodeToJPG ();
			//			File.WriteAllBytes("src.png", webcamToPng);
//			Debug.Log ("Webcam is (" + webcam.width/ratio + ", " + webcam.height/ratio + ") with size " + imgBytes.GetLength (0) + "bytes");
			
//			ImgReceiver imgReceiver = receiverObject.GetComponentInChildren <ImgReceiver> () as ImgReceiver;
//			imgReceiver.DrawImg (imgBytes);

		}
	}

	private static int SendBuffer (Socket s, byte[] buff) {
		int total = 0;
		int size = buff.Length;
		int dataleft = size;
		int sent;

		byte[] datasize = new byte[0];
		datasize = BitConverter.GetBytes (size);
		sent = s.Send (datasize);

		while (total < size) 
		{
			sent = s.Send (buff, total, dataleft, SocketFlags.None);
			total += sent;
			dataleft -= sent;
		}

		return total;
	}

	public void stopListening() {
		mRunning = false;
		server.Stop ();
	}

	void OnApplicationQuit () {
		stopListening();
	}
}

using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ImgServer : MonoBehaviour {

	public const int WEBCAM_WIDTH = 1280;
	public const int WEBCAM_HEIGHT = 720;
	public const int ratio = 4;

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
		ThreadStart ts = new ThreadStart(TcpListener_Co);
		thread = new Thread(ts);
		thread.Start();
	}

	// Use this for initialization
	void Start () {
		webcam = new WebCamTexture (WEBCAM_WIDTH, WEBCAM_HEIGHT, 10);
		webcam.Play ();
		if (webcam != null)
			texture = new Texture2D (WEBCAM_WIDTH/ratio, WEBCAM_HEIGHT/ratio);
	}

//	IEnumerator ServerListen () {
//		IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3003);
//		server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//		try
//		{
//			server.Bind (ipep);
//			server.Listen (2);
//		}
//		catch (SocketException e)
//		{
//			Debug.Log(e.ToString());
//		}
//		while (true) 
//		{	
//			Socket client;
//			try {
//				client = server.Accept ();
//				if (client.Connected)
//					SendBuffer(client, imgBytes);
//			}
//			catch (Exception ex) {
//				Debug.Log (ex.ToString ());
//			}
//			yield return new WaitForSeconds (0.01f);
//		}
//	}

	void TcpListener_Co () {
//	IEnumerator TcpListener_Co () {
		server = new TcpListener (IPAddress.Parse("127.0.0.1"), 3003);
		server.Start ();
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
				byte inByte;
				while (client.Connected) {
					inByte = (byte)stream.ReadByte ();
					Debug.Log ("[server] get: " + inByte.ToString ());
					stream.WriteByte(++inByte);
					Thread.Sleep (10);
				}
//				client.Close ();
				Debug.Log ("[Server] Disconnected");
				break;
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
	}

	void OnApplicationQuit () {
		stopListening();
		server.Stop ();
	}
}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class WebCamServer : MonoBehaviour {
	public const int WEBCAM_WIDTH = 1280;
	public const int WEBCAM_HEIGHT = 720;
	public const int ratio = 4;

//	public Image webCamImage;
	
	WebCamTexture webcam = null;
	static Texture2D texture;
	static byte[] imgBytes;
	
	TcpListener server = null;
	Thread thread;
	bool mRunning;

	byte imgCommand = 0;

	void Awake () {
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
		mRunning = true;
		try {
			server = new TcpListener (IPAddress.Any, 3003);
			server.Start ();
		}
		catch (Exception ex) {
			Debug.Log (ex.ToString ());
		}
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
				byte[] inByte = new byte[1024];
				while (client.Connected) {
					try {
						int nRead = stream.Read (inByte, 0, 1024);
						Debug.Log ("[server] get: " + inByte[0].ToString () + ", " + nRead.ToString () + " bytes");
					} catch (Exception ex) {
						Debug.Log (ex.ToString ());
						continue;
					}
					if (inByte[0] == 100) {
						lock (imgBytes)
						{
							byte[] intBytes = BitConverter.GetBytes (imgBytes.Length);
							stream.Write (intBytes, 0, intBytes.Length);
							stream.Write (imgBytes, 0, imgBytes.Length);
//							Debug.Log ("[server] send count: " + imgBytes.Length.ToString ());
							stream.Flush ();
						}
						Thread.Sleep (10);
					}
					else if (inByte[0] > 0) {
						imgCommand = inByte[0];
						Debug.Log ("Get " + imgCommand.ToString ());
					}
				}
				client.Close ();
				Debug.Log ("[server] client disconnected");
			}
			Thread.Sleep (10);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!webcam.isPlaying)
			return;

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
			texture.Apply ();

			UITexture sprite = GetComponentInChildren<UITexture> ();
			Material tempMat = null;
			if (sprite.material == null)
			{
				tempMat = new Material(Shader.Find("Unlit/Transparent Colored"));
			}
			else
			{
				tempMat = new Material(sprite.material);
			}
			sprite.material = tempMat;
			tempMat.mainTexture = texture;
			sprite.MakePixelPerfect();

			imgBytes = texture.EncodeToJPG ();
//			imgBytes = texture.EncodeToPNG ();
//			File.WriteAllBytes ("src.jpg", imgBytes);
		}

		if (imgCommand > 0)
		{
			ImgManager.instance.ShowImage(imgCommand);
			Debug.Log ("Update Get " + imgCommand.ToString ());
			switch(imgCommand)
			{
			case 14:
				SaveWebcam("before.png");
				break;
			case 102:
			case 111:
				SaveWebcam("after.png");
				break;
			}
			imgCommand = 0;
		}
	}

	void SaveWebcam(string name)
	{
		const int ratio = 2;
		Texture2D texture = new Texture2D(webcam.width/ratio, webcam.height/ratio);
		for (int y = 0 ; y < webcam.height ; y+=ratio)
		{
			for (int x = 0 ; x < webcam.width ; x+=ratio)
			{
				Color color = webcam.GetPixel(x, y);
				texture.SetPixel(x/ratio, y/ratio, color); 
			}
		}
//		snap.SetPixels(webcam.GetPixels());
		texture.Apply();
		System.IO.File.WriteAllBytes(Application.dataPath + "/" + name, texture.EncodeToPNG());
		Debug.Log ("Save webcam to " + name);
	}

	public void stopListening() {
		mRunning = false;
		server.Stop ();
	}
	
	void OnApplicationQuit () {
		stopListening();
		thread.Abort ();
	}
}

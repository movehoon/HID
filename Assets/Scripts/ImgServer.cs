using UnityEngine;
using System.Collections;
using Verso.Core;
using Verso.Server;

public class ImgServer : MonoBehaviour {

	public const int WEBCAM_WIDTH = 1280;
	public const int WEBCAM_HEIGHT = 720;
	public const int ratio = 4;

	public GameObject receiverObject;
	
	WebCamTexture webcam = null;
	static Texture2D texture;
	static byte[] imgBytes;

	static Server server = null;
	static Connection myConnection;

	// Use this for initialization
	void Start () {
		webcam = new WebCamTexture (WEBCAM_WIDTH, WEBCAM_HEIGHT, 10);
		webcam.Play ();
		if (webcam != null)
			texture = new Texture2D (WEBCAM_WIDTH/ratio, WEBCAM_HEIGHT/ratio);

		ServerConfig config = new ServerConfig ();
		config.port = 3003;
		server = new Server (config);
		server.OnClientConnected += server_OnClientConnected;
		server.OnConnectionClosed += connection_OnConnectionClosed;
		server.Start ();
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

	static void server_OnClientConnected(Connection connection)
	{
		Debug.Log("Client " + connection.remoteEndPoint + " connected.");
		myConnection = connection;
		Debug.Log ("Send img buffer size " + imgBytes.GetLength (0).ToString ());
		connection.SendRawData (imgBytes, 0, imgBytes.GetLength (0));
	}

	static void connection_OnConnectionClosed(Connection connection)
	{
		Debug.Log("Client " + connection.remoteEndPoint + " disconnected.");
	}

}

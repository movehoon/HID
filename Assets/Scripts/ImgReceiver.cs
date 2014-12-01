using UnityEngine;
using System.Collections;
using System.IO;
using Verso.Core;
using Verso.Client;

public class ImgReceiver : MonoBehaviour {

	Texture2D texture;

	private Client client;
	private Connection connection;



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
		Debug.Log ("ConnectToServer");

		ClientConfig cc = new ClientConfig();
		cc.realTime = true;
		client = new Client(cc);
		client.OnConnected += client_OnConnected;
		client.OnConnectionClosed += client_OnConnectionClosed;
		client.OnRawDataReceived += client_OnRawDataReceived;
		client.OnError += client_OnError;
		client.Connect ("127.0.0.1", 3003);
	}

	
	void client_OnError(string error)
	{
		Debug.Log("Connection error: " + error);
	}

	void client_OnRawDataReceived(byte[] data)
	{
		Debug.Log("client_OnRawDataReceived: " + data.GetLength (0).ToString ());
	}
	
//	void client_OnMessageReceived(IncomingMessage im)
//	{
//		Debug.Log("client_OnMessageReceived: " + im.length.ToString ());
//	}
	
	void client_OnConnectionClosed()
	{
		Debug.Log("Disconnected from server.");
		connection = null;
	}
	
	void client_OnConnected()
	{
		connection = client.connection;
		Debug.Log("Connected to server.");
		OutgoingMessage om = new OutgoingMessage();
//		om.WriteByte(C_ENTER_GAME);
		connection.SendMessage(om);
	}

}

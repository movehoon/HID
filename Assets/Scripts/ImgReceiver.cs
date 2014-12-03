using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class ImgReceiver : MonoBehaviour {

	Texture2D texture;

	TcpClient client = new TcpClient ();
//	Socket client = null;

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
//		IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3003);
//		client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//		try
//		{
//			client.Connect(ipep);
//		}
//		catch (SocketException e)
//		{
//			Debug.Log(e.ToString());
//		}

		client.Connect (IPAddress.Parse ("127.0.0.1"), 3003);
		NetworkStream nNetStream = client.GetStream ();
		while (client.Connected)
		{
			Debug.Log ("[client]Connected...");
			byte[] bytes = new byte[client.ReceiveBufferSize];
			if (nNetStream.CanRead)
			{
				nNetStream.Read(bytes, 0, bytes.Length);  
			}
			else
			{
				client.Close();
				nNetStream.Close();
			}
		}
		client.Close();
	}
}

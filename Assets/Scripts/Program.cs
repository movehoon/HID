using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using LitJson;

public class Program : MonoBehaviour {

	static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
	static private string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Connect () {
		Debug.Log ("Connecting");
		string jsonString = @"{""Dialog"":""hello"", ""TTS"":""Im nao"", ""Behavior"":""Behavior1""}";
		Hashtable hashTable = JsonParser._Instance.StringToHashTable (jsonString);
		Debug.Log (JsonParser._Instance.HashtableToJsonString(hashTable));

		string data = hashTable == null ? "{}" : JsonParser._Instance.HashtableToJsonString (hashTable);

		Hashtable header = new Hashtable();
		header.Add ("Content-Type", "application/json; charset=utf-8");
		header.Add ("Content-Length", data.Length );

		WWW www = new WWW ("192.168.1.12:2223", Encoding.UTF8.GetBytes(data), header);
		if (www.error != null)
			Debug.Log ("Error");
		else
			Debug.Log ("Success");
	}

}

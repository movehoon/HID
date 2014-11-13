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

	public UIInput ipaddress;
	public UIInput speakText;
	int port = 2223;

	static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
	static private string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
	
	public void Connect () {
		Debug.Log ("Connecting");
//		string jsonString = @"{""dialog"":""hello"", ""tts"":""Im nao"", ""behavior"":""Behavior1""}";
//		Hashtable hashTable = JsonParser._Instance.StringToHashTable (jsonString);
//		Debug.Log (JsonParser._Instance.HashtableToJsonString(hashTable));
//
//		string data = hashTable == null ? "{}" : JsonParser._Instance.HashtableToJsonString (hashTable);
//
//		Hashtable header = new Hashtable();
//		header.Add ("Content-Type", "application/json; charset=utf-8");
//		header.Add ("Content-Length", data.Length );
//
//		WWW www = new WWW (ipaddress.value+":"+port.ToString(), Encoding.UTF8.GetBytes(data), header);
//		if (www.error != null)
//			Debug.Log ("Error");
//		else
//			Debug.Log ("Success");
	}

	public void Speak () {
		string jsonString = @"{""dialog"":""";
		jsonString += speakText.value;
		jsonString += @"""}";
		Debug.Log (jsonString);
		Send (jsonString);
	}

	public void Behavior1 () {
		Behavior ("behavior1");
	}
	
	public void Behavior2 () {
		Behavior ("behavior2");
	}
	
	public void Behavior3 () {
		Behavior ("behavior3");
	}
	
	public void Behavior4 () {
		Behavior ("behavior4");
	}
	
	public void Behavior5 () {
		Behavior ("behavior5");
	}
	
	void Behavior (string str) {
		string jsonString = @"{""behavior"":""";
		jsonString += str;
		jsonString += @"""}";
		Send (jsonString);
	}

	void Send (string jsonString) {
		Hashtable hashTable = JsonParser._Instance.StringToHashTable (jsonString);
		Debug.Log (JsonParser._Instance.HashtableToJsonString(hashTable));
		
		string data = hashTable == null ? "{}" : JsonParser._Instance.HashtableToJsonString (hashTable);
		
		Hashtable header = new Hashtable();
		header.Add ("Content-Type", "application/json; charset=utf-8");
		header.Add ("Content-Length", data.Length );
		
		WWW www = new WWW (ipaddress.value+":"+port.ToString(), Encoding.UTF8.GetBytes(data), header);
		if (www.error != null)
			Debug.Log ("Error");
		else
			Debug.Log ("Success");
	}
}

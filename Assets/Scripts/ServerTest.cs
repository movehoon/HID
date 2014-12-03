using UnityEngine;
using System.Collections;

public class ServerTest : MonoBehaviour {

	TcpServer tcpServer = null;

	// Use this for initialization
	void Start () {
		tcpServer = new TcpServer (3003);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

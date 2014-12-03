using UnityEngine;
using System.Collections;

public class ClientTest : MonoBehaviour {

	ClientDemo clientDemo = null;

	// Use this for initialization
	void Start () {
		clientDemo = new ClientDemo ("127.0.0.1", 3003);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

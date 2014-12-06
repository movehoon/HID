using UnityEngine;
using System.Collections;
using System.Net;

public class WebCamServerApp : MonoBehaviour {

	public UILabel labelIpAddr;

	// Use this for initialization
	void Start () {
		// Then using host name, get the IP address list..
		IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
		IPAddress[] addr = ipEntry.AddressList;
		labelIpAddr.text = "IP Address: " + addr[0].ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reset () {
		Application.LoadLevel (0);
	}
}

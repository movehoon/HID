using UnityEngine;
using System.Collections;
using System.Net;

public class IpAddressLabel : MonoBehaviour {

	void Start () {
		UILabel label = GetComponentInChildren<UILabel> ();
		label.text = "";
		IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
		IPAddress[] addr = ipEntry.AddressList;
		if (addr.Length > 0)
		{
			foreach (IPAddress ip in addr) 
			{
				if (ip.ToString ().StartsWith("192"))
					label.text = "IP Address: " + addr[addr.Length-1].ToString ();
			}
		}
	}
}

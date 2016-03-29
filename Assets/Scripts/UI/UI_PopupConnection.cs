using UnityEngine;
using System.Collections;

public class UI_PopupConnection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			gameObject.SetActive (false);
	}
}

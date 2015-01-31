using UnityEngine;
using System;
using System.Collections;

public class StateOnClick : MonoBehaviour {

	void OnClick () 
	{
		UILabel label = gameObject.transform.Find("Label ID").GetComponentInChildren<UILabel> ();
		byte stateID = Convert.ToByte(label.text);
		GameObject.Find ("@Program").GetComponentInChildren<Program> ().ChangeRobotStateN (stateID);
#if UNITY_EDITOR
		Debug.Log ("OnClick: " + label.text);
#endif
	}
}

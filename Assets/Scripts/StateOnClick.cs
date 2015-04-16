using UnityEngine;
using System;
using System.Collections;

public class StateOnClick : MonoBehaviour {

	void OnClick () 
	{
		UILabel label = gameObject.transform.Find("Label ID").GetComponentInChildren<UILabel> ();
		string[] words = label.text.Split('-');
		byte state = Convert.ToByte (words [0]);
		byte substate = Convert.ToByte (words [1]);
		GameObject.Find ("@Program").GetComponentInChildren<Program> ().ChangeRobotStateN (state, substate);
#if UNITY_EDITOR
		Debug.Log ("OnClick: " + label.text);
#endif
	}
}

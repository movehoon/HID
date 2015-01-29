using UnityEngine;
using System;
using System.Collections;

public class StateOnClick : MonoBehaviour {

	void OnClick () 
	{
		UILabel label = gameObject.transform.Find("Label ID").GetComponentInChildren<UILabel> ();
		int stateID = Convert.ToInt32(label.text);
		GameObject.Find ("@Program").GetComponentInChildren<Program> ().ChangeRobotState (stateID);
#if UNITY_EDITOR
		Debug.Log ("OnClick: " + label.text);
#endif
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Trigger : MonoBehaviour {

	public void Fire () {
		string trigger = GetComponentInChildren<Text> ().text;
		GameObject.Find ("Program").GetComponentInChildren<Program> ().SendSpeechRecognized (trigger);
	}
}

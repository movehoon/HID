using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Trigger : MonoBehaviour {

	public void Fire () {
		string trigger = GetComponentInChildren<Text> ().text;
		GameObject program = GameObject.Find ("Program");
		if (trigger.Contains ("*") | trigger.Contains("|")) {
			program.GetComponentInChildren<Program> ().UI_PopupRecogInput.SetActive (true);
			program.GetComponentInChildren<Program> ().UI_PopupRecogInput.GetComponentInChildren<UI_PopupRecogInput> ().InitUI (trigger);
		} else {
			program.GetComponentInChildren<Program> ().SendSpeechRecognized (trigger);
		}
	}
}

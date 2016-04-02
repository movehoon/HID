using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Trigger : MonoBehaviour {

	public void Fire () {
		string trigger = GetComponentInChildren<Text> ().text;
		GameObject program = GameObject.Find ("Program");
		if (trigger.Contains("|")) {
			program.GetComponentInChildren<Program> ().UI_PopupRecogInput.SetActive (true);
			program.GetComponentInChildren<Program> ().UI_PopupRecogInput.GetComponentInChildren<UI_PopupRecogInput> ().InitUI (trigger);
		}
		else if (trigger.Contains("*") | trigger.Contains("#") | trigger.Contains("_")) {
			program.GetComponentInChildren<Program> ().UI_PopupWildcardInput.SetActive (true);
			program.GetComponentInChildren<Program> ().UI_PopupWildcardInput.GetComponentInChildren<UI_PopupWildcardInput> ().InitUI (trigger);
		}
		else {
			program.GetComponentInChildren<Program> ().SendSpeechRecognized (trigger);
		}
	}
}

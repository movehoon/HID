using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UI_PopupWildcardInput : MonoBehaviour {

	public Text Trigger;
	public InputField[] Wildcards;
	public GameObject[] HidePanel;

	public void InitUI (string triggers_input) {
		foreach (GameObject panel in HidePanel)
			panel.SetActive (false);

		Trigger.text = triggers_input;
		foreach (InputField wildcard in Wildcards)
			wildcard.text = "";
		EventSystem.current.SetSelectedGameObject(Wildcards[0].gameObject, null);
		Wildcards[0].OnPointerClick (new PointerEventData(EventSystem.current));
	}

	public void Send () {
		GameObject program = GameObject.Find ("Program");
		string speak = Trigger.text.Replace("*", Wildcards[0].text);
		program.GetComponentInChildren<Program> ().SendSpeechRecognized (speak);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			gameObject.SetActive (false);
	}
}

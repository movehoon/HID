using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI_PopupRecogInput : MonoBehaviour {

	public InputField wildcard;
	public RectTransform ScrollView;
	public GameObject UI_Button;

	public void InitUI (string triggers_input) {
		wildcard.text = "";
		EventSystem.current.SetSelectedGameObject(wildcard.gameObject, null);
		wildcard.OnPointerClick (new PointerEventData(EventSystem.current));

		triggers_input = triggers_input.Replace ("(", string.Empty);
		triggers_input = triggers_input.Replace (")", string.Empty);
		string[] trigs = triggers_input.Split ('|');
		List<string> triggers = new List<string> ();
		foreach (string trig in trigs)
			triggers.Add (trig);

		RectTransform Content = ScrollView.GetComponentInChildren<ScrollRect>().content.GetComponentInChildren<RectTransform>();
		Content.sizeDelta = new Vector2 (Content.sizeDelta.x, triggers.Count * 40f);

		float Y_STEP = -40f;
		for (int i = 0; i < Content.childCount; i++) {
			Destroy (Content.GetChild (i).gameObject);
		}
		Content.DetachChildren ();
		for (int i = 0; i < triggers.Count; i++) {
			RectTransform uiTrigger = Instantiate (UI_Button).GetComponentInChildren<RectTransform> ();
			uiTrigger.SetParent (Content);
			uiTrigger.GetComponentInChildren<Text> ().text = triggers [i];
			uiTrigger.GetComponentInChildren<RectTransform> ().offsetMin = new Vector2(10f, Y_STEP * i - 30);
			uiTrigger.GetComponentInChildren<RectTransform> ().offsetMax = new Vector2(-10f, Y_STEP * i);
		}
	}

	public void Send () {
//		string recognized_word = Trigger.text;
//		recognized_word = recognized_word.Replace ("*", wildcard.text);
//		GameObject.Find ("Program").GetComponentInChildren<Program> ().SendSpeechRecognized (recognized_word);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			gameObject.SetActive (false);
	}
}

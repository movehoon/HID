using UnityEngine;
using System.Collections;

public class UiAnswer : MonoBehaviour {
	public UIInput input;

	public void SetText (string text)
	{
		input.value = text;
	}

	public void ClickSend ()
	{
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendSpeechRecognized (input.value);
	}
}

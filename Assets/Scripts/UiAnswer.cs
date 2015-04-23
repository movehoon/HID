using UnityEngine;
using System.Collections;

public class UiAnswer : MonoBehaviour {
	public UIInput input;
	public UIButton button;

	public void SetText (string text)
	{
		input.value = text;
	}

	public string GetText () 
	{
		return input.value;
	}

	public void ClickSend ()
	{
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendSpeechRecognized (input.value);
	}

	public void EnableButton (bool enable)
	{
		button.enabled = enable;
	}
}

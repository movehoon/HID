using UnityEngine;
using System.Collections;

public class UiSpeechRecognizedManager : MonoBehaviour {
	public UILabel[] labelSpeech;
	public UILabel[] labelConfidence;
	public UIButton[] buttons;

	public void SetSpeechWithConfidence(int n, string speech, float confidence)
	{
		if (n < labelSpeech.Length) {
			labelSpeech [n].text = speech;
			labelConfidence [n].text = ((int)(confidence*100)).ToString () + "%";
		}
	}

	void Start () {
		Invoke ("KillMe", 10);
	}

	void KillMe () {
		Destroy (gameObject);
	}
	
	public void ClickSpeech1 () {
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendSpeechRecognized (labelSpeech[0].text);
		Destroy (gameObject);
	}
	public void ClickSpeech2 () {
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendSpeechRecognized (labelSpeech[1].text);
		Destroy (gameObject);
	}
	public void ClickSpeech3 () {
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendSpeechRecognized (labelSpeech[2].text);
		Destroy (gameObject);
	}

	public void EnableButtons (bool enable) {
		foreach (UIButton button in buttons) {
			button.enabled = enable;
		}
	}
}

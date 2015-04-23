using UnityEngine;
using System.Collections;

public class SpeechRecPresetManager : MonoBehaviour {

	public UILabel [] words;
	public UILabel [] confidences;

	string [] presetWords1 = {"전진", "언제",  "전쟁"};
	string [] presetConfidences1 = {"0.8", "0.75",  "0.6"};
	string [] presetWords2 = {"전진", "언제",  "전쟁"};
	string [] presetConfidences2 = {"0.45", "0.37",  "0.42"};

	public void SetPreset1 () 
	{
		for (int i = 0 ; i < words.Length ; i++) {
			words[i].text = presetWords1[i];
			confidences[i].text = presetConfidences1[i];
		}
	}

	public void SetPreset2 () {
		for (int i = 0 ; i < words.Length ; i++) {
			words[i].text = presetWords2[i];
			confidences[i].text = presetConfidences2[i];
		}
	}
}

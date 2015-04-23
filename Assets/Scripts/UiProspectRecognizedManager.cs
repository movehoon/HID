using UnityEngine;
using System.Collections;

public class UiProspectRecognizedManager : MonoBehaviour {
	public UILabel labelPositives;
	public UILabel labelNegatives;
	public UIButton buttonPositive;
	public UIButton buttonNegative;

	Color colorSelected = new Color(25/255f, 233/255f, 59/255f);
	Color colorDeselected = new Color(0/255f, 0/255f, 0/255f);

	bool _state = false;

	void Start ()
	{
		SetState (_state);
	}

	public void SetState(bool state)
	{
		_state = state;
		labelPositives.color = state ? colorSelected : colorDeselected;
		labelNegatives.color = state ? colorDeselected : colorSelected;
	}

	public bool IsPositive ()
	{
		return _state;
	}
	
	public void ClickPositive () {
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendProspectRecognizedPositives ();
	}
	
	public void ClickNegative () {
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendProspectRecognizedNegatives ();
	}

	public void SetStateTrue () {
		SetState (true);
	}

	public void SetStateFalse () {
		SetState (false);
	}

	public void EnableButton(bool enable) {
		buttonPositive.enabled = enable;
		buttonNegative.enabled = enable;
	}
}

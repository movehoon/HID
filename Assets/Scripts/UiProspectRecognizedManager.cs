using UnityEngine;
using System.Collections;

public class UiProspectRecognizedManager : MonoBehaviour {
	public UILabel labelPositives;
	public UILabel labelNegatives;
	
	Color colorSelected = new Color(25/255f, 233/255f, 59/255f);
	Color colorDeselected = new Color(0/255f, 0/255f, 0/255f);
	
	public void SetState(bool state)
	{
		labelPositives.color = state ? colorSelected : colorDeselected;
		labelNegatives.color = state ? colorDeselected : colorSelected;
	}
	
	public void ClickPositive () {
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendProspectRecognizedPositives ();
	}
	
	public void ClickNegative () {
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendProspectRecognizedNegatives ();
	}
}

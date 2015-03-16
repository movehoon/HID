using UnityEngine;
using System.Collections;

public class UiBoolManager : MonoBehaviour {
	public UILabel labelTrue;
	public UILabel labelFalse;

	Color colorSelected = new Color(25/255f, 233/255f, 59/255f);
	Color colorDeselected = new Color(0/255f, 0/255f, 0/255f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetState(bool state)
	{
		labelTrue.color = state ? colorSelected : colorDeselected;
		labelFalse.color = state ? colorDeselected : colorSelected;
	}

	public void ClickTrue () {
//		SetState (true);
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendFaceDetectTrue ();
	}

	public void ClickFalse () {
//		SetState (false);
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendFaceDetectFalse ();
	}
}

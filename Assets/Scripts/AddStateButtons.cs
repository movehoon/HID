using UnityEngine;
using System.Collections;

public class AddStateButtons : MonoBehaviour {

	public StateManager states;
	public GameObject stateButtonPrefab;

	Color colorOrange = new Color (245f/255f, 166f/255f,  45f/255f);
	Color colorGreen  = new Color (126f/255f, 211f/255f,  33f/255f);
	Color colorBlue   = new Color ( 74f/255f, 114f/255f, 226f/255f);

	void Awake () {
		int count = states.states.Count;
		foreach (SimpleState state in states.states) 
		{
			foreach (SimpleSubstate substate in state.substates)
			{
				GameObject go = Instantiate(stateButtonPrefab) as GameObject;
				UILabel labelID = go.transform.Find("Label ID").GetComponentInChildren<UILabel> ();
				labelID.text = state.id.ToString () + "-" + substate.id.ToString ();
				labelID.maxLineCount = state.id * 10 + substate.id;
				UILabel labelDetail = go.transform.Find("Label Detail").GetComponentInChildren<UILabel> ();
				labelDetail.text = substate.detail;
				go.transform.parent = this.transform;
//				UIButton button = go.GetComponentInChildren<UIButton> ();
//				if (state.id % 2 == 0)
//					button.defaultColor = colorGreen;
//				else
//					button.defaultColor = colorBlue;
				UISprite sprite = go.GetComponentInChildren<UISprite> ();
				sprite.MakePixelPerfect ();
			}
		}
	}
}

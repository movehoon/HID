using UnityEngine;
using System.Collections;

public class AddStateButtons : MonoBehaviour {

	public StateManager states;
	public GameObject stateButtonPrefab;

	void Awake () {
		int count = states.states.Count;
		foreach (SimpleState state in states.states) 
		{
			GameObject go = Instantiate(stateButtonPrefab) as GameObject;
			UILabel labelID = go.transform.Find("Label ID").GetComponentInChildren<UILabel> ();
			labelID.text = state.id.ToString ();
			labelID.maxLineCount = state.id;
			UILabel labelDetail = go.transform.Find("Label Detail").GetComponentInChildren<UILabel> ();
			labelDetail.text = state.detail;
			go.transform.parent = this.transform;
			UISprite sprite = go.GetComponentInChildren<UISprite> ();
			sprite.MakePixelPerfect ();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

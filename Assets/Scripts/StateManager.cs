using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SimpleState {
	public int id;
	public string detail;
	public List<SimpleSubstate> substates = new List<SimpleSubstate> ();
}

[System.Serializable]
public class SimpleSubstate {
	public int id;
	public string detail;
}

public class StateManager : MonoBehaviour {
	public List<SimpleState> states = new List<SimpleState> ();
}

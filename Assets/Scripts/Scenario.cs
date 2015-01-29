using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Data {
	public State states;
}

[System.Serializable]
public class State {
	public int id;
	public string detail;
}

class Action {
}

class Condition {
}

public class Scenario : MonoBehaviour {

	public List<State> data = new List<State> ();
}

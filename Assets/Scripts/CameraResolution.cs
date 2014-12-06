using UnityEngine;
using System.Collections;

public class CameraResolution : MonoBehaviour {

	public Camera camera;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float perx = 1080.0f/Screen.width;
		float pery = 1920.0f/Screen.height;
		float v = (perx<pery) ? perx : pery;
		camera.GetComponent<Camera> ().orthographicSize = v;
	}
}

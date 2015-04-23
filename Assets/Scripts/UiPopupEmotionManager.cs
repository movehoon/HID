using UnityEngine;
using System.Collections;

public class UiPopupEmotionManager : MonoBehaviour {
	public Transform thumb;

	int width = 800;
	int height = 800;
	float touchXMax = 0.6f;
	float touchYMax = 0.6f;

	void Start () {
//		SetThumbPosition (0.5f, -0.5f);
	}

	public void TouchUp () {
		Vector3 touchPosition = UICamera.currentCamera.ScreenPointToRay (UICamera.lastTouchPosition).origin;
		float pleasure = touchPosition.x / touchXMax;
		float arrousal = touchPosition.y / touchYMax;
		SetThumbPosition (pleasure, arrousal);
//		Debug.Log (UICamera.currentCamera.ScreenPointToRay (UICamera.lastTouchPosition).origin.ToString ());
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendEmotionPosition (pleasure, arrousal);
	}

	public void SetThumbPosition (float x, float y) {
		float dstX = thumb.localPosition.x;
		float dstY = thumb.localPosition.y;
		if (-1.0f <= x && x <= 1.0f)
			dstX = x * width / 2;
		if (-1.0f <= y && y <= 1.0f)
			dstY = y * height / 2;
		thumb.localPosition = new Vector3 (dstX, dstY, 0);
		Debug.Log ("Move Thumb: " + thumb.localPosition.ToString ());
	}
}

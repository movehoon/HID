using UnityEngine;
using System.Collections;

public class UiPopupEmotionManager : MonoBehaviour {
	public Transform thumb;

	public int width = 800;
	public int height = 800;
	public float touchXMin = 0.6f;
	public float touchXMax = 1.6f;
	public float touchYMin = 0.5f;
	public float touchYMax = -0.5f;

	void Start () {
//		SetThumbPosition (0.5f, -0.5f);
	}

	public void TouchUp () {
		Vector3 touchPosition = UICamera.currentCamera.ScreenPointToRay (UICamera.lastTouchPosition).origin;
		Debug.Log (UICamera.currentCamera.ScreenPointToRay (UICamera.lastTouchPosition).origin.ToString ());
		float pleasure =  Mathf.Lerp (-1f, 1f, Mathf.InverseLerp (touchXMin, touchXMax, touchPosition.x));
		float arrousal =  Mathf.Lerp (-1f, 1f, Mathf.InverseLerp (touchYMin, touchYMax, touchPosition.y));
		Debug.Log ("pleasure: " + pleasure.ToString () + ", arrousal: " + arrousal.ToString ());
		SetThumbPosition (pleasure, arrousal);
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

	public void SendCurrentEmotionToServer () {
		float pleasure = thumb.localPosition.x / (width /2);
		float arrousal = thumb.localPosition.y / (height /2);
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendEmotionPosition (pleasure, arrousal);
	}

	public void SendCurrentEmotionToHID () {
		float pleasure = thumb.localPosition.x / (width /2);
		float arrousal = thumb.localPosition.y / (height /2);
		GameObject.Find ("@Program").GetComponentInChildren <ProgramForRos> ().SendEmotionPosition (pleasure, arrousal);
	}
}

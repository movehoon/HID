using UnityEngine;
using System.Collections;

public class UiPopupEmotionManager : MonoBehaviour {
	public Transform thumb;

	int width = 800;
	int height = 800;

	void Start () {
//		SetThumbPosition (0.5f, -0.5f);
	}

	public void SetThumbPosition (float x, float y) {
		int dstX = 0;
		int dstY = 0;
		if (-1.0f <= x && x <= 1.0f)
			dstX = (int)(x * width / 2);
		if (-1.0f <= y && y <= 1.0f)
			dstY = -(int)(x * height / 2);
		thumb.localPosition = new Vector3 (dstX, dstY, 0);
	}
}

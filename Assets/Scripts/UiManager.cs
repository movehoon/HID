using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UiManager : MonoBehaviour {

	enum UI_TYPE{
		UI_NOT_DEFINED,
		UI_FACE_DETECTED,
		UI_MOTION_DETECTED,
		UI_PERSON_DETECTED,
		UI_GESTURE_RECOGNIZED,
		UI_PERSON_RECOGNIZED,
		UI_FACE_RECOGNIZED,
		UI_FACIAL_EXPRESSION_RECOGNIZED,
		UI_SPEECH_RECOGNIZED,
		UI_PROSPECT_RECOGNIZED,
	};

	public Transform uiFaceDetected;
	public Transform uiMotionDetected;
	public Transform uiPersonDetected;
	public Transform uiGestureRecognized;
	public Transform uiPersonRecognized;
	public Transform uiFaceRecognized;
	public Transform uiFacialExpressionRecognized;
	public Transform uiSpeechRecognized;
	public Transform uiProspectRecognized;

	List<Transform> uiList = new List<Transform> ();

	// Update is called once per frame
	void Update () {
		float uiHeight = 0;
		foreach (Transform transform in uiList) {
			if (transform.localPosition.y != uiHeight)
			{
				transform.localPosition = new Vector3(0, uiHeight, 0);
			}
			Debug.Log ("Set ui height: " + uiHeight.ToString ());
			uiHeight -= GetUIHeight(GetUIType(transform.name));
		}
	}

	public void UpdateStart () {

	}

	public void UpdateEnd () {
	}

	public void SetFaceDetected(bool detected)
	{
		if (!HasUI ("FaceDetected")) 
		{
			Transform transform = Instantiate (uiFaceDetected, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
			uiList.Add(transform);
		}
		UiFaceDetectedManager ui = GetUI(UI_TYPE.UI_FACE_DETECTED).gameObject.GetComponentInChildren<UiFaceDetectedManager> ();
		ui.SetState (detected);
	}

	public void SetMotionDetected(bool detected)
	{
		if (!HasUI ("MotionDetected")) 
		{
			Transform transform = Instantiate (uiMotionDetected, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
			uiList.Add(transform);
		}
		UiMotionDetectedManager ui = GetUI(UI_TYPE.UI_MOTION_DETECTED).gameObject.GetComponentInChildren<UiMotionDetectedManager> ();
		ui.SetState (detected);
	}

	bool HasUI(string uiName)
	{
		foreach (Transform transform in uiList) {
			if (transform.name.Contains (uiName))
				return true;
		}
		return false;
	}

	bool HasUI(UI_TYPE type)
	{
		foreach (Transform transform in uiList) 
		{
			switch (type)
			{
			case UI_TYPE.UI_FACE_DETECTED:
				if (transform.name.Contains ("FaceDetected"))
				    return true;
			    break;
			case UI_TYPE.UI_MOTION_DETECTED:
				if (transform.name.Contains ("MotionDetected"))
					return true;
				break;
			case UI_TYPE.UI_PERSON_DETECTED:
				if (transform.name.Contains ("PersonDetected"))
					return true;
				break;
			case UI_TYPE.UI_GESTURE_RECOGNIZED:
				if (transform.name.Contains ("GestureRecognized"))
					return true;
				break;
			case UI_TYPE.UI_PERSON_RECOGNIZED:
				if (transform.name.Contains ("PersonRecognized"))
					return true;
				break;
			case UI_TYPE.UI_FACE_RECOGNIZED:
				if (transform.name.Contains ("FaceRecognized"))
					return true;
				break;
			case UI_TYPE.UI_FACIAL_EXPRESSION_RECOGNIZED:
				if (transform.name.Contains ("FacialExpressionRecognized"))
					return true;
				break;
			case UI_TYPE.UI_SPEECH_RECOGNIZED:
				if (transform.name.Contains ("SpeechRecognized"))
					return true;
				break;
			case UI_TYPE.UI_PROSPECT_RECOGNIZED:
				if (transform.name.Contains ("ProspectRecognized"))
					return true;
				break;
			}
		}
		return false;
	}

	int GetUIHeight (UI_TYPE type)
	{
		switch (type)
		{
		case UI_TYPE.UI_FACE_DETECTED:
		case UI_TYPE.UI_MOTION_DETECTED:
		case UI_TYPE.UI_PERSON_DETECTED:
		case UI_TYPE.UI_PROSPECT_RECOGNIZED:
			return 100;
		case UI_TYPE.UI_GESTURE_RECOGNIZED:
		case UI_TYPE.UI_PERSON_RECOGNIZED:
		case UI_TYPE.UI_FACE_RECOGNIZED:
		case UI_TYPE.UI_FACIAL_EXPRESSION_RECOGNIZED:
		case UI_TYPE.UI_SPEECH_RECOGNIZED:
			return 300;
		}
		return 0;
	}

	UI_TYPE GetUIType (string uiName)
	{
		if (uiName.Contains ("FaceDetected"))
			return UI_TYPE.UI_FACE_DETECTED;
		if (uiName.Contains ("MotionDetected"))
			return UI_TYPE.UI_MOTION_DETECTED;
		if (uiName.Contains ("PersonDetected"))
			return UI_TYPE.UI_PERSON_DETECTED;
		if (uiName.Contains ("GestureRecognized"))
			return UI_TYPE.UI_GESTURE_RECOGNIZED;
		if (uiName.Contains ("PersonRecognized"))
			return UI_TYPE.UI_PERSON_RECOGNIZED;
		if (uiName.Contains ("FaceRecognized"))
			return UI_TYPE.UI_FACE_RECOGNIZED;
		if (uiName.Contains ("FacialExpressionRecognized"))
			return UI_TYPE.UI_FACIAL_EXPRESSION_RECOGNIZED;
		if (uiName.Contains ("SpeechRecognized"))
			return UI_TYPE.UI_SPEECH_RECOGNIZED;
		if (uiName.Contains ("ProspectRecognized"))
			return UI_TYPE.UI_PROSPECT_RECOGNIZED;
		return UI_TYPE.UI_NOT_DEFINED;
	}

	Transform GetUI(UI_TYPE type)
	{
		foreach (Transform transform in uiList) 
		{
			switch (type)
			{
			case UI_TYPE.UI_FACE_DETECTED:
				if (transform.name.Contains ("FaceDetected"))
					return transform;
				break;
			case UI_TYPE.UI_MOTION_DETECTED:
				if (transform.name.Contains ("MotionDetected"))
					return transform;
				break;
			case UI_TYPE.UI_PERSON_DETECTED:
				if (transform.name.Contains ("PersonDetected"))
					return transform;
				break;
			case UI_TYPE.UI_GESTURE_RECOGNIZED:
				if (transform.name.Contains ("GestureRecognized"))
					return transform;
				break;
			case UI_TYPE.UI_PERSON_RECOGNIZED:
				if (transform.name.Contains ("PersonRecognized"))
					return transform;
				break;
			case UI_TYPE.UI_FACE_RECOGNIZED:
				if (transform.name.Contains ("FaceRecognized"))
					return transform;
				break;
			case UI_TYPE.UI_FACIAL_EXPRESSION_RECOGNIZED:
				if (transform.name.Contains ("FacialExpressionRecognized"))
					return transform;
				break;
			case UI_TYPE.UI_SPEECH_RECOGNIZED:
				if (transform.name.Contains ("SpeechRecognized"))
					return transform;
				break;
			case UI_TYPE.UI_PROSPECT_RECOGNIZED:
				if (transform.name.Contains ("ProspectRecognized"))
					return transform;
				break;
			}		}
		return null;
	}

}

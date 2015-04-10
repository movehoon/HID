using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UiManager : MonoBehaviour {

	enum UI_TYPE{
		UI_FACE_REC,
		UI_VOICE_REC,
	};

	public Transform uiFaceRec;
	public Transform uiVoiceRec;

	List<Transform> uiList = new List<Transform> ();
	float ui_height;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	bool HasUI(UI_TYPE type)
	{
		foreach (Transform transform in uiList) 
		{
			if (type == UI_TYPE.UI_FACE_REC && transform.name.Contains ("FaceRec"))
			{
				return true;
			}
			else if (type ==UI_TYPE.UI_VOICE_REC && transform.name.Contains ("VoiceRec"))
			{
				return true;
			}
		}
		return false;
	}

	Transform GetUI(UI_TYPE type)
	{
		foreach (Transform transform in uiList) 
		{
			if (type == UI_TYPE.UI_FACE_REC && transform.name.Contains ("FaceRec"))
			{
				return transform;
			}
			else if (type ==UI_TYPE.UI_VOICE_REC && transform.name.Contains ("VoiceRec"))
			{
				return transform;
			}
		}
		return null;
	}
}

	public void SetFaceRec(bool recognized)
	{
		if (!HasUI (UI_TYPE.UI_FACE_REC)) 
		{
			// Instanciate face rec ui
			Transform transform = Instantiate (uiFaceRec, new Vector3(0, ui_height, 0), Quaternion.identity) as Transform;
			uiList.Add(transform);
//			ui_height += uiFaceRec.localScale.y;
		}
		UiFaceRecManager ui = GetUI(UI_TYPE.UI_FACE_REC).GetComponentInChild<UiFaceRecManager> ();
		// Set data
	}

	void setVoiceRec()
	{
	}
}

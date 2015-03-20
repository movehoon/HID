using UnityEngine;
using System;
using System.Collections;

public class StateOnClick : MonoBehaviour {

	void OnClick () 
	{
		UILabel label = gameObject.transform.Find("Label ID").GetComponentInChildren<UILabel> ();
		string[] words = label.text.Split('-');
		byte state = Convert.ToByte (words [0]);
		byte substate = Convert.ToByte (words [1]);
		GameObject.Find ("@Program").GetComponentInChildren<Program> ().ChangeRobotStateN (state, substate);
#if UNITY_EDITOR
		Debug.Log ("OnClick: " + label.text);
#endif

//		UICenterOnChild center = NGUITools.FindInParents<UICenterOnChild>(gameObject);
//		UIPanel panel = NGUITools.FindInParents<UIPanel>(gameObject);
//		
//		if (center != null)
//		{
//			if (center.enabled)
//				center.CenterOn(transform);
//		}
//		else if (panel != null && panel.clipping != UIDrawCall.Clipping.None)
//		{
//			UIScrollView sv = panel.GetComponent<UIScrollView>();
//			Vector3 offset = -panel.cachedTransform.InverseTransformPoint(transform.position);
//			if (!sv.canMoveHorizontally) offset.x = panel.cachedTransform.localPosition.x;
//			if (!sv.canMoveVertically) offset.y = panel.cachedTransform.localPosition.y;
//			SpringPanel.Begin(panel.cachedGameObject, offset, 6f);
//		}
	}
}

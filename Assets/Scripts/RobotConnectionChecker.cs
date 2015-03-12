using UnityEngine;
using System.Collections;

public class RobotConnectionChecker : MonoBehaviour {

	public ProgramForRos program;

	void FixedUpdate () {
		UISprite sprite = GetComponentInChildren <UISprite> ();
		if (program.IsConnected ())
			sprite.color = new Color (0, 0, 255);
		else
			sprite.color = new Color (255, 255, 255);
	}
}

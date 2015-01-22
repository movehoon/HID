﻿using UnityEngine;
using System.Collections;

public class WebCamConnectionChecker : MonoBehaviour {

	public WebCamClient webCamClient;

	// Update is called once per frame
	void Update () {
		UISprite sprite = GetComponentInChildren <UISprite> ();
		if (webCamClient.IsConnected ())
			sprite.color = new Color (0, 0, 255);
		else
			sprite.color = new Color (255, 255, 255);
	}
}
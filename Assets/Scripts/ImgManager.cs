using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImgManager : MonoBehaviour {

	public static ImgManager instance;

	public GameObject texture;
	public List<Texture> textures = new List<Texture> ();

	public void ShowImage0 ()
	{
		UITexture tex = texture.GetComponentInChildren<UITexture> ();
		tex.mainTexture = textures[0];
	}

	public void ShowImage1 ()
	{
		UITexture tex = texture.GetComponentInChildren<UITexture> ();
		tex.mainTexture = textures[1];
	}

	public void ShowImage2 ()
	{
		UITexture tex = texture.GetComponentInChildren<UITexture> ();
		tex.mainTexture = textures[2];
	}

	public void SetImage(int n)
	{

	}

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}

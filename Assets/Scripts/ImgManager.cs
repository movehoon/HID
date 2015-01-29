using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImgManager : MonoBehaviour {

	public static ImgManager instance;

	public GameObject texture;
	public List<Texture> textures = new List<Texture> ();

	public void ShowImage0 ()
	{
		SetImage (null);
	}

	public void ShowImage1 ()
	{
		SetImage (textures[1]);
	}

	public void ShowImage2 ()
	{
		SetImage (textures[2]);
	}

	public void SetImage(Texture tex)
	{
		UITexture tx = texture.GetComponentInChildren<UITexture> ();
		tx.mainTexture = tex;
	}

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImgManager : MonoBehaviour {

	public static ImgManager instance;

	public GameObject texture;
	public List<Texture> textures = new List<Texture> ();

	public float SlideTimeSec = 4f;

	public void ShowImage (int n)
	{
		switch (n) 
		{
		case 0:
			SetImage (textures[0]);
			break;
		case 15:
			SetImage (textures[1]);
			break;
		case 16:
			SetImage (textures[2]);
			break;
		case 17:
			SetImage (textures[3]);
			break;
		case 18:
			SetImage (textures[4]);
			break;
		case 42:
			SetImage (textures[5]);
			break;
		case 43:
			SetImage (textures[6]);
			break;
		case 44:
			SetImage (textures[7]);
			break;
		case 52:
			SetImage (textures[8]);
			break;
		case 53:
			SetImage (textures[9]);
			break;
		case 54:
			SetImage (textures[10]);
			break;
		case 55:
			SetImage (textures[11]);
			break;
		case 56:
			SetImage (textures[12]);
			Invoke ("ShowFanfare", 2f);
			break;
		case 84:
			count = 11;
			imageIndex = 13;
			ShowCountdownImage ();
			break;
		}
	}

	void ShowFanfare ()
	{
		SetImage (textures[7]);
	}

	public void SetImage(Texture tex)
	{
		UITexture tx = texture.GetComponentInChildren<UITexture> ();
		tx.mainTexture = tex;
	}

	int count;
	int imageIndex;
	void ShowCountdownImage()
	{
		SetImage (textures [imageIndex++]);
		count--;
		if (count > 0)
			Invoke ("ShowCountdownImage", SlideTimeSec);
		else
			SetImage (textures [0]);
	}

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}

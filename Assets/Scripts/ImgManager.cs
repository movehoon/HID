using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImgManager : MonoBehaviour {

	public static ImgManager instance;

	public GameObject texture;
	public List<Texture> textures = new List<Texture> ();

	public UITexture before;
	public UITexture after;

	public float SlideTimeSec = 5f;

	public void ShowReadyImage ()
	{
		SetImage (textures[0]);
	}

	public void ShowImage (int n)
	{
		switch (n) 
		{
		case 0:
			SetImage (textures[0]);
			break;
		case 15:
			Invoke ("ShowOrganizeMethodShape", 7f);
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
		case 61:
			SetImage (textures[25]);
			break;
		case 84:
			count = 11;
			imageIndex = 13;
			ShowCountdownImage ();
			break;
		case 102:
		case 111:
			// TODO: Show before and after image
			Invoke ("ShowBeforeAfter", 2f);
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

	void ShowOrganizeMethodShape ()
	{
		SetImage (textures[2]);
		Invoke ("ShowOrganizeMethodSize", 1.5f);
	}
	void ShowOrganizeMethodSize ()
	{
		SetImage (textures[3]);
		Invoke ("ShowOrganizeMethodColor", 3f);
	}
	void ShowOrganizeMethodColor ()
	{
		SetImage (textures[4]);
		Invoke ("ShowOrganizeMethodSize", 2f);
	}
	void ShowOrganizeMethod ()
	{
		SetImage (textures[1]);
	}

	public void ShowBeforeAfter ()
	{
		SetImage (textures[24]);

		before.gameObject.SetActive (true);
		Material tempMat = null;
		if (before.material == null)
		{
			tempMat = new Material(Shader.Find("Unlit/Transparent Colored"));
		}
		else
		{
			tempMat = new Material(before.material);
		}
		before.material = tempMat;
		before.mainTexture = LoadPNG (Application.dataPath + "/before.png");
		before.MakePixelPerfect();
		
		after.gameObject.SetActive (true);
		if (after.material == null)
		{
			tempMat = new Material(Shader.Find("Unlit/Transparent Colored"));
		}
		else
		{
			tempMat = new Material(after.material);
		}
		after.material = tempMat;
		after.mainTexture  = LoadPNG (Application.dataPath + "/after.png");
		after.MakePixelPerfect();
		
		Invoke ("HideBeforeAfter", 15f);
	}
	
	void HideBeforeAfter ()
	{
		before.gameObject.SetActive (false);
		after.gameObject.SetActive (false);
		ShowReadyImage();
	}
	
	static Texture2D LoadPNG(string filePath) {
		
		Texture2D tex = null;
		byte[] fileData;
		
		if (System.IO.File.Exists(filePath))     {
			fileData = System.IO.File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData);
		}
		return tex;
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
			Invoke ("ShowReadyImage", SlideTimeSec);
	}

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}

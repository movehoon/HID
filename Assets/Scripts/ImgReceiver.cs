using UnityEngine;
using System.Collections;
using System.IO;

public class ImgReceiver : MonoBehaviour {

	Texture2D texture;

	// Use this for initialization
	void Start () {
		texture = new Texture2D (Webcam.WEBCAM_WIDTH, Webcam.WEBCAM_HEIGHT);
	}
	
	public void DrawImg (byte[] img) {
		texture.LoadImage (img);
		texture.Apply ();
		renderer.material.mainTexture = texture;
//		File.WriteAllBytes ("dst.png", png);
	}
}

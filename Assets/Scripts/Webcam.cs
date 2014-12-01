using UnityEngine;
using System.Collections;
using System.IO;

public class Webcam : MonoBehaviour {

	public const int WEBCAM_WIDTH = 1280;
	public const int WEBCAM_HEIGHT = 720;

	public GameObject receiverObject;

	WebCamTexture webcam = null;
	Texture2D texture;

	// Use this for initialization
	void Start () {
		webcam = new WebCamTexture (WEBCAM_WIDTH, WEBCAM_HEIGHT, 10);
		webcam.Play ();
		if (webcam != null)
			texture = new Texture2D (WEBCAM_WIDTH, WEBCAM_HEIGHT);
	}
	
	// Update is called once per frame
	void Update () {
		if (!webcam.isPlaying)
			return;

		if (webcam.width == WEBCAM_WIDTH && webcam.height == WEBCAM_HEIGHT) 
		{
			renderer.material.mainTexture = webcam;
			texture.SetPixels (webcam.GetPixels ());
			texture.Apply ();
			byte[] imgBytes = texture.EncodeToJPG ();
//			File.WriteAllBytes("src.png", webcamToPng);
			Debug.Log ("Webcam is (" + webcam.width + ", " + webcam.height + ") with size " + imgBytes.GetLength (0) + "bytes");

			ImgReceiver imgReceiver = receiverObject.GetComponentInChildren <ImgReceiver> () as ImgReceiver;
			imgReceiver.DrawImg (imgBytes);
		}
	}
}

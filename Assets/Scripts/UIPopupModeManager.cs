using UnityEngine;
using System.Collections;

public class UIPopupModeManager : MonoBehaviour {

	public UILabel[] labelModes;
	public int currentMode = 0;

	Color colorSelected = new Color(255/255f, 23/255f, 59/255f);
	Color colorDeselected = new Color(0/255f, 0/255f, 0/255f);

	void Start ()
	{
		SetMode (0);
	}

	public void SetMode(int mode)
	{
		currentMode = mode;
		for (int i = 0 ; i < labelModes.Length ; i++) {
			if (i == mode)
				labelModes[i].color = colorSelected;
			else
				labelModes[i].color = colorDeselected;
		}
	}

	public void ClickMode0 ()
	{
		SetMode (0);
	}
	
	public void ClickMode1 ()
	{
		SetMode (1);
	}
	
	public void ClickMode2 ()
	{
		SetMode (2);
	}
	
	public void ClickMode3 ()
	{
		SetMode (3);
	}
}

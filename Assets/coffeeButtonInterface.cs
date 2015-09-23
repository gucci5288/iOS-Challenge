using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class coffeeButtonInterface : MonoBehaviour 
{
	public int index = 0;
	public Text textUI = null;
	public Text textUI_Front = null;
	public RawImage rawImage = null;

	public CoffeeInfo coffeeInfo_ButtonTemp;

	public MenuControl menuItem;
	

	public void On_Click_Button()
	{
		if (coffeeInfo_ButtonTemp != null) 
		{
			MainSceneControl.instance.SetState (SceneState.LookMenuItem);
			menuItem.setMenuContect (coffeeInfo_ButtonTemp);
			menuItem.currentID = index;
		} 
		else 
		{
			Debug.LogWarning("coffeeInfo_ButtonTemp is null");
		}

	}

}

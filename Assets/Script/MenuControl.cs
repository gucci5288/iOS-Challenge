using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour 
{
	public Text TextLabel_Name = null;
	public Text TextLabel_sourceLand = null;
	public Text TextLabel_feature = null;
	public Text TextLabel_story = null;
	public RawImage rawImg = null;
	
	public int currentID = 0;

	// Update is called once per frame
	void Update () {
	
	}

	public void setMenuContect(CoffeeInfo coffeeInfo)
	{
		this.gameObject.SetActive (true);

		TextLabel_Name.text = coffeeInfo.coffee_name;
		TextLabel_sourceLand.text = coffeeInfo.from;
		TextLabel_feature.text = coffeeInfo.feature;
		TextLabel_story.text = coffeeInfo.story;
		rawImg.texture = coffeeInfo.texture;
	}

	public void OnClickNext()
	{	

			if(currentID < NetWorkInitCoffeeData.instance.CoffeeInfoList.Count - 1 )
			{
				currentID++;
			}
			else
			{
				currentID = 0;
			}

			setMenuContect(NetWorkInitCoffeeData.instance.CoffeeInfoList[currentID]);

	}

	public void OnClickBack()
	{	

			if(currentID > 0)
			{
				currentID--;
			}
			else
			{
				currentID = NetWorkInitCoffeeData.instance.CoffeeInfoList.Count -1;
			}

			setMenuContect(NetWorkInitCoffeeData.instance.CoffeeInfoList[currentID]);

	}

	public void OnClickCloseMenu()
	{


			MainSceneControl.instance.SetState(SceneState.LookMenuList);

	}

}

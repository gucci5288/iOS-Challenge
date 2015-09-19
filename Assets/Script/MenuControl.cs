using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour 
{
	List<CoffeeInfo> CoffeeList;

	public Text TextLabel_Name = null;
	public Text TextLabel_sourceLand = null;
	public Text TextLabel_feature = null;
	public Text TextLabel_story = null;

	public int currentID = 0;

	// Use this for initialization
	void Awake () 
	{	

		CoffeeList = new List<CoffeeInfo> ();
		CoffeeToJson ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setMenuContect(int id)
	{
		TextLabel_Name.text = CoffeeList[id].coffee_name;
		TextLabel_sourceLand.text = CoffeeList[id].from;
		TextLabel_feature.text = CoffeeList[id].feature;
		TextLabel_story.text = CoffeeList[id].story;
	}


	public void CoffeeToJson()
	{
		CoffeeInfo coffee1 = new CoffeeInfo ();

		coffee1.coffee_name = "Coffee 01";
		coffee1.from = "coffee 01 Sourceland";
		coffee1.feature = "coffee 01 Feature";
		coffee1.story = "coffee 01 Story";

		CoffeeInfo coffee2 = new CoffeeInfo ();

		coffee2.coffee_name = "Coffee 02";
		coffee2.from = "coffee 02 Sourceland";
		coffee2.feature = "coffee 02 Feature";
		coffee2.story = "coffee 02 Story";

		CoffeeInfo coffee3 = new CoffeeInfo ();
		
		coffee3.coffee_name = "Coffee 03";
		coffee3.from = "coffee 03 Sourceland";
		coffee3.feature = "coffee 03 Feature";
		coffee3.story = "coffee 03 Story";

		CoffeeList.Add (coffee1);
		CoffeeList.Add (coffee2);
		CoffeeList.Add (coffee3);
		
		
		//string json_coffee = JsonMapper.ToJson(coffee);
		
		Debug.Log("Coffee Inited");
	}

	public void OnClickNext()
	{	
		//check if this is Menu state
		if (MainSceneControl.instance.IsMenuNow()) 
		{
			if(currentID < CoffeeList.Count -1 )
			{
				currentID++;
			}
			else
			{
				currentID = 0;
			}

			setMenuContect(currentID);
		}
	}

	public void OnClickBack()
	{	
		//check if this is Menu state
		if (MainSceneControl.instance.IsMenuNow()) 
		{

			if(currentID > 0)
			{
				currentID--;
			}
			else
			{
				currentID = CoffeeList.Count -1;
			}

			setMenuContect(currentID);
		}
	}

	public void OnClickCloseMenu()
	{

		//check if this is Menu state
		if (MainSceneControl.instance.IsMenuNow()) 
		{
			MainSceneControl.instance.SetState(SceneState.LookScene);
		}
	}

}

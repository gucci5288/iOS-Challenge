using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuControl : MonoBehaviour 
{

	public Text TextLabel_Name = null;
	public Text TextLabel_sourceLand = null;
	public Text TextLabel_feature = null;
	public Text TextLabel_story = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setMenuContect(CoffeeInfo coffee)
	{
		TextLabel_Name.text = coffee.Name;
		TextLabel_sourceLand.text = coffee.Sourceland;
		TextLabel_feature.text = coffee.Feature;
		TextLabel_story.text = coffee.Story;
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

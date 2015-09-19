using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

using UnityEngine;
using System.Collections;

public enum SceneState
{
	LookScene,
	LookMenu
}


public class MainSceneControl : MonoBehaviour 
{

	public static MainSceneControl instance = null;

	public SceneState State = SceneState.LookScene;

	public MenuControl menu = null;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}


		SetState (SceneState.LookScene);
	
	}	


	public bool IsMenuNow()
	{
		return State == SceneState.LookMenu;
	}

	public bool IsSceneNow()
	{
		return State == SceneState.LookScene;
	}


	public void SetState(SceneState state)
	{
		State = state;

		if (state == SceneState.LookScene) 
		{
			menu.gameObject.SetActive (false);
		} 
		else if (state == SceneState.LookMenu) 
		{
			menu.gameObject.SetActive (true);
		}

	}



}

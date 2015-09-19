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
	public Camera_virtualTouch virtualTouch= null;
	public MenuControl menu = null;

	void Start()
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
			virtualTouch.joystickRight.gameObject.SetActive(true);
			virtualTouch.joystickLeft.gameObject.SetActive(true);
		} 
		else if (state == SceneState.LookMenu) 
		{
			menu.gameObject.SetActive (true);
			virtualTouch.joystickRight.gameObject.SetActive(false);
			virtualTouch.joystickLeft.gameObject.SetActive(false);
		}

	}



}

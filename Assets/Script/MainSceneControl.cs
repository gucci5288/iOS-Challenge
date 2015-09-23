using UnityEngine;
using System.Collections;

public enum SceneState
{
	LookScene,
	LookMenuList,
	LookMenuItem
}


public class MainSceneControl : MonoBehaviour 
{

	public static MainSceneControl instance = null;

	public SceneState State = SceneState.LookScene;
	public Camera_virtualTouch virtualTouch= null;

	public GameObject MenuListUI = null;
	public GameObject MenuItemUI = null;

	void Start()
	{
		if (instance == null) 
		{
			instance = this;
		}

		SetState (SceneState.LookScene);
	}	


	public bool IsMenuListNow()
	{
		return State == SceneState.LookMenuList;
	}

	public bool IsMenuItemNow()
	{
		return State == SceneState.LookMenuItem;
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
			MenuListUI.gameObject.SetActive(false);
			MenuItemUI.gameObject.SetActive(false);

			virtualTouch.joystickRight.gameObject.SetActive(true);
			virtualTouch.joystickLeft.gameObject.SetActive(true);
		} 
		else if (state == SceneState.LookMenuList) 
		{
			MenuListUI.gameObject.SetActive (true);
			MenuItemUI.gameObject.SetActive(false);

			virtualTouch.joystickRight.gameObject.SetActive(false);
			virtualTouch.joystickLeft.gameObject.SetActive(false);
		}
		else if (state == SceneState.LookMenuItem) 
		{
			MenuListUI.gameObject.SetActive (false);
			MenuItemUI.gameObject.SetActive(true);

			virtualTouch.joystickRight.gameObject.SetActive(false);
			virtualTouch.joystickLeft.gameObject.SetActive(false);
		}

	}



}

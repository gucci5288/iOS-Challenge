using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuListControl : MonoBehaviour 
{
	

	public Transform Grid = null;

	public GameObject cloneButton = null;

	public GridLayoutGroup GridGroup = null;

	public GameObject NextPageButton = null;
	public GameObject BackPageButton = null;
	 
	public void SettingGridItems( List<CoffeeInfo> coffeeInfoList)
	{

		if (coffeeInfoList.Count > 9) {
			NextPageButton.SetActive (true);
			BackPageButton.SetActive (true);
		} 
		else 
		{
			NextPageButton.SetActive (false);
			BackPageButton.SetActive (false);
		}


		int index = 0;

		foreach (CoffeeInfo coffee in coffeeInfoList) 
		{
			GameObject button = Instantiate(cloneButton) as GameObject;

			button.name = "CoffeeItem " + index.ToString();

			button.GetComponent<RectTransform>().SetParent(Grid);
			button.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

			coffeeButtonInterface buttonUI = button.GetComponent<coffeeButtonInterface>();

			buttonUI.index = index;
			buttonUI.textUI.text = coffee.coffee_name;
			buttonUI.textUI_Front.text = coffee.coffee_name;
			buttonUI.rawImage.texture = coffee.texture;
			buttonUI.coffeeInfo_ButtonTemp = coffee;

			index++;
		}

		//GridGroup.CalculateLayoutInputVertical ();
	}

	public void OnClickNext()
	{	
		Debug.Log ("OnClickNext..");
	}
	
	public void OnClickBack()
	{	
		Debug.Log ("OnClickBack..");

	}
	
	public void OnClickCloseMenu()
	{

		MainSceneControl.instance.SetState(SceneState.LookScene);
		
	}

}

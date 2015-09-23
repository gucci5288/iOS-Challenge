using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class NetWorkInitCoffeeData : MonoBehaviour 
{
	public static NetWorkInitCoffeeData instance = null;

	public NetWorkBase netWorkBase;
	public MenuControl menuControl;
	public MenuListControl menuListControl = null;

	public List<CoffeeInfo> CoffeeInfoList = new List<CoffeeInfo> ();

	// Use this for initialization
	void Start () 
	{
		if (instance == null) 
		{
			instance = this;
		}

		StartCoroutine (Init ());
	}
	
	IEnumerator Init()
	{
		WWW coffeeWWW = netWorkBase.GET ("http://52.69.250.119/getAllCoffee");

		yield return coffeeWWW;
    
		TestReadingArray (coffeeWWW.text);
	}

	void TestReadingArray(string json_array)
	{
		CoffeeInfo[] coffeeArray;

		coffeeArray = JsonMapper.ToObject<CoffeeInfo[]>(json_array);

		for (int i = 0; i < coffeeArray.Length; i++) 
		{
			showCoffeeLog(coffeeArray[i] , i);
		}

		//set 9-grid List UI
		menuListControl.SettingGridItems (CoffeeInfoList);

	}


	void showCoffeeLog(CoffeeInfo coffee,int index)
	{
		Debug.Log(coffee.id);
		Debug.Log(coffee.coffee_name);
		Debug.Log(coffee.from);
		Debug.Log (coffee.story);
		Debug.Log (coffee.img);

		char[] seprator = {','};

		string[] base64String = coffee.img.Split (seprator, StringSplitOptions.None);
	
		byte[] b64_bytes = System.Convert.FromBase64String(base64String[1]); 

		Texture2D tex = new Texture2D(1,1);
		tex.LoadImage (b64_bytes);

		coffee.texture = tex;

		CoffeeInfoList.Add(coffee);
	}

}

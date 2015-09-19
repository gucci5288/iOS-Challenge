using UnityEngine;
using System.Collections;
using LitJson;

public class CoffeeContainer : MonoBehaviour 
{
	public int coffeeID;

	private CoffeeInfo coffee = null;

	void Start () 
	{
		CoffeeToJson ();
	}

	void Update () 
	{
	
	}

	public CoffeeInfo GetCoffeeInfo()
	{
		return coffee;
	}

	public void CoffeeToJson()
	{
		coffee = new CoffeeInfo ();


		if (coffeeID == 1) 
		{
			coffee.coffee_name = "Coffee 01";
			coffee.from = "coffee 01 Sourceland";
			coffee.feature = "coffee 01 Feature";
			coffee.story = "coffee 01 Story";
		} 
		else 
		{
			coffee.coffee_name = "Coffee 02";
			coffee.from = "coffee 02 Sourceland";
			coffee.feature = "coffee 02 Feature";
			coffee.story = "coffee 02 Story";
		}


		string json_coffee = JsonMapper.ToJson(coffee);
		
		Debug.Log(json_coffee);
	}
	

}

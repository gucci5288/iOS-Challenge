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
			coffee.Name = "Coffee 01";
			coffee.Sourceland = "coffee 01 Sourceland";
			coffee.Feature = "coffee 01 Feature";
			coffee.Story = "coffee 01 Story";
		} 
		else 
		{
			coffee.Name = "Coffee 02";
			coffee.Sourceland = "coffee 02 Sourceland";
			coffee.Feature = "coffee 02 Feature";
			coffee.Story = "coffee 02 Story";
		}


		string json_coffee = JsonMapper.ToJson(coffee);
		
		Debug.Log(json_coffee);
	}
	

}

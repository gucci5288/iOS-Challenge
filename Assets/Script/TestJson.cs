using UnityEngine;
using System.Collections;
using LitJson;
using System;

public class TestJson : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
	
		PersonToJson();
		JsonToPerson();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public class Person
	{
		// C# 3.0 auto-implemented properties
		public string   Name     { get; set; }
		public int      Age      { get; set; }
		public DateTime Birthday { get; set; }
	}

		public void PersonToJson()
		{
			Person bill = new Person();
			
			bill.Name = "William Shakespeare";
			bill.Age  = 51;
			bill.Birthday = new DateTime(1564, 4, 26);
			
			string json_bill = JsonMapper.ToJson(bill);
			
			Debug.Log(json_bill);
		}
		
		public void JsonToPerson()
		{
			string json = @"
            {
                ""Name""     : ""Thomas More"",
                ""Age""      : 57,
                ""Birthday"" : ""02/07/1478 00:00:00""
            }";
			
			Person thomas = JsonMapper.ToObject<Person>(json);
			
			Debug.Log("Thomas' age:"+ thomas.Age.ToString());
		}


}

using UnityEngine;
using System.Collections;

public class TouchLogic : MonoBehaviour 
{
	public  Camera Main_camera = null;

	void Update () 
	{
		if (MainSceneControl.instance.IsSceneNow()) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				CreateRayCast ();
			}
		}
	}

	void CreateRayCast()
	{
		Ray ray = Main_camera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitinfo2;
		
		int mapLayerInt = LayerMask.NameToLayer("map");

		if (Physics.Raycast (ray, out hitinfo2 , 10000 , 1 << mapLayerInt  ))
		{
			Debug.Log( "mouse hititem : "+ hitinfo2.collider.gameObject.name +" // point: "+hitinfo2.point);

			CoffeeContainer coffeeContain = hitinfo2.collider.gameObject.GetComponent<CoffeeContainer>();
			
			if(coffeeContain != null)
			{
				MainSceneControl.instance.menu.setMenuContect(coffeeContain.GetCoffeeInfo());
				MainSceneControl.instance.SetState(SceneState.LookMenu);
			}


		}
	}
}

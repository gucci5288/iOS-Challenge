/* Written by Kaz Crowe */
/* CreateUltimateJoystickEditor.cs ver. 1.1 */
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateUltimateJoystickEditor
{
	// This creates our menu within the UI section
	[ MenuItem( "GameObject/UI/Ultimate Joystick/Ultimate" ) ]
	private static void CreateUltimateJoystick ()
	{
		// Find our joystickPrefab
		GameObject joystickPrefab = Resources.Load( "UltimateJoystick", typeof( GameObject ) ) as GameObject;

		// If we found the prefab, create a joystick with the prefab we just got
		if( joystickPrefab != null )
			CreateJoystick( joystickPrefab );
		// else we have no prefab, or it's in the wrong folder, we need to put an error in the console
		else
			Debug.LogError( "Could not find 'UltimateJoystick.prefab' in any Resources folders." );
	}

	// Repeat above but with our Standard Joystick
	[ MenuItem( "GameObject/UI/Ultimate Joystick/Simple" ) ]
	private static void CreateStandardJoystick ()
	{
		GameObject joystickPrefab = Resources.Load( "StandardJoystick", typeof( GameObject ) ) as GameObject;

		if( joystickPrefab != null )
			CreateJoystick( joystickPrefab );
		else
			Debug.LogError( "Could not find 'StandardJoystick.prefab' in any Resources folders." );
	}
	
	private static void CreateJoystick ( Object joystickPrefab )
	{
		// create our prefab in our scene
		GameObject instJoy = ( GameObject )Object.Instantiate( joystickPrefab, Vector3.zero, Quaternion.identity );

		// Our instJoy.name currently has (Clone) at the end, so rename it to our original
		instJoy.name = joystickPrefab.name;

		// Focus on the new GameObject
		Selection.activeGameObject = instJoy;

		// Check if we need anything else created( Canvas, EventSystem )
		CheckNeededObjects( instJoy );
	}

	private static void CheckNeededObjects ( GameObject joystick )
	{
		// Find if we have a canvas in the scene
		Canvas currCanvas = ( Canvas )GameObject.FindObjectOfType( typeof( Canvas ) );
		
		// If we do, then set the joystick's parent to the canvas
		if( currCanvas != null )
			joystick.transform.SetParent( currCanvas.transform, false );
		// Else we need to create a new Canvas
		else
			CreateNewUI( joystick );
	}

	static public void CreateNewUI ( GameObject joystick )// This used to be a gameObject to return
	{
		// Root for the UI
		GameObject root = new GameObject( "Canvas" );
		root.layer = LayerMask.NameToLayer( "UI" );
		Canvas canvas = root.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		root.AddComponent<CanvasScaler>();
		root.AddComponent<GraphicRaycaster>();
		Undo.RegisterCreatedObjectUndo( root, "Create " + root.name );

		// Here we set the joystick to be a child of the canvas
		joystick.transform.SetParent( root.transform, false );

		// if there is no event system add one...
		CreateEventSystem( root.gameObject );
	}

	private static void CreateEventSystem ( GameObject parent )
	{
		// Find an EventSystem if it is active
		Object esys = Object.FindObjectOfType<EventSystem>();
		if( esys == null )
		{
			GameObject eventSystem = new GameObject( "EventSystem" );
			GameObjectUtility.SetParentAndAlign( eventSystem, parent );
			esys = eventSystem.AddComponent<EventSystem>();
			eventSystem.AddComponent<StandaloneInputModule>();
			eventSystem.AddComponent<TouchInputModule>();
			
			Undo.RegisterCreatedObjectUndo( eventSystem, "Create " + eventSystem.name );
		}
	}
}
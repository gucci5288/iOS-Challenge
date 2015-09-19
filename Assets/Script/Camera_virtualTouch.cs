using UnityEngine;
using System.Collections;


[RequireComponent( typeof( Rigidbody ) )]
public class Camera_virtualTouch : MonoBehaviour
{
	/* Transform's and Movement Variables */
	Transform myTransform;

	public Transform playerCameraPivot;
	public float speed = 0.05f;
	float rotationSpeed = 50;
	float cameraRotationSpeed = 2.0f;
	
	/* Joystick's */
	public UltimateJoystick joystickLeft;
	public UltimateJoystick joystickRight;
	
	
	void Start () 
	{
		// We need to store some variables before we do anything else
		myTransform = GetComponent<Transform>();

	}
	
	void Update ()
	{
		// In order to use our joystick, we will call our  JoystickPosition which will return our Joystick's position as a Vector2.
		Vector2 joystickLeftPos = joystickLeft.JoystickPosition;
		Vector2 joystickRightPos = joystickRight.JoystickPosition;
		
		// If our joystickLeftPos is not equal to Vector2.zero, then that means we are touching it
		if( joystickLeftPos != Vector2.zero && MainSceneControl.instance.IsSceneNow())
		{
			// Since the camera is moving, we need to cast our vector3 into our current direction
			Vector3 movement = myTransform.TransformDirection( playerCameraPivot.forward );
			
			// Now add in our joysticks position
			movement = new Vector3( joystickLeftPos.x, 0, joystickLeftPos.y );
			
			// And apply that to our transform
			myTransform.Translate( movement * speed );
		}
		
		// This will make our camera stay with our player
		playerCameraPivot.position = myTransform.position;
		
		// If we are touching our right joystick, then we want to turn our camera
		if( joystickRightPos != Vector2.zero && MainSceneControl.instance.IsSceneNow())
		{
			// Temporary float to store our joystick's X position to our camera's rotation
			float camRotationX = joystickRightPos.x;
			
			// Apply our speed to that rotation
			camRotationX *= rotationSpeed * Time.deltaTime;
			
			// This will rotate myTransfrom with just a rotation, not a direction
			myTransform.Rotate( 0, camRotationX, 0, Space.World );
			
			// This will follow the rotation of myTransform, and is only updated when we are using the right joystick, so put it within here
			playerCameraPivot.rotation = Quaternion.Slerp( playerCameraPivot.rotation, myTransform.rotation, cameraRotationSpeed );
		}

	}
	
}
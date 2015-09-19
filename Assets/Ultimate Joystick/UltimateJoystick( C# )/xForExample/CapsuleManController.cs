/* Written by Kaz Crowe */
/* CapsuleManController.cs ver. 1.2.1 */
using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Rigidbody ) )]
public class CapsuleManController : MonoBehaviour
{
	/* Transform's and Movement Variables */
	Transform myTransform;
	Rigidbody myRigidbody;
	public Transform playerCameraPivot;
	public float speed = 0.1f;
	float rotationSpeed = 50;
	float cameraRotationSpeed = 2.0f;
	
	/* Joystick's */
	public UltimateJoystick joystickLeft;
	public UltimateJoystick joystickRight;

	/* Variables for Jumping */
	bool isGrounded = false;

	
	void Start () 
	{
		// We need to store some variables before we do anything else
		myTransform = GetComponent<Transform>();
		myRigidbody = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		// In order to use our joystick, we will call our  JoystickPosition which will return our Joystick's position as a Vector2.
		Vector2 joystickLeftPos = joystickLeft.JoystickPosition;
		Vector2 joystickRightPos = joystickRight.JoystickPosition;
		
		// If our joystickLeftPos is not equal to Vector2.zero, then that means we are touching it
		if( joystickLeftPos != Vector2.zero )
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
		if( joystickRightPos != Vector2.zero )
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

		// Call our jump check function
		PlayerJumpCheck();
	}

	void PlayerJumpCheck ()
	{
		// We need to check for the ground
		if( Physics.Raycast( myTransform.position, Vector3.down, 1.1f ) )
		{
			if( isGrounded == false )
				isGrounded = true;
		}
		else
			isGrounded = false;
	}
	
	public void PlayerJump ()
	{
		if( isGrounded == true )
		{
			Vector3 jumpVector = new Vector3( 0, 5, 0 );
			myRigidbody.velocity = jumpVector;
		}
	}
}
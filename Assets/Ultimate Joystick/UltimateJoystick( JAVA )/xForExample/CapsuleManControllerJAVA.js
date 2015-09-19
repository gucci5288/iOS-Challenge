/* Written by Kaz Crowe */
/* CapsuleManControllerJAVA.js ver. 1.2.1 */
#pragma strict
@script RequireComponent( Rigidbody );

/* Transform's and Movement Variables */
private var myTransform : Transform;
private var myRigidbody : Rigidbody;
var playerCameraPivot : Transform;
var speed : float = 0.1;
private var rotationSpeed : int = 50;
private var cameraRotationSpeed : float = 2.0;

/* Joystick's */
var joystickLeft : UltimateJoystickJAVA;
var joystickRight : UltimateJoystickJAVA;

/* Variables for Jumping */
private var isGrounded : boolean;


function Start () 
{
	// We need to store some variables before we do anything else
	myTransform = GetComponent( Transform );
	myRigidbody = GetComponent( Rigidbody );
}

function Update ()
{
	// In order to use our joystick, we will call our  JoystickPosition which will return our Joystick's position as a Vector2.
	var joystickLeftPos : Vector2 = joystickLeft.JoystickPosition;
//	joystickLeftPos = joystickLeftPos.CalculateUJDeadZone( 0.5f, 0.75f );
	var joystickRightPos : Vector2 = joystickRight.JoystickPosition;
	
	// If our joystickLeftPos is not equal to Vector2.zero, then that means we are touching it
	if( joystickLeftPos != Vector2.zero )
	{
		// Since the camera is moving, we need to cast our vector3 into our current direction
		var movement : Vector3 = myTransform.TransformDirection( playerCameraPivot.forward );
		
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
		var camRotationX : float = joystickRightPos.x;
		
		// Apply our speed to that rotation
		camRotationX *= rotationSpeed * Time.deltaTime;
		
		// This will rotate myTransfrom with just a rotation, not a direction
		myTransform.Rotate( 0, camRotationX, 0, Space.World );

		// This will follow the rotation of myTransform, and is only updated when we are using the right joystick, so put it within here
		playerCameraPivot.rotation = Quaternion.Slerp( playerCameraPivot.rotation, myTransform.rotation, cameraRotationSpeed );
	}
	
	// This will check for the ground and check for tap count on our right joystick
	PlayerJumpCheck();
}

private function PlayerJumpCheck ()
{
	// We need to check for the ground
	if( Physics.Raycast( myTransform.position, Vector3.down, 1.1 ) )
	{
		if( isGrounded == false )
			isGrounded = true;
	}
	else
		isGrounded = false;
}

function PlayerJump ()
{
	// Check if we are grounded
	if( isGrounded == true )
	{
		var jumpVector : Vector3 = new Vector3( 0, 5, 0 );
		myRigidbody.velocity = jumpVector;
	}
}
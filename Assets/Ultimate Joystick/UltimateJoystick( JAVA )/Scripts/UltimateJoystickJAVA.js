/* Written by Kaz Crowe */
/* UltimateJoystickJAVA.js ver. 1.5.7 */
#pragma strict

/* 
 * First off, we are using [ExecuteInEditMode] to be able to show changes in real time.
 * This will not affect anything within a build or play mode. This simply makes the script
 * able to be run while in the Editor in Edit Mode.
*/
@script ExecuteInEditMode()
public class UltimateJoystickJAVA extends MonoBehaviour implements UnityEngine.EventSystems.IPointerDownHandler, UnityEngine.EventSystems.IDragHandler, UnityEngine.EventSystems.IPointerUpHandler
{
	/* Basic Variables */
	var joystick : Transform;
	var joystickSizeFolder : RectTransform;
	private var joystickCenter : Vector3;
	private var textureCenter : Vector2;
	private var defaultPos : Vector2;
	/* Size and Placement */
	enum Anchor
	{
		Left,
		Right
	}
	var anchor : Anchor;
	enum JoystickTouchSize
	{
		Default,
		Medium,
		Large
	}
	var joystickTouchSize : JoystickTouchSize;
	var joystickSize : float = 1.75;
	var radiusModifier : float = 4.5;
	private var radius : float;
	var touchBasedPositioning : boolean = false;
	var overrideTouchSize : boolean = false;
	var tbp_X : float = 50.0f;
	var tbp_Y : float = 75.0f;
	var cs_X : float = 5.0f;
	var cs_Y : float = 20.0f;
	/* Style and Options */
	var touchPad : boolean = false;
	var throwable : boolean = false;
	var throwDuration : float = 0.5f;
	var showHighlight : boolean = false;
	var highlightColor : Color = new Color( 1, 1, 1, 1 );
	var highlightBase : UnityEngine.UI.Image;
	var highlightJoystick : UnityEngine.UI.Image;
	var showTension : boolean = false;
	var tensionColorNone : Color = new Color( 1, 1, 1, 1 );
	var tensionColorFull : Color = new Color( 1, 1, 1, 1 );
	var tensionAccentUp : UnityEngine.UI.Image;
	var tensionAccentDown : UnityEngine.UI.Image;
	var tensionAccentLeft : UnityEngine.UI.Image;
	var tensionAccentRight : UnityEngine.UI.Image;
	enum Axis
	{
		Both,
		X,
		Y
	}
	var axis : Axis;
	enum Boundary
	{
		Circular,
		Square
	}
	var boundary : Boundary;
	/* Touch Actions */
	enum TapCountOption
	{
		NoCount,
		Accumulate,
		TouchRelease
	}
	var tapCountOption : TapCountOption;
	var tapCountDuration : float = 0.5;
	var tapCountEvent : UnityEngine.Events.UnityEvent;
	var targetTapCount : int = 2;
	var currentTapTime : float = 0.0;
	private var countingDown : boolean;
	private var tapCount : int = 0;
	var useAnimation : boolean;
	var joystickAnimator : Animator;
	var useFade : boolean;
	var fadeUntouched : float = 1.0;
	var fadeTouched : float = 0.5;
	var joystickBase : UnityEngine.UI.Image;


	function Start ()
	{
		if( Application.isPlaying == true )
		{
			// First off, UpdatePositioning of the joystick
			UpdatePositioning();

			// Call Setup functions
			SetHighlight();
			SetTensionAccent();
			if( useAnimation == true )
				SetAnimation();
		}
	}
	
	// This function allows other scripts to get our joysticks position data
	function get JoystickPosition () : Vector2
	{
		// Make a temporary Vector2 and divide it by our radius to give us a value between -1 and 1
		var tempVec : Vector2 = joystick.position - joystickCenter;
		return tempVec / radius;
	}
	
	// This function gets us the distance of the joystick from center
	function get JoystickDistance () : float
	{
		var joystickDistance : float = Vector3.Distance( joystick.position, joystickCenter ) / radius;
		return joystickDistance;
	}
	
	// This means we have touched, so process where we have touched
	function OnPointerDown ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// If we have a throwable joystick, then we want to stop the current moving
		if( throwable == true )
			StopCoroutine( "ThrowableMovement" );
			
		// If we are using the Touch Based Positioning we need to do a few things here
		if( touchBasedPositioning == true || touchPad == true )
		{
			// We need to move our joysticSizeFolder to the point of our touch
			joystickSizeFolder.position = touchInfo.position - textureCenter;

			// Set our center to the touch position since it has changed
			joystickCenter = touchInfo.position;
		}

		// Call UpdateJoystick with the info from our PointerEventData
		UpdateJoystick( touchInfo );

		// If we want to show animations on Touch, do that here
		if( useAnimation == true )
			joystickAnimator.SetBool( "Touch", true );

		// If we are using our fade on touch do that here as well
		if( useFade == true )
			HandleFade( "Touched" );
			
		// Check if we are using any TapCountOption
		if( tapCountOption != TapCountOption.NoCount )
		{
			// If we are accumulating taps, then we need to do some things
			if( tapCountOption == TapCountOption.Accumulate )
			{
				// Here, if we aren't counting down, set tapCount to 1 since it's our first touch and start TapCounter
				if( countingDown == false )
				{
					countingDown = true;
					tapCount = 1;
					TapCounter();
				}
				// If we are currently counting down then increase tapCount
				else
					++tapCount;
			}
			// Otherwise we just set down touches as 1
			else
				TapCounterDelay();
		}
	}
	
	// This means we are moving
	function OnDrag ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// We need to call our UpdateJoystick and process where we are touching
		UpdateJoystick( touchInfo );
	}
	
	// This means we have let go
	function OnPointerUp ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// If we are using Touch Based Positioning, then we need to change a few things
		if( touchBasedPositioning == true || touchPad == true )
		{
			// we need to set our folder back to defaultPos
			joystickSizeFolder.position = defaultPos;

			// set our joystickCenter again since it changed when we touched down
			joystickCenter = joystickSizeFolder.position + textureCenter;
		}

		if( throwable == true )
			StartCoroutine( "ThrowableMovement" );
		else
		{
			// Set the joystick to our center
			joystick.position = joystickCenter;

			// If we have our highlightJoystick then we want to move it back to center
			if( showHighlight == true && highlightJoystick != null )
				highlightJoystick.transform.position = joystickCenter;
		}

		// If we are showing Tension Accents, then we need to reset if we don't have throwable on
		if( showTension == true && throwable == false )
			TensionAccentReset();

		// If we want to show animations on touch up, do that here
		if( useAnimation == true )
			joystickAnimator.SetBool( "Touch", false );

		// If we are using our fade on touch up
		if( useFade == true )
			HandleFade( "Untouched" );
			
		// Check if we are using TouchAndRelease tapcount and set tapCount to 0 since the touch has released
		if( tapCountOption == TapCountOption.TouchRelease && currentTapTime > 0 )
		{
			StopCoroutine( "TapCounterDelay" );
			tapCountEvent.Invoke();
		}
	}
	
	// This function updates our joystick according to our touch
	function UpdateJoystick ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// Create a new Vector2 to equal the vector from our curret touch to the center of joystick
		var tempVector : Vector2 = touchInfo.position - joystickCenter;

		// If we are using just X or Y, then we just need to zero out the right value
		if( axis == Axis.X )
			tempVector.y = 0;
		else if( axis == Axis.Y )
			tempVector.x = 0;

		if( boundary == Boundary.Circular )
		{
			// Clamp the Vector, which will give us a round boundary
			tempVector = Vector2.ClampMagnitude( tempVector, radius );
		}
		else if( boundary == Boundary.Square )
		{
			// We want to Clamp both X and Y seperately so we get a square boundary
			tempVector.x = Mathf.Clamp( tempVector.x,  -radius,  radius );
			tempVector.y = Mathf.Clamp( tempVector.y,  -radius,  radius );
		}

		// Apply the Vector to our Joystick's position
		joystick.transform.position = joystickCenter + tempVector;

		// If we have our highlightJoystick then we want to move to the same place as our joystick
		if( highlightJoystick != null && showHighlight == true )
			highlightJoystick.transform.position = joystick.transform.position;

		// If we are using the style Ultimate Joystick and if showTension is true, display Tension
		if( showTension == true )
			TensionAccentDisplay();
	}
	
	// This function updates our options. It is public so we can call it from other scripts to update our positioning
	public function UpdatePositioning ()
	{
		// We want our joystick size to be larger when we have a larger number, so we need to calculate that out
		var textureSize : float = Screen.height * ( joystickSize / 10 );

		// Same with our radius modifier
		radius = textureSize * ( radiusModifier / 10 );

		// We need to store this object's RectTrans so that we can position it
		var baseTrans : RectTransform = GetComponent( RectTransform );
		
		// Force our scale and rotation to our correct values
		if( baseTrans.transform.localScale != Vector3.one )
			baseTrans.transform.localScale = Vector3.one;
		if( baseTrans.transform.rotation != Quaternion.identity )
			baseTrans.transform.rotation = Quaternion.identity;

		// We need to get a position for our joystick based on our position variable
		var joystickTexturePosition : Vector2 = ConfigureTexturePosition( textureSize );

		// If we are wanting our touch size to override our JoystickTouchSize option
		if( overrideTouchSize == true )
		{
			// Fix our size variables
			var fixedFBPX : float = tbp_X / 100;
			var fixedFBPY : float = tbp_Y / 100;

			// Depending on our joystickTouchSize options, we need to configure our size
			baseTrans.sizeDelta = new Vector2( Screen.width * fixedFBPX, Screen.height * fixedFBPY );

			// If anchor is set to Left, then we want to set it in the bottom left corner
			if( anchor == Anchor.Left )
				baseTrans.position = new Vector2( 0, 0 );
			else // it is right, then we want it to be in the center of our screen
			{
				if( overrideTouchSize == true )
					baseTrans.position = new Vector2( Screen.width - baseTrans.sizeDelta.x, 0 );
				else
					baseTrans.position = new Vector2( Screen.width / 2, 0 );
			}
		}
		else
		{
			// Our touch size needs to be fixed to a float value
			var fixedTouchSize : float;
			if( joystickTouchSize == JoystickTouchSize.Large )
				fixedTouchSize = 2.0f;
			else if( joystickTouchSize == JoystickTouchSize.Medium )
				fixedTouchSize = 1.51f;
			else
				fixedTouchSize = 1.01f;

			// Make a temporary Vector2
			var tempVector : Vector2 = new Vector2( textureSize, textureSize );

			// Our touch area is standard, so set it up with our tempVector multiplied by our fixedTouchSize
			baseTrans.sizeDelta = tempVector * fixedTouchSize;

			// We get our texture position and modify it with our sizeDelta - tempVector ( gives us the difference ) and divide by 2
			baseTrans.position = joystickTexturePosition - ( ( baseTrans.sizeDelta - tempVector ) / 2 );
		}
		
		if( touchBasedPositioning == true || touchPad == true )
		{
			// We need to know our texture's center so that we can move it to our touch position correctly
			textureCenter = new Vector2( textureSize / 2, textureSize / 2 );
			
			// Also need to store our default position so that we can return after the touch has been lifted
			defaultPos = joystickTexturePosition;
		}
		
		// Our joystickSizeFolder needs to be textureSize and texture position
		joystickSizeFolder.sizeDelta = new Vector2( textureSize, textureSize );
		joystickSizeFolder.position = joystickTexturePosition;

		// Store the joystick center so we can return to it
		joystickCenter = joystick.position;
	}
	
	// This function will configure a Vector2 for the position of our Joystick
	function ConfigureTexturePosition ( textureSize : float ) : Vector2
	{
		// We need a few temporary Vector2's to work with
		var tempPosVector : Vector2;

		// we need to fix our custom spacing variables to be usable
		var fixedCSX : float = cs_X / 100;
		var fixedCSY : float = cs_Y / 100;

		// We also need two floats for applying our spacers according to our screen size
		var positionSpacerX : float = Screen.width * fixedCSX - ( textureSize * fixedCSX );
		var positionSpacerY : float = Screen.height * fixedCSY - ( textureSize * fixedCSY );

		// If it's left, we can simply apply our positionxSpacerX
		if( anchor == Anchor.Left )
			tempPosVector.x = positionSpacerX;
		// else it's right, we need to calculate out from our right side and apply our positionSpaceX
		else
			tempPosVector.x = ( Screen.width - textureSize ) - positionSpacerX;

		// Here we just apply our positionSpacerY
		tempPosVector.y = positionSpacerY;
		
		// Return our updated Vector2
		return tempPosVector;
	}
	
	// This function is called to set up our Highlight Images
	function SetHighlight ()
	{
		if( showHighlight == true )
		{
			// Enable images if they are off
			if( highlightBase != null && highlightBase.gameObject.activeInHierarchy == false )
				highlightBase.gameObject.SetActive( true );
			if( highlightJoystick != null && highlightJoystick.gameObject.activeInHierarchy == false )
				highlightJoystick.gameObject.SetActive( true );

			// Here we need to check if each variable is assigned so we don't get a null reference log error when applying color
			if( highlightBase != null )
				highlightBase.color = highlightColor;
			if( highlightJoystick != null )
				highlightJoystick.color = highlightColor;

			// If we are using fade, then we want to modify our highlight's color
			if( useFade == true )
				HandleFade( "Untouched" );
		}
		else
		{
			// Here we want to check if we have any highlights, and if so, SetActive( false )
			if( highlightBase != null && highlightBase.gameObject.activeInHierarchy == true )
				highlightBase.gameObject.SetActive( false );
			if( highlightJoystick != null && highlightJoystick.gameObject.activeInHierarchy == true )
				highlightJoystick.gameObject.SetActive( false );
		}
	}
	
	/* These next functions are for our Tension Accents */
	function SetTensionAccent ()
	{
		if( showTension == true )
		{
			// Check our images and make sure they are active
			if( tensionAccentUp != null && tensionAccentUp.gameObject.activeInHierarchy == false )
				tensionAccentUp.gameObject.SetActive( true );
			if( tensionAccentDown != null && tensionAccentDown.gameObject.activeInHierarchy == false )
				tensionAccentDown.gameObject.SetActive( true );
			if( tensionAccentLeft != null && tensionAccentLeft.gameObject.activeInHierarchy == false )
				tensionAccentLeft.gameObject.SetActive( true );
			if( tensionAccentRight != null && tensionAccentRight.gameObject.activeInHierarchy == false )
				tensionAccentRight.gameObject.SetActive( true );

			// We need to check if ANY of our tension accents are unassigned
			if( tensionAccentUp == null || tensionAccentDown == null || tensionAccentLeft == null || tensionAccentRight == null )
			{
				// Let the user know that we don't have the needed variables assigned
				Debug.LogError( "Not all Tension Accent's are assign. Please assign all Tension Accents before modifying color values." );
			}
			// else we have all variables assigned and we can use them without errors
			else
				TensionAccentReset();
		}
		else
		{
			// Here we want to check if we have any tension accents, and if so, SetActive( false )
			if( tensionAccentUp != null && tensionAccentUp.gameObject.activeInHierarchy == true )
				tensionAccentUp.gameObject.SetActive( false );
			if( tensionAccentDown != null && tensionAccentDown.gameObject.activeInHierarchy == true )
				tensionAccentDown.gameObject.SetActive( false );
			if( tensionAccentLeft != null && tensionAccentLeft.gameObject.activeInHierarchy == true )
				tensionAccentLeft.gameObject.SetActive( false );
			if( tensionAccentRight != null && tensionAccentRight.gameObject.activeInHierarchy == true )
				tensionAccentRight.gameObject.SetActive( false );
		}
	}
	
	// This function is called when our joystick is moving
	function TensionAccentDisplay ()
	{
		// We need a Vector2 to store our JoystickPosition
		var tension : Vector2 = JoystickPosition;

		// If our joystick is to the right
		if( tension.x > 0 )
		{
			// Then we lerp our color according to our X position
			tensionAccentRight.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );
			
			// If our tensionAccentLeft is not tensionColorNone, we want to make it so
			if( tensionAccentLeft.color != tensionColorNone )
				tensionAccentLeft.color = tensionColorNone;
		}
		// else our joystick is to the left
		else
		{
			/// Mathf.Abs gives us a positive number to work with
			tension.x = Mathf.Abs( tension.x );
			tensionAccentLeft.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );

			if( tensionAccentRight.color != tensionColorNone )
				tensionAccentRight.color = tensionColorNone;
		}

		// If our joystick is up
		if( tension.y > 0 )
		{
			// Then we lerp our color according to our Y position
			tensionAccentUp.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			// If our tensionAccentDown is not tensionColorNone, we want to make it so
			if( tensionAccentDown.color != tensionColorNone )
				tensionAccentDown.color = tensionColorNone;
		}
		// else it is down
		else
		{
			// Mathf.Abs gives us a positive number to work with
			tension.y = Mathf.Abs( tension.y );
			tensionAccentDown.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			if( tensionAccentUp.color != tensionColorNone )
				tensionAccentUp.color = tensionColorNone;
		}
	}
	
	function TensionAccentReset ()
	{
		// This resets our tension colors back to default
		tensionAccentUp.color = tensionColorNone;
		tensionAccentDown.color = tensionColorNone;
		tensionAccentLeft.color = tensionColorNone;
		tensionAccentRight.color = tensionColorNone;
	}
	
	// Enables/Disables animator component according to options
	function SetAnimation ()
	{
		// If we are using any animations, then make sure it's enabled
		if( useAnimation == true )
		{
			if( joystickAnimator != null )
				if( joystickAnimator.enabled == false )
					joystickAnimator.enabled = true;
		}
		// Else make sure that it is disabled
		else
		{
			if( joystickAnimator != null )
				if( joystickAnimator.enabled == true )
					joystickAnimator.enabled = false;
		}
	}
	
	// This function handles fading of the different parts of the Ultimate Joystick
	function HandleFade ( fadeAction : String )
	{
		// Temporary float to hold our modifier for our color.a
		var alphaMod : float;

		// Based on our fadeAction, we will modify our alphaMod for use
		if( fadeAction == "Touched" )
			alphaMod = fadeTouched;
		else if( fadeAction == "Untouched" )
			alphaMod = fadeUntouched;
		else
			alphaMod = 1.0f;

		// We need to check if both these are assigned
		if( joystickBase != null && joystick != null )
		{
			// And get a temporary color that is the same as our joystickBase, then change the alpha to our alphaMod
			var joystickColor : Color = joystickBase.color;
			joystickColor.a = alphaMod;

			// Now apply the temporary color to our joystickBase and joystick
			joystickBase.color = joystickColor;
			joystick.GetComponent( UnityEngine.UI.Image ).color = joystickColor;
		}

		// Check if we are truely using our highlights
		if( showHighlight == true )
		{
			// We want our fade to scale correctly with our current highlightColor.a
			var hlAlphaMod : float = highlightColor.a * alphaMod;
			
			// Check our highlightBase Image
			if( highlightBase != null )
			{
				// Temporary Color variable
				var highlightBaseColor : Color = highlightBase.color;

				// If we are using Fade, then we want to change to our alphaMod
				if( useFade == true )
					highlightBaseColor.a = hlAlphaMod;
				// However, if we are not, then we want our HL to be the same as our highlightColor option
				else
					highlightBaseColor.a = highlightColor.a;

				// Apply our new color to our highlightBase
				highlightBase.color = highlightBaseColor;
			}
			
			// Repeat the steps we just did for our highlightBase
			if( highlightJoystick != null )
			{
				var highlightJoyColor : Color = highlightJoystick.color;
				if( useFade == true )
					highlightJoyColor.a = hlAlphaMod;
				else
					highlightJoyColor.a = highlightColor.a;

				highlightJoystick.color = highlightJoyColor;
			}
		}
	}
	
	// This function delays for our tapCountDuration to accumulate taps
	private function TapCounter ()
	{
		var currentTime : float = 0.0;
		var continueCheck : boolean = true;
		while( currentTime < tapCountDuration && continueCheck == true )
		{
			currentTime += Time.deltaTime;
			if( tapCount >= targetTapCount )
			{
				continueCheck = false;
				tapCountEvent.Invoke();
			}
			yield;
		}
		
		// Then reset our tapCount and countingDown
		tapCount = 0;
		countingDown = false;
	}
	
	// This function delays for our tapCountDuration to accumulate taps
	private function TapCounterDelay ()
	{
		currentTapTime = tapCountDuration;
		while( currentTapTime >= 0 )
		{
			currentTapTime -= Time.deltaTime;
			yield;
		}
	}
	
	// Function for moving our joystickk kback to center over a set time
	function ThrowableMovement () : IEnumerator
	{
		// Fix our throwDuration into a speed
		var throwSpeed : float = 1.0 / throwDuration;

		// Store the position of where the joystick is currently
		var startJoyPos : Vector3 = joystick.position;

		// Boolean to control our slow at the end of the throw
		var hasSlowed : boolean = false;
		for( var i : float = 0.0; i < 1.0; i += Time.deltaTime * throwSpeed )
		{
			// Lerp our joystick's position from where this coroutine started to our center
			joystick.position = Vector3.Lerp( startJoyPos, joystickCenter, i );
			if( showHighlight == true && highlightJoystick != null )
				highlightJoystick.transform.position = joystick.position;
				
			// This will slow our joystick down at the end
			if( i >= 0.85 && hasSlowed == false )
			{
				hasSlowed = true;
				throwSpeed *= 0.5;
			}
			
			// If we are showing tension, then we want to display that as it moves
			if( showTension == true )
				TensionAccentDisplay();
			
			yield;
		}
		// Finalize our joystick's position
		joystick.position = joystickCenter;
		if( showHighlight == true && highlightJoystick != null )
			highlightJoystick.transform.position = joystick.position;
			
		// Here at the end, we want to just reset our tension
		if( showTension == true )
			TensionAccentReset();
	}
	
	/* 
	 * This function allows us to apply changes in screen size and joystick options in real time
	 * However anything within this #if section will not be run in a game build, only within Unity
	*/
#if UNITY_EDITOR
	private var lastScreenSize : Vector3;
	var needToUpdatePositioning : boolean = false;
	
	function Update ()
	{
		// We want to constantly keep our joystick updated while the game is not being run in Unity
		if( Application.isPlaying == false )
		{
			// But only if we have values that have changed
			if( needToUpdatePositioning == true )
				UpdatePositioning();
		}
		
		var currentScreenSize : Vector3 = new Vector2( Screen.width, Screen.height );
		// Or if our screen size has changed
		if( lastScreenSize != currentScreenSize )
		{
			lastScreenSize = currentScreenSize;
			UpdatePositioning();
		}
	}
#endif
}
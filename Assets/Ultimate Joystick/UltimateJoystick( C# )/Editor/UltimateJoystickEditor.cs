/* Written by Kaz Crowe */
/* UltimateJoystickEditor.cs ver. 1.2.1 */
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[ CanEditMultipleObjects ]
[ CustomEditor( typeof( UltimateJoystick ) ) ]
public class UltimateJoystickEditor : Editor
{
	CanvasScaler canvasRect;

	/* Assigned Variables */
	SerializedProperty joystick;
	SerializedProperty joystickSizeFolder;
	SerializedProperty highlightBase;
	SerializedProperty highlightJoystick;
	SerializedProperty tensionAccentUp;
	SerializedProperty tensionAccentDown;
	SerializedProperty tensionAccentLeft;
	SerializedProperty tensionAccentRight;
	SerializedProperty joystickAnimator;
	SerializedProperty joystickBase;

	/* Size and Placement */
	SerializedProperty anchor;
	SerializedProperty joystickTouchSize;
	SerializedProperty joystickSize;
	SerializedProperty radiusModifier;
	SerializedProperty tbp_X;
	SerializedProperty tbp_Y;
	SerializedProperty cs_X;
	SerializedProperty cs_Y;

	/* Style and Options */
	SerializedProperty throwDuration;
	SerializedProperty highlightColor;
	SerializedProperty tensionColorNone;
	SerializedProperty tensionColorFull;
	SerializedProperty axis;
	SerializedProperty boundary;

	/* Touch Actions */
	SerializedProperty tapCountOption;
	SerializedProperty tapCountDuration;
	SerializedProperty tapCountEvent;
	SerializedProperty targetTapCount;
	SerializedProperty fadeUntouched;
	SerializedProperty fadeTouched;


	void OnEnable ()
	{
		// Here we should Initialize all of our needed variables so that we don't have to find them each updated frame
		joystick = serializedObject.FindProperty( "joystick" );
		joystickSizeFolder = serializedObject.FindProperty( "joystickSizeFolder" );
		highlightBase = serializedObject.FindProperty( "highlightBase" );
		highlightJoystick = serializedObject.FindProperty( "highlightJoystick" );
		tensionAccentUp = serializedObject.FindProperty( "tensionAccentUp" );
		tensionAccentDown = serializedObject.FindProperty( "tensionAccentDown" );
		tensionAccentLeft = serializedObject.FindProperty( "tensionAccentLeft" );
		tensionAccentRight = serializedObject.FindProperty( "tensionAccentRight" );
		joystickAnimator = serializedObject.FindProperty( "joystickAnimator" );
		joystickBase = serializedObject.FindProperty( "joystickBase" );
		
		/* Store our Size And Placement Variables */
		anchor = serializedObject.FindProperty( "anchor" );
		joystickTouchSize = serializedObject.FindProperty( "joystickTouchSize" );
		joystickSize = serializedObject.FindProperty( "joystickSize" );
		radiusModifier = serializedObject.FindProperty( "radiusModifier" );
		tbp_X = serializedObject.FindProperty( "tbp_X" );
		tbp_Y = serializedObject.FindProperty( "tbp_Y" );
		cs_X = serializedObject.FindProperty( "cs_X" );
		cs_Y = serializedObject.FindProperty( "cs_Y" );
		
		/* Style and Options */
		throwDuration = serializedObject.FindProperty( "throwDuration" );
		highlightColor = serializedObject.FindProperty( "highlightColor" );
		tensionColorNone = serializedObject.FindProperty( "tensionColorNone" );
		tensionColorFull = serializedObject.FindProperty( "tensionColorFull" );
		axis = serializedObject.FindProperty( "axis" );
		boundary = serializedObject.FindProperty( "boundary" );

		/* Touch Actions */
		tapCountOption = serializedObject.FindProperty( "tapCountOption" );
		tapCountDuration = serializedObject.FindProperty( "tapCountDuration" );
		targetTapCount = serializedObject.FindProperty( "targetTapCount" );
		tapCountEvent = serializedObject.FindProperty( "tapCountEvent" );

		fadeUntouched = serializedObject.FindProperty( "fadeUntouched" );
		fadeTouched = serializedObject.FindProperty( "fadeTouched" );

		// When we have selected this object, we want to constantly update our positioning
		UltimateJoystick uj = ( UltimateJoystick )target;
		uj.needToUpdatePositioning = true;
	}

	void OnDisable ()
	{
		// When we have deselected the Ultimate Joystick, stop updating
		UltimateJoystick uj = ( UltimateJoystick )target;
		uj.needToUpdatePositioning = false;
	}
	
	/*
	For more information on the OnInspectorGUI and adding your own variables
	in the UltimateJoystick.cs script and displaying them in this script,
	see the EditorGUILayout section in the Unity Documentation to help out.
	*/
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		// Store the joystick that we are selecting
		UltimateJoystick uj = ( UltimateJoystick )target;

		#region ERROR CHECKING
		/* ---------------------------------------- > ERROR CHECKING < ---------------------------------------- */
		if( canvasRect == null )
		{
			if( uj.transform.root.GetComponent<CanvasScaler>() )
				canvasRect = uj.transform.root.GetComponent<CanvasScaler>();
			else
				canvasRect = GetParentCanvas( uj );
		}
		if( canvasRect != null && canvasRect.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize )
		{
			EditorGUILayout.BeginVertical( "Button" );
			EditorGUILayout.HelpBox( "The Ultimate Joystick cannot be used correctly because the root canvas is using " + canvasRect.uiScaleMode.ToString() + ". Please place the Ultimate Joystick into a different Canvas with the ConstantPixelSize option.", MessageType.Error );
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button( "Adjust Canvas", GUILayout.Width( 100 ), GUILayout.Height( 20 ) ) )
			{
				Debug.Log( "CanvasScaler / ScaleMode option has been adjusted." );
				canvasRect.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
			}
			GUILayout.FlexibleSpace();
			if( GUILayout.Button( "Adjust Joystick", GUILayout.Width( 100 ), GUILayout.Height( 20 ) ) )// 75
			{
				bool canvasExists = false;
				Transform targetCanvas = uj.transform;
				CanvasScaler[] allCanvas = GameObject.FindObjectsOfType<CanvasScaler>();
				foreach( CanvasScaler currCanvas in allCanvas )
				{
					if( canvasExists == false )
					{
						if( currCanvas.uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize )
						{
							canvasExists = true;
							targetCanvas = currCanvas.transform;
						}
					}
				}
				// If we didn't find a Canvas with the right options, then we need to make one
				if( canvasExists == false )
				{
					// For full comments, please refer to CreateJoystickEditor.cs
					Debug.Log( "Ultimate UI Canvas has been created." );
					GameObject root = new GameObject( "Ultimate UI Canvas" );
					root.layer = LayerMask.NameToLayer( "UI" );
					Canvas canvas = root.AddComponent<Canvas>();
					canvas.renderMode = RenderMode.ScreenSpaceOverlay;
					root.AddComponent<CanvasScaler>();
					root.AddComponent<GraphicRaycaster>();
					Undo.RegisterCreatedObjectUndo( root, "Create " + root.name );
					targetCanvas = root.transform;
				}
				
				// Here we set the joystick to be a child of the canvas
				uj.transform.SetParent( targetCanvas.transform, false );
				
				// Focus on the moved Ultimate Joystick gameObject
				Selection.activeGameObject = uj.gameObject;
				
				// Tell the user
				Debug.Log( "Ultimate Joystick has been relocated to Ultimate UI Canvas." );
				
				canvasRect = null;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			return;
		}
		/* ---------------------------------------- > END ERROR CHECKING < ---------------------------------------- */
		#endregion

		EditorGUILayout.Space();
		
		#region ASSIGNED VARIABLES
		/* ---------------------------------------- > ASSIGNED VARIABLES < ---------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Assigned Variables", EditorStyles.boldLabel );
		string v_option = "Show";
		if( EditorPrefs.GetBool( "UUI_Variables" ) == true )
			v_option = "Hide";
		if( GUILayout.Button( v_option, EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )
		{
			if( EditorPrefs.GetBool( "UUI_Variables" ) == true )
				EditorPrefs.SetBool( "UUI_Variables", false );
			else
				EditorPrefs.SetBool( "UUI_Variables", true );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();

		if( EditorPrefs.GetBool( "UUI_Variables" ) == true )
		{
			EditorGUILayout.Space();
			EditorGUI.indentLevel = 1;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( joystick );
			EditorGUILayout.PropertyField( joystickSizeFolder, new GUIContent( "Size Folder" ) );
			EditorGUI.indentLevel = 0;
			if( uj.showHighlight == true )
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Highlight Variables", EditorStyles.boldLabel );
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField( highlightBase );
				EditorGUILayout.PropertyField( highlightJoystick );
				EditorGUI.indentLevel = 0;
			}
			if( uj.showTension == true )
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Tension Variables", EditorStyles.boldLabel );
				EditorGUI.indentLevel = 1;
				EditorGUILayout.PropertyField( tensionAccentUp, new GUIContent( "Tension Up" ) );
				EditorGUILayout.PropertyField( tensionAccentDown, new GUIContent( "Tension Down" ) );
				EditorGUILayout.PropertyField( tensionAccentLeft, new GUIContent( "Tension Left" ) );
				EditorGUILayout.PropertyField( tensionAccentRight, new GUIContent( "Tension Right" ) );
				EditorGUI.indentLevel = 0;
			}
			if( uj.useAnimation || uj.useFade == true )
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField( "Touch Action Variables", EditorStyles.boldLabel );
				EditorGUI.indentLevel = 1;
				if( uj.useAnimation == true )
					EditorGUILayout.PropertyField( joystickAnimator );
				if( uj.useFade == true )
					EditorGUILayout.PropertyField( joystickBase );
				EditorGUI.indentLevel = 0;
			}
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		/* ---------------------------------------- > END ASSIGNED VARIABLES < ---------------------------------------- */
		#endregion

		EditorGUILayout.Space();
		
		#region SIZE AND PLACEMENT
		/* ---------------------------------------- > SIZE AND PLACEMENT < ---------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Size and Placement", EditorStyles.boldLabel );
		string sap_option = "Show";
		if( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) == true )
			sap_option = "Hide";
		if( GUILayout.Button( sap_option, EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )//
		{
			if( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) == true )
				EditorPrefs.SetBool( "UUI_SizeAndPlacement", false );
			else
				EditorPrefs.SetBool( "UUI_SizeAndPlacement", true );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();

		if( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) == true )
		{
			EditorGUILayout.Space();
			// Arrange our button variables to be shown the way we want
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( anchor, new GUIContent( "Anchor", "The side of the screen that the\njoystick will be anchored to" ) );
			EditorGUILayout.PropertyField( joystickTouchSize, new GUIContent( "Touch Size", "The size of the area in which\nthe touch can be initiated" ) );
			EditorGUILayout.Slider( joystickSize, 1.0f, 4.0f, new GUIContent( "Joystick Size", "The overall size of the joystick" ) );
			EditorGUILayout.Slider( radiusModifier, 2.0f, 7.0f, new GUIContent( "Radius", "Determines how far the joystick can\nmove visually from the center" ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			EditorGUI.BeginChangeCheck();
			uj.touchBasedPositioning = EditorGUILayout.ToggleLeft( new GUIContent( "Touch Based Positioning", "Moves the joystick to the position\nof the initial touch" ), uj.touchBasedPositioning );
			if( EditorGUI.EndChangeCheck() )
				EditorUtility.SetDirty( target );

			if( uj.touchBasedPositioning == true )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				uj.overrideTouchSize = EditorGUILayout.Toggle( new GUIContent( "Override Size", "Allows a large section of the screen\nto be used for input" ), uj.overrideTouchSize, EditorStyles.radioButton );
				if( EditorGUI.EndChangeCheck() )
					EditorUtility.SetDirty( target );

				if( uj.overrideTouchSize == true )
				{
					EditorGUI.indentLevel = 2;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.Slider( tbp_X, 0.0f, 100.0f, new GUIContent( "Width", "The width of the Joystick Touch Area" ) );
					EditorGUILayout.Slider( tbp_Y, 0.0f, 100.0f, new GUIContent( "Height", "The height of the Joystick Touch Area" ) );
					if( EditorGUI.EndChangeCheck() )
						serializedObject.ApplyModifiedProperties();
				}
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			else
			{
				if( uj.overrideTouchSize == true )
					uj.overrideTouchSize = false;
			}

			EditorGUILayout.BeginVertical( "Box" );
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( new GUIContent( "Joystick Positioning", "Customize the position of the joystick" ) );
			string cs_option = "+";
			if( EditorPrefs.GetBool( "UUI_CustomSpacing" ) == true )
				cs_option = "-";
			if( GUILayout.Button( cs_option, GUILayout.Width( 35 ), GUILayout.Height( 14f ) ) )
			{
				if( EditorPrefs.GetBool( "UUI_CustomSpacing" ) == true )
					EditorPrefs.SetBool( "UUI_CustomSpacing", false );
				else
					EditorPrefs.SetBool( "UUI_CustomSpacing", true );
			}
			GUILayout.EndHorizontal();
			if( EditorPrefs.GetBool( "UUI_CustomSpacing" ) == true )
			{
				EditorGUI.indentLevel = 1;
				EditorGUILayout.Space();
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( cs_X, 0.0f, 50.0f, new GUIContent( "X Position:" ) );
				EditorGUILayout.Slider( cs_Y, 0.0f, 100.0f, new GUIContent( "Y Position:" ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();

				EditorGUILayout.Space();
				EditorGUI.indentLevel = 0;
			}
			EditorGUILayout.EndVertical();
		}
		/* ---------------------------------------- > END SIZE AND PLACEMENT < ---------------------------------------- */
		#endregion

		EditorGUILayout.Space();

		#region STYLE AND OPTIONS
		/* ---------------------------------------- > STYLE AND OPTIONS < ---------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Style and Options", EditorStyles.boldLabel );
		string sao_option = "Show";
		if( EditorPrefs.GetBool( "UUI_StyleAndOptions" ) == true )
			sao_option = "Hide";
		if( GUILayout.Button( sao_option, EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )//
		{
			if( EditorPrefs.GetBool( "UUI_StyleAndOptions" ) == true )
				EditorPrefs.SetBool( "UUI_StyleAndOptions", false );
			else
				EditorPrefs.SetBool( "UUI_StyleAndOptions", true );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();

		if( EditorPrefs.GetBool( "UUI_StyleAndOptions" ) == true )
		{
			EditorGUILayout.Space();
			// TouchPad
			EditorGUI.BeginChangeCheck();
			uj.touchPad = EditorGUILayout.ToggleLeft( new GUIContent( "Touch Pad", "Disables the visuals of the joystick" ), uj.touchPad );
			if( EditorGUI.EndChangeCheck() )
				EditorUtility.SetDirty( target );

			if( uj.touchPad == true )
			{
				if( uj.showHighlight == true )
					uj.showHighlight = false;
				if( uj.showTension == true )
					uj.showTension = false;
				if( uj.joystickBase.enabled == true )
					uj.joystickBase.enabled = false;
				if( uj.joystick.GetComponent<Image>().enabled == true )
					uj.joystick.GetComponent<Image>().enabled = false;

				// Set our HL and Tension
				uj.SetHighlight();
				uj.SetTensionAccent();
			}
			else
			{
				if( uj.joystickBase.enabled == false )
					uj.joystickBase.enabled = true;
				if( uj.joystick.GetComponent<Image>().enabled == false )
					uj.joystick.GetComponent<Image>().enabled = true;
			}

			// Throwable Joystick
			EditorGUI.BeginChangeCheck();
			uj.throwable = EditorGUILayout.ToggleLeft( new GUIContent( "Throwable", "Smoothly transitions the joystick back to\ncenter when the input is released" ), uj.throwable );
			if( EditorGUI.EndChangeCheck() )
				EditorUtility.SetDirty( target );

			if( uj.throwable == true )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( throwDuration, 0.05f, 1.0f, new GUIContent( "Throw Duration", "Time in seconds to return to center" ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();

				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			if( uj.touchPad == false )
			{
				// Show Highlight
				EditorGUI.BeginChangeCheck();
				uj.showHighlight = EditorGUILayout.ToggleLeft( new GUIContent( "Show Highlight", "Shows custom highlight color on the\njoystick" ), uj.showHighlight );
				if( EditorGUI.EndChangeCheck() )
				{
					EditorUtility.SetDirty( target );
					uj.SetHighlight();
				}

				if( uj.showHighlight == true )
				{
					// Highlight Options
					EditorGUI.indentLevel = 1;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField( highlightColor );
					if( EditorGUI.EndChangeCheck() )
						serializedObject.ApplyModifiedProperties();

					EditorGUI.indentLevel = 0;

					uj.SetHighlight();

					// If any of the variables are unassigned, we want to tell them
					if( uj.highlightBase == null && uj.highlightJoystick == null )
						EditorGUILayout.HelpBox( "Base and Joystick Highlight will not be displayed.", MessageType.Warning );
					else if( uj.highlightBase == null && uj.highlightJoystick != null )
						EditorGUILayout.HelpBox( "Base Highlight will not be displayed", MessageType.Warning );
					else if( uj.highlightBase != null && uj.highlightJoystick == null )
						EditorGUILayout.HelpBox( "Joystick Highlight will not be displayed", MessageType.Warning );

					EditorGUILayout.Space();
				}

				// Show Tension
				EditorGUI.BeginChangeCheck();
				uj.showTension = EditorGUILayout.ToggleLeft( new GUIContent( "Show Tension", "Displays the distance and direction of the\njoystick visually with custom colors" ), uj.showTension );
				if( EditorGUI.EndChangeCheck() )
				{
					EditorUtility.SetDirty( target );
					uj.SetTensionAccent();
				}

				if( uj.showTension == true )
				{
					// Tension Options and Variables
					EditorGUI.indentLevel = 1;
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField( tensionColorNone, new GUIContent( "Tension None", "The color displayed when the joystick\nis not being touched" ) );
					EditorGUILayout.PropertyField( tensionColorFull, new GUIContent( "Tension Full", "The color displayed when the joystick\nis at the furthest distance" ) );
					if( EditorGUI.EndChangeCheck() )
						serializedObject.ApplyModifiedProperties();

					EditorGUI.indentLevel = 0;
					EditorGUILayout.Space();

					uj.SetTensionAccent();
				}
			}

			// This if for using constraints and boundries
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( axis, new GUIContent( "Axis", "Contrains the joystick to a certain axis" ) );
			EditorGUILayout.PropertyField( boundary, new GUIContent( "Boundry", "Determines how the joystick's position is clamped" ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		/* ---------------------------------------- > END STYLE AND OPTIONS < ---------------------------------------- */
		#endregion
		
		EditorGUILayout.Space();
		
		#region TOUCH ACTIONS
		/* ---------------------------------------- > TOUCH ACTIONS < ---------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Touch Actions", EditorStyles.boldLabel );
		string ta_option = "Show";
		if( EditorPrefs.GetBool( "UUI_TouchActions" ) == true )
			ta_option = "Hide";
		if( GUILayout.Button( ta_option, EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )//
		{
			if( EditorPrefs.GetBool( "UUI_TouchActions" ) == true )
				EditorPrefs.SetBool( "UUI_TouchActions", false );
			else
				EditorPrefs.SetBool( "UUI_TouchActions", true );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();

		if( EditorPrefs.GetBool( "UUI_TouchActions" ) == true )
		{
			EditorGUILayout.Space();
			// This is for calculating our taps within a time window
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( tapCountOption, new GUIContent( "Tap Count", "Allows the joystick to calculate double taps and a touch and release within a certain time window" ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			if( uj.tapCountOption != UltimateJoystick.TapCountOption.NoCount )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( tapCountEvent );
				EditorGUILayout.Slider( tapCountDuration, 0.0f, 1.0f, new GUIContent( "Tap Time Window", "Time in seconds that the joystick can recieve taps" ) );
				if( uj.tapCountOption == UltimateJoystick.TapCountOption.Accumulate )
					EditorGUILayout.IntSlider( targetTapCount, 1, 5, new GUIContent( "Target Tap Count", "How many taps to activate the Tap Count Event?" ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();

				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}

			// This is for implementing our touch actions with animations
			EditorGUI.BeginChangeCheck();
			uj.useAnimation = EditorGUILayout.ToggleLeft( new GUIContent( "Use Animation", "Play animation in reaction to input" ), uj.useAnimation );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				uj.SetAnimation();
			}
			if( uj.useAnimation == true )
			{
				EditorGUI.indentLevel = 1;

				if( uj.joystickAnimator == null )
					EditorGUILayout.HelpBox( "Joystick Animator needs to be assigned.", MessageType.Error );

				EditorGUI.indentLevel = 0;
			}
			// This is for implementing color fading with touch
			EditorGUI.BeginChangeCheck();
			uj.useFade = EditorGUILayout.ToggleLeft( new GUIContent( "Use Fade", "Fade joystick visuals when touched,\nand released?" ), uj.useFade );
			if( EditorGUI.EndChangeCheck() )
			{
				EditorUtility.SetDirty( target );
				if( uj.useFade == true )
					uj.HandleFade( "Untouched" );
				else
					uj.HandleFade( "Reset" );
			}
			if( uj.useFade == true )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( fadeUntouched, 0.0f, 1.0f, new GUIContent( "Fade Untouched", "The alpha of the joystick when it is NOT receiving input" ) );
				EditorGUILayout.Slider( fadeTouched, 0.0f, 1.0f, new GUIContent( "Fade Touched", "The alpha of the joystick when receiving input" ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();

				uj.HandleFade( "Untouched" );

				if( uj.showTension == true )
					EditorGUILayout.HelpBox( "The alpha of Tension Color will not fade. If you want to change the alpha of the Tension Color, modify it with the Tension Color property directly.", MessageType.Warning );
				EditorGUI.indentLevel = 0;
			}
		}
		/* ---------------------------------------- > END TOUCH ACTIONS < ---------------------------------------- */
		#endregion
		
		EditorGUILayout.Space();

		#region RESETS
		/* ---------------------------------------- > RESETS < ---------------------------------------- */
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( "Restore To Default", EditorStyles.boldLabel );
		string rtd_option = "Show";
		if( EditorPrefs.GetBool( "UUI_RestoreToDefault" ) == true )
			rtd_option = "Hide";
		if( GUILayout.Button( rtd_option, EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )
		{
			if( EditorPrefs.GetBool( "UUI_RestoreToDefault" ) == true )
				EditorPrefs.SetBool( "UUI_RestoreToDefault", false );
			else
				EditorPrefs.SetBool( "UUI_RestoreToDefault", true );
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		if( EditorPrefs.GetBool( "UUI_RestoreToDefault" ) == true )
		{
			// In this section, we just are setting up hard coded values to be able to reset our options to
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button( "Size and Placement", EditorStyles.miniButton, GUILayout.Width( 120 ), GUILayout.Height( 20 ) ) )
				if( EditorUtility.DisplayDialog( "Warning!", "Are you sure you reset the Size and Placement options back to default?", "Yes", "No" ) )
					ResetSizeAndPlacement( uj );
			if( GUILayout.Button( "Style and Options", EditorStyles.miniButton, GUILayout.Width( 120 ), GUILayout.Height( 20 ) ) )
				if( EditorUtility.DisplayDialog( "Warning!", "Are you sure you reset the Style and Options options back to default?", "Yes", "No" ) )
					ResetStyleAndOptions( uj );
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button( "Touch Actions", EditorStyles.miniButton, GUILayout.Width( 120 ), GUILayout.Height( 20 ) ) )
				if( EditorUtility.DisplayDialog( "Warning!", "Are you sure you reset the Touch Actions options back to default?", "Yes", "No" ) )
					ResetTouchActions( uj );
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		/* ---------------------------------------- > END RESETS < ---------------------------------------- */
		#endregion

		EditorGUILayout.Space();
		
		// This is for showing helpful tips to help them avoid errors
		if( uj.joystick == null )
			EditorGUILayout.HelpBox( "Joystick needs to be assigned in 'Assigned Variables'!", MessageType.Error );
		if( uj.joystickSizeFolder == null )
			EditorGUILayout.HelpBox( "Joystick Size Folder needs to be assigned in 'Assigned Variables'!", MessageType.Error );
	}
	
	#region RESET FUNCTIONS
	void ResetSizeAndPlacement ( UltimateJoystick uj )
	{
		uj.joystickTouchSize = UltimateJoystick.JoystickTouchSize.Default;
		uj.joystickSize = 2.5f;
		uj.radiusModifier = 4.5f;
		uj.touchBasedPositioning = false;
		uj.overrideTouchSize = false;
		uj.tbp_X = 50.0f;
		uj.tbp_Y = 75.0f;
		uj.cs_X = 5.0f;
		uj.cs_Y = 20.0f;
	}

	void ResetStyleAndOptions ( UltimateJoystick uj )
	{
		uj.touchPad = false;
		uj.throwable = true;
		uj.throwDuration = 0.5f;
		uj.showHighlight = true;
		uj.highlightColor = new Color( 0.118f, 0.992f, 0.0f, 1.0f );
		uj.SetHighlight();
		uj.showTension = true;
		uj.tensionColorNone = new Color( 0.118f, 0.992f, 0.0f, 0.0f );
		uj.tensionColorFull = new Color( 0.118f, 0.992f, 0.0f, 1.0f );
		uj.SetTensionAccent();
		uj.axis = UltimateJoystick.Axis.Both;
		uj.boundary = UltimateJoystick.Boundary.Circular;
	}

	void ResetTouchActions ( UltimateJoystick uj )
	{
		uj.tapCountOption = UltimateJoystick.TapCountOption.NoCount;
		uj.tapCountDuration = 0.5f;
		uj.targetTapCount = 2;
		uj.useAnimation = false;
		uj.SetAnimation();
		uj.useFade = false;
		uj.fadeUntouched = 1.0f;
		uj.fadeTouched = 0.5f;
		uj.HandleFade( "Reset" );
	}
	#endregion

	CanvasScaler GetParentCanvas ( UltimateJoystick joystick )
	{
		Transform parent = joystick.transform.parent;
		while( parent != null )
		{ 
			if( parent.transform.GetComponent<CanvasScaler>() )
				return parent.transform.GetComponent<CanvasScaler>();

			parent = parent.transform.parent;
		}
		return null;
	}
}
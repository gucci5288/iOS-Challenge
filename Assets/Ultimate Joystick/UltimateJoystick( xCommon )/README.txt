Thank you for purchasing the Ultimate Joystick UnityPackage!

This package comes with both C# and Javascript coding languages for all needed scripts,
so please feel free to use which ever coding language you are most comfortable with.

/* -----> IMPORTANT INFORMATION <----- */
Within Unity, please go to Window / Ultimate Joystick to access important information on how to get started
using the Ultimate Joystick. There are links in that window to an Extensive Online Readme, Complete Documentation,
and FREE example scripts to get you started as fast as possible!
/* ---> END IMPORTANT INFORMATION <--- */


/* IF YOU CAN'T VIEW THE ONLINE INFORMATION, READ THIS SECTION */
Please note that there is a C# AND Javascript version of the UltimateJoystick, UltimateJoystickEditor and the
CapsuleManController scripts. We also included C# and Javascript prefabs located in a folder named Resources.
Also, please note that we are using our own custom inspector, and these Editor scripts are located in the Ultimate
Joystick folder in a folder named "Editor". You must leave these scripts in this folder in order for the Editor
scripts to work correctly.

/* HOW TO REFERENCE THE ULTIMATE JOYSTICK */
One of the great things about the Ultimate Joystick is the easy reference from other scripts. In order to get the information
from our Ultimate Joystick, we need to have a variable to store that particular joystick.

/* ---> EXAMPLE <--- */
EXAMPLE C#: public UltimateJoystick joystick;
EXAMPLE Java: var joystick : UltimateJoystickJAVA;

Once we have these variables, all we need to do is get our JoystickPosition from that Joystick by storing it into a
Vector2 variable at runtime.

/* ---> EXAMPLE <--- */
EXAMPLE C#: Vector2 joystickPos = joystick.JoystickPosition;
EXAMPLE Java: var joystickPos : Vector2 = joystick.JoystickPosition;

After we have these values, we can apply that to anything we need. Please note that the values returned by the
JoystickPosition function will be a value between -1 and 1, with 0 being the exact center. The JoystickPosition
function will return an X and Y value for our Vector2.
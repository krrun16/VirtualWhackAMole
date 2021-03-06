using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JoyconHammer : MonoBehaviour
{

	private static List<Joycon> joycons;

	// Values made available via Unity
	public float[] stick;
	public Vector3 gyro;
	public Vector3 accel;
	public int jc_ind = 0;
	public Quaternion orientation;

	void Start()
	{
		gyro = new Vector3(0, 0, 0);
		accel = new Vector3(0, 0, 0);
		// get the public Joycon array attached to the JoyconManager in scene
		joycons = JoyconManager.Instance.j;
		if (joycons.Count < jc_ind + 1)
		{
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// make sure the Joycon only gets checked if attached

		if (joycons.Count > 0)
		{
			Joycon j = joycons[jc_ind];
				
			if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
			{
				Debug.Log("Rumble");

				// Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
				// https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

				j.SetRumble(160, 320, 0.6f, 200);

				// The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
				// (Useful for dynamically changing rumble values.)
				// Then call SetRumble(0,0,0) when you want to turn it off.
			}
		}
	}

	public static void rightVibrate()
    {
		if (joycons.Count > 0)
        {
			for (int i = 0; i < joycons.Count; i++)
            {
				if (joycons.ElementAt(i).isLeft)
                {
					continue;
                }

				if (!joycons.ElementAt(i).isLeft)
                {
					Debug.Log("This ran.");
					joycons.ElementAt(i).SetRumble(160, 320, 0.6f, 200);
				}
			}
        }
    }

	public static void leftVibrate()
    {
		if (joycons.Count > 0)
		{
			for (int i = 0; i < joycons.Count; i++)
			{
				if (joycons.ElementAt(i).isLeft)
				{
					Debug.Log("This ran.");
					joycons.ElementAt(i).SetRumble(160, 320, 0.6f, 200);
				}

				if (!joycons.ElementAt(i).isLeft)
				{
					continue;
				}
			}
		}
	}

}


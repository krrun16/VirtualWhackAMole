using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using System;

public class HammerController : MonoBehaviour
{

    private Mole[] moles;
    private Rigidbody rb;

    public float speed = 20f;

    private Vector3 newLeftHammerPosition;
    private Vector3 newRightHammerPosition;
    public static Quaternion endRotation;

    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list 
        moles = GameObject.FindObjectsOfType<Mole>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float height =  GameObject.Find("GridManager").transform.localScale.y;
        transform.localScale = new Vector3(height, height, height);

        newLeftHammerPosition = new Vector3(-BodySourceView.leftHandPosition.X, BodySourceView.leftHandPosition.Y, BodySourceView.leftHandPosition.Z);
        newRightHammerPosition = new Vector3(-BodySourceView.rightHandPosition.X, BodySourceView.rightHandPosition.Y, BodySourceView.rightHandPosition.Z);
    
        //TODO: Fix hammer rotation
        if(rb.CompareTag("leftHammer")) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newLeftHammerPosition, Time.fixedDeltaTime * 13);
            RotateHammer(BodySourceView.leftHandPosition, BodySourceView.leftWristPosition, BodySourceView.leftElbowPosition);
        }
        else if (rb.CompareTag("rightHammer")) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newRightHammerPosition, Time.fixedDeltaTime * 13);
            RotateHammer(BodySourceView.rightHandPosition, BodySourceView.rightWristPosition, BodySourceView.rightElbowPosition);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Mole"))
        {
            foreach (Mole mole in moles)
            {
                if (mole.name == col.gameObject.name)
                {
                    mole.HitMole();
                }
            }
        }


        // Hit vibration using tag of 
        if (gameObject.CompareTag("rightHammer"))
        {
            JoyconHammer.rightVibrate();
        }
        else
        {
            JoyconHammer.leftVibrate();
        }
    }


    /// <summary>
    /// Calculates the rotation of the bat in the virtual world
    /// </summary>
    /// <param name="handBasePos">Distance of the base of the hand from the Kinect</param>
    /// <param name="handTipPos">Distance of the tip of the hand from the Kinect</param>
    private void RotateHammer(CameraSpacePoint hand, CameraSpacePoint elbow, CameraSpacePoint shoulder)
    {
        Vector3 vector1;
        vector1.x = elbow.X - shoulder.X;
        vector1.y = elbow.Y - shoulder.Y;
        vector1.z = elbow.Z - shoulder.Z;

        Vector3 vector2;
        vector2.x = hand.X - elbow.X;
        vector2.y = hand.Y - elbow.Y;
        vector2.z = hand.Z - elbow.Z;
        float length1 = (float)Math.Sqrt((vector1.x * vector1.x) + (vector1.y * vector1.y) + (vector1.z * vector1.z));
        float length2 = (float)Math.Sqrt((vector2.x * vector2.x) + (vector2.y * vector2.y) + (vector2.z * vector2.z));

        float dot = (vector1.x * vector2.x) + (vector1.y * vector2.y) + (vector1.z * vector2.z);
        float angle = (float)Math.Acos(dot / (length1 * length2)); // Radians
        angle *= 180.0f / (float)Math.PI; // Degrees

        // one implementation 
        /* if (angle > 1 || angle <-1)
         {
             endRotation = Quaternion.Euler(-angle, 0, 0);
             transform.localRotation = Quaternion.Slerp(transform.rotation, endRotation, .05f);
         }*/

        //second implementation
        if (-5 <= angle && angle < 20)
        {
            endRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (angle >= 20 && angle < 25)
        {
            endRotation = Quaternion.Euler(-15, 0, 0);
        }
        else if (angle >= 25 && angle < 30)
        {
            endRotation = Quaternion.Euler(-30, 0, 0);
        }
        else if (angle >= 30)
        {
            endRotation = Quaternion.Euler(-45, 0, 0);
        }
        else if (angle < -5)
        {
            endRotation = Quaternion.Euler(0, 0, 0);
        }
        transform.localRotation = Quaternion.Slerp(transform.rotation, endRotation, .05f);
    }


}

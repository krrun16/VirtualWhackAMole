﻿using UnityEngine;
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

    float GridEdge = 0;
    float CenterX = 0;

    private Vector3 newLeftHammerPosition;
    private Vector3 newRightHammerPosition;
    private Vector3 newRotation2;
    public static Quaternion newRotation;

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
        CameraSpacePoint leftHandPosition = BodySourceView.leftHandPosition;
        CameraSpacePoint rightHandPosition = BodySourceView.rightHandPosition;
        CameraSpacePoint midSpinePosition = BodySourceView.baseKinectPosition;

        newLeftHammerPosition = new Vector3(-BodySourceView.leftHandPosition.X, BodySourceView.leftHandPosition.Y, BodySourceView.leftHandPosition.Z);
        newRightHammerPosition = new Vector3(-BodySourceView.rightHandPosition.X, BodySourceView.rightHandPosition.Y, BodySourceView.rightHandPosition.Z);
    
        //TODO: Fix hammer rotation
        if(rb.CompareTag("leftHammer")) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newLeftHammerPosition, Time.fixedDeltaTime * 13);
            //RotateHammer(BodySourceView.leftHandPosition, BodySourceView.leftWristPosition, BodySourceView.leftElbowPosition);
        }
        else if (rb.CompareTag("rightHammer")) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newRightHammerPosition, Time.fixedDeltaTime * 13);
            //RotateHammer(BodySourceView.rightHandPosition, BodySourceView.rightWristPosition, BodySourceView.rightElbowPosition);
        }
        

        //CameraSpacePoint rightHandPosition = BodySourceView.rightHandPosition;
        /*
        float centerXPoint = CheckCalibratedX(midSpinePosition.X);
        float maxZPoint = CheckCalibratedZ(midSpinePosition.Z);

        //Calculate the position of the paddle based on the distance from the mid spine join
        if (rb.CompareTag("leftHammer"))
        {
            print("entered left hammer " + rb.tag);
            float xPos = (centerXPoint - leftHandPosition.X) * 11,
                  zPos = (leftHandPosition.Z - 1) * 10,
                  yPos = leftHandPosition.Y * 11;
            Vector3 newPosition = new Vector3(xPos, yPos, zPos);
            rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.fixedDeltaTime * 13));
            //RotateHammer(BodySourceView.leftHandPosition, BodySourceView.leftElbowPosition, BodySourceView.leftShoulderPosition);
            RotateHammer(BodySourceView.leftHandPosition, BodySourceView.leftWristPosition, BodySourceView.leftElbowPosition);
        }
        else if (rb.CompareTag("rightHammer"))
        {
            //print("entered right hammer " + rb.tag);
            float xPos = (centerXPoint - rightHandPosition.X) * 11,
                  zPos = (rightHandPosition.Z - 1) * 10,
                  yPos = rightHandPosition.Y * 11;
            Vector3 newPosition = new Vector3(xPos, yPos, zPos);
            rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.fixedDeltaTime * 13));
            //RotateHammer(BodySourceView.rightHandPosition, BodySourceView.rightElbowPosition, BodySourceView.rightShoulderPosition);
            RotateHammer(BodySourceView.rightHandPosition, BodySourceView.rightWristPosition, BodySourceView.rightElbowPosition);
        }
        */
    }
    private float CheckCalibratedX(float xPos)
    {
        return CenterX != 0 ? CenterX : xPos;
    }

    private float CheckCalibratedZ(float zPos)
    {
        return GridEdge != 0 ? GridEdge : zPos;
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

        Quaternion newRotation = Quaternion.AngleAxis(0, Vector3.back);

        newRotation2.y = transform.rotation.eulerAngles.y;
        newRotation2.z = transform.rotation.eulerAngles.z;
        newRotation2.x = -angle;

        newRotation = Quaternion.Euler(newRotation2);
        rb.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed * Time.deltaTime);
        //rb.MoveRotation(Quaternion.Euler(-angle, 0, 0));
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class hipPlane : MonoBehaviour
{

    Vector3 endPosition;
    Vector3 inPosition;
    Vector3 centerFoot;
    Vector3 side1;
    Vector3 side2;
    float yRotation;
    float zRotation;
    private Vector3 endRotation2;
    private float speed;
    public static Quaternion endRotation;

    void Start()
    {
        inPosition = transform.localPosition;
        endPosition = inPosition;
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        CameraSpacePoint leftHip = BodySourceView.leftHipPosition;
        CameraSpacePoint rightHip = BodySourceView.rightHipPosition;
        CameraSpacePoint leftFoot = BodySourceView.leftFootPosition;
        CameraSpacePoint rightFoot = BodySourceView.rightFootPosition;


        //calculating point between two feet
        float centerFeetX = (leftFoot.X + rightFoot.X)/ 2;
        float centerFeetY = (leftFoot.Y + rightFoot.Y) / 2;
        float centerFeetZ = (leftFoot.Z + rightFoot.Z) / 2;

        // setting these points to a vector, centerFoot
        centerFoot.x = centerFeetX;
        centerFoot.y = centerFeetY;
        centerFoot.z = centerFeetZ;

        side1.x = centerFoot.x - leftHip.X;
        side1.y = centerFoot.y - leftHip.Y;
        side1.z = centerFoot.z - leftHip.Z;
        side2.x = centerFoot.x - rightHip.X;
        side2.y = centerFoot.y - rightHip.Y;
        side2.z = centerFoot.z - rightHip.Z;

        Vector3 normalVector = Vector3.Cross(side1, side2);

        //Calculate the position of the board using centroid of leftthip, right hip, and centerFeet
        float xPos = (leftHip.X + rightHip.X + centerFeetX)/3;
        float yPos = (leftHip.Y + rightHip.Y + centerFeetY)/3;
        float zPos = (leftHip.Z + rightHip.Z + centerFeetZ)/3;
        endPosition.x = -xPos;
        endPosition.y = GameObject.Find("Main Camera").transform.position.y - GameObject.Find("GridManager").transform.localScale.y;
        //endPosition.z = inPosition.z;
        endPosition.z = zPos;
        
        //endPosition = endPosition * 10f;
        transform.localPosition = Vector3.Lerp(transform.localPosition, endPosition, Time.deltaTime * speed);

        //find rotation of 
        endRotation = Quaternion.LookRotation(normalVector, Vector3.up);
        yRotation = endRotation.eulerAngles.y - 90;
        zRotation = endRotation.eulerAngles.z + 90;
        //endRotation2.x = endRotation.eulerAngles.x;
        endRotation2.y = -yRotation;
        endRotation2.z = zRotation;
        endRotation = Quaternion.Euler(endRotation2);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, endRotation, speed * Time.deltaTime);
    }


}

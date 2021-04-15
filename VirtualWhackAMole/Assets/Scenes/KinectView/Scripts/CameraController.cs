using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect.Face;



public class CameraController : MonoBehaviour
{
    private Vector3 startHeadPosition;
    private Vector3 newHeadPosition;
    private Vector3 endRotation2;
    float yRotation;
    public float smooth = 10F;
    public float speed = 5.0F;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO: subtract the grid position from the headPosition so the grid is always the origin
        //newHeadPosition = new Vector3(-BodySourceView.headPosition.X - .62F, BodySourceView.headPosition.Y, BodySourceView.headPosition.Z - 1.48F);
        newHeadPosition = new Vector3(-BodySourceView.headPosition.X, BodySourceView.headPosition.Y, BodySourceView.headPosition.Z + 0.4f);
        //newHeadPosition *= 10f;
        transform.position = Vector3.Lerp(transform.position, newHeadPosition, Time.deltaTime * 4f);
    }

    void Update()
    {
        //Rotate camera to match face rotation
        Quaternion fr = BodySourceView.faceRotation;

        yRotation = fr.eulerAngles.y + 180;
        endRotation2.x = fr.eulerAngles.x;
        endRotation2.y = yRotation;
        endRotation2.z = fr.eulerAngles.z;

        Quaternion endRotation = Quaternion.Euler(-endRotation2);
        transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, speed*Time.deltaTime);
    }
}

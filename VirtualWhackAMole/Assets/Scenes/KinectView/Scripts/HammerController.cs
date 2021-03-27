using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class HammerController : MonoBehaviour
{
    private Mole[] moles;
    private Rigidbody rb;

    public float speed = 20f;
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
        {
            /*
            CameraSpacePoint midSpinePosition = BodySourceView.baseKinectPosition;
            CameraSpacePoint leftHandPosition = BodySourceView.leftHandPosition;
            CameraSpacePoint rightHandPosition = BodySourceView.rightHandPosition;

            float centerXPoint = CheckCalibratedX(midSpinePosition.X);
            float maxZPoint = CheckCalibratedZ(midSpinePosition.Z);

            //Calculate the position of the paddle based on the distance from the mid spine join
            float xPos = (centerXPoint - leftHandPosition.X) * 100,
                  zPos = (maxZPoint - (-leftHandPosition.Z)) * 100,
                  yPos = transform.position.y;

            //Smoothing applied to slow down bat so it doesn't phase through ball
            Vector3 newPosition = new Vector3(-xPos, yPos, zPos);
            //Smooting factor of fixedDeltaTime*20 is to keep the paddle from moving so quickly that is
            //phases through the ball on collision.
            rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.fixedDeltaTime * 13));



            RotateHammer(BodySourceView.leftWristPosition, BodySourceView.leftHandPosition);
            RotateHammer(BodySourceView.rightWristPosition, BodySourceView.rightHandPosition);
            */

        }
    }
    /*
    private float CheckCalibratedX(float xPos)
    {
        return xPos;
    }

    private float CheckCalibratedZ(float zPos)
    {
        return zPos;
    }*/
        private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Mole"))
        {
            foreach (Mole mole in moles) {
                if (mole.name == col.gameObject.name)
                {
                    mole.HideMole();
                } 
            }
            
        }
    }
/*
/// <summary>
/// Calculates the rotation of the bat in the virtual world
/// </summary>
/// <param name="handBasePos">Distance of the base of the hand from the Kinect</param>
/// <param name="handTipPos">Distance of the tip of the hand from the Kinect</param>
private void RotateHammer(CameraSpacePoint handBasePos, CameraSpacePoint handTipPos)
{
    print("hello sarah");
    float o = (-handBasePos.Z) - (-handTipPos.Z),
          a = handBasePos.X - handTipPos.X,
          angle = Mathf.Rad2Deg * Mathf.Atan2(o, a);

    Quaternion newRotation = Quaternion.AngleAxis(0, Vector3.up);

    if (-35 <= angle && angle < 35)
    {
        newRotation = Quaternion.AngleAxis(0, Vector3.up);
    }
    else if (angle >= 35 && angle < 90)
    {
        newRotation = Quaternion.AngleAxis(45, Vector3.up);
    }
    else if (angle >= 90 && angle < 135)
    {
        newRotation = Quaternion.AngleAxis(135, Vector3.up);
    }
    else if (angle >= 135)
    {
        newRotation = Quaternion.AngleAxis(180, Vector3.up);
    }
    else if (angle < -35)
    {
        newRotation = Quaternion.AngleAxis(-45, Vector3.up);
    }
    rb.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);

    //No snapping or smoothing
    rb.MoveRotation(Quaternion.Euler(0, angle, 0));
    } */
}

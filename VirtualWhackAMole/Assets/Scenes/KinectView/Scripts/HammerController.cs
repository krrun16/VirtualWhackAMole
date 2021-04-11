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

    float GridEdge = 0;
    float CenterX = 0;

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
        CameraSpacePoint leftWristPosition = BodySourceView.leftWristPosition;
        CameraSpacePoint rightHandPosition = BodySourceView.rightHandPosition;
        CameraSpacePoint rightWristPosition = BodySourceView.rightWristPosition;
        CameraSpacePoint midSpinePosition = BodySourceView.baseKinectPosition;
        //CameraSpacePoint rightHandPosition = BodySourceView.rightHandPosition;

        float centerXPoint = CheckCalibratedX(midSpinePosition.X);
        float maxZPoint = CheckCalibratedZ(midSpinePosition.Z);

        //Calculate the position of the paddle based on the distance from the mid spine join
        if (rb.CompareTag("leftHammer")) {
            print("entered left hammer " + rb.tag);
            float xPos = (centerXPoint-leftHandPosition.X) *11,
                  zPos = (leftHandPosition.Z-1) *10,
                  yPos = leftHandPosition.Y*11;
            Vector3 newPosition = new Vector3(xPos, yPos, zPos);
            rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.fixedDeltaTime * 13));
        } else if (rb.CompareTag("rightHammer"))
        {
            print("entered right hammer " + rb.tag);
            float xPos = (centerXPoint - rightHandPosition.X) * 11,
                  zPos = (rightHandPosition.Z - 1) * 10,
                  yPos = rightHandPosition.Y * 11;
            Vector3 newPosition = new Vector3(xPos, yPos, zPos);
            rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.fixedDeltaTime * 13));
        }
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
            foreach (Mole mole in moles) {
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
    private void RotateHammer(CameraSpacePoint handBasePos, CameraSpacePoint handTipPos)
    {
        float o = handBasePos.Z - handTipPos.Z,
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
        //rb.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);

        //No snapping or smoothing
        //rb.MoveRotation(Quaternion.Euler(0, angle, 0));
    }

}

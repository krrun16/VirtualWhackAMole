using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodySourceView : MonoBehaviour 
{
    public BodySourceManager mBodySourceManager;
    public GameObject mHandObject;

    //public static Kinect.CameraSpacePoint leftHandPosition;
    //public static Kinect.CameraSpacePoint rightHandPosition;

    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType>
    {
        JointType.HandLeft,
        JointType.HandRight,
    };

    void Update()
    {
        #region Get Kinect data
        Body[] data = mBodySourceManager.GetData();
        if (data == null)
        {
            return;
        }
        #endregion

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        #region Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach (ulong trackingId in knownIds)
        {
            Destroy(mBodies[trackingId]);

            mBodies.Remove(trackingId);
        }
        #endregion

        #region Create Kinect bodies
        foreach (var body in data)
        {
            //if no body, skip
            if (body == null)
                continue;
            if (body.IsTracked)
            {
                if (!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                UpdateBodyObject(body, mBodies[body.TrackingId]);
            }
        }
        #endregion
    }


    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        foreach (JointType joint in _joints)
        {
            //Create object
            GameObject newJoint = Instantiate(mHandObject);
            newJoint.name = joint.ToString();
            newJoint.transform.parent = body.transform;
        }
        return body;
    }

    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        foreach (JointType _joint in _joints)
        {
            // Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            //targetPosition.z = 0;

            //Get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;

           // leftHandPosition = body.Joints[Kinect.JointType.HandTipLeft].Position;
            //rightHandPosition = body.Joints[Kinect.JointType.HandTipRight].Position;
        }
    }


    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}

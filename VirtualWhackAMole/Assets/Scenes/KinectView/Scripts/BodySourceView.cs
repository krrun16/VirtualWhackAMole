using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Microsoft.Kinect.Face;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using System;

public class BodySourceView : MonoBehaviour 
{
    public BodySourceManager mBodySourceManager;

    public static CameraSpacePoint headPosition;
    public static CameraSpacePoint leftElbowPosition;
    public static CameraSpacePoint rightElbowPosition;
    public static Quaternion faceRotation;
    public GameObject leftHandObject;
    public GameObject rightHandObject;
    public GameObject myplane;


    public static CameraSpacePoint leftHandPosition;
    public static CameraSpacePoint rightHandPosition;
    public static CameraSpacePoint leftWristPosition;
    public static CameraSpacePoint rightWristPosition;
    public static CameraSpacePoint leftShoulderPosition;
    public static CameraSpacePoint rightShoulderPosition;

    // storing these values to make sure ankles are positioned correctly on player
    // check is carried out in GameController
    public static CameraSpacePoint leftAnklePosition;
    public static CameraSpacePoint rightAnklePosition;



    public static CameraSpacePoint leftHipPosition;
    public static CameraSpacePoint rightHipPosition;
    public static CameraSpacePoint leftFootPosition;
    public static CameraSpacePoint rightFootPosition;

    public static CameraSpacePoint leftHipRotation;

    public static CameraSpacePoint baseKinectPosition;
    public static CameraSpacePoint closestZPoint;
    public static float MaxZDistance;

    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType>
    {
        JointType.HandLeft,
        JointType.HandRight,
        JointType.Head,
        JointType.SpineBase,
    };

    private List<JointType> _planejoints = new List<JointType>
    {
        JointType.HipLeft,
        JointType.HipRight,
        JointType.FootLeft,
        JointType.FootRight,
    };

    private static float hipToHeadHeight;
    private static float footToHeadHeight;

    private void Start()
    {
    }
    
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

        FaceFrameResult[] faceData = mBodySourceManager.GetFaceData();

        if (faceData[0] != null)
        {
            faceRotation = new Quaternion(faceData[0].FaceRotationQuaternion.X, faceData[0].FaceRotationQuaternion.Y, faceData[0].FaceRotationQuaternion.Z, faceData[0].FaceRotationQuaternion.W);
        }
        
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        return body;
    }

    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        leftHandPosition = body.Joints[JointType.HandTipLeft].Position;
        leftWristPosition = body.Joints[JointType.HandLeft].Position;
        rightHandPosition = body.Joints[JointType.HandTipRight].Position;
        rightWristPosition = body.Joints[JointType.HandRight].Position;

        leftHipPosition = body.Joints[JointType.HipLeft].Position;
        rightHipPosition = body.Joints[JointType.HipRight].Position;
        leftFootPosition = body.Joints[JointType.FootLeft].Position;
        rightFootPosition = body.Joints[JointType.FootRight].Position;

        hipToHeadHeight = body.Joints[JointType.Head].Position.Y - body.Joints[JointType.HipLeft].Position.Y;
        footToHeadHeight = body.Joints[JointType.Head].Position.Y - body.Joints[JointType.FootLeft].Position.Y;

        headPosition = body.Joints[JointType.Head].Position;
        leftElbowPosition = body.Joints[JointType.ElbowLeft].Position;
        rightElbowPosition = body.Joints[JointType.ElbowRight].Position;
        leftShoulderPosition = body.Joints[JointType.ShoulderLeft].Position;
        rightShoulderPosition = body.Joints[JointType.ShoulderRight].Position;


        leftAnklePosition = body.Joints[JointType.AnkleLeft].Position;
        rightAnklePosition = body.Joints[JointType.AnkleRight].Position;


        MaxZDistance =
            Math.Max(-body.Joints[JointType.Head].Position.Z,
            Math.Max(-body.Joints[JointType.Head].Position.Z,
            Math.Max(-body.Joints[JointType.Neck].Position.Z,
            Math.Max(-body.Joints[JointType.SpineMid].Position.Z,
            Math.Max(-body.Joints[JointType.SpineShoulder].Position.Z,
            Math.Max(-body.Joints[JointType.HipLeft].Position.Z,
                -body.Joints[JointType.HipRight].Position.Z))))));
        /*
        //float minZBodyDist =
        //   Math.Min(body.Joints[JointType.Head].Position.Z,
        //   Math.Min(body.Joints[JointType.Head].Position.Z,
        //   Math.Min(body.Joints[JointType.Neck].Position.Z,
        //   Math.Min(body.Joints[JointType.SpineMid].Position.Z,
        //   Math.Min(body.Joints[JointType.SpineShoulder].Position.Z,
        //   Math.Min(body.Joints[JointType.HipLeft].Position.Z,
        //       body.Joints[JointType.HipRight].Position.Z))))));

        float minZDistance =
            Math.Min(-body.Joints[JointType.Head].Position.Z,
            Math.Min(-body.Joints[JointType.Head].Position.Z,
            Math.Min(-body.Joints[JointType.Neck].Position.Z,
                -body.Joints[JointType.SpineShoulder].Position.Z)));
        */
        baseKinectPosition = new CameraSpacePoint()
        {
            X = body.Joints[JointType.SpineShoulder].Position.X,
            Y = body.Joints[JointType.Head].Position.Y,
            Z = MaxZDistance
        };
        /*
        closestZPoint = new CameraSpacePoint()
        {
            X = body.Joints[JointType.SpineMid].Position.X,
            Y = body.Joints[JointType.SpineMid].Position.Y,
            Z = minZDistance
        };
        */
    }


    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(-joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    public static float getHipToHeadHeight()
    {
        return hipToHeadHeight;
    }
    public static float getHeadToFootHeight()
    {
        return footToHeadHeight;
    }


}

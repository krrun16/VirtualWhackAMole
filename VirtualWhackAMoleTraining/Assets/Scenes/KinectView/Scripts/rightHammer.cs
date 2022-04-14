using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using System;
using AudioSource = UnityEngine.AudioSource;

public class rightHammer : MonoBehaviour
{
    private Mole[] moles;
    private Rigidbody rb;


    private AudioSource[] pianoSounds;
    private AudioSource head1;
    private AudioSource head2;
    private AudioSource head3;
    private AudioSource head4;
    private AudioSource head5;

    private AudioSource neck1;
    private AudioSource neck2;
    private AudioSource neck3;
    private AudioSource neck4;
    private AudioSource neck5;

    private AudioSource chest1;
    private AudioSource chest2;
    private AudioSource chest3;
    private AudioSource chest4;
    private AudioSource chest5;

    private AudioSource stomach1;
    private AudioSource stomach2;
    private AudioSource stomach3;
    private AudioSource stomach4;
    private AudioSource stomach5;

    private AudioSource hip1;
    private AudioSource hip2;
    private AudioSource hip3;
    private AudioSource hip4;
    private AudioSource hip5;

    private AudioSource[,] spaitializedSounds;

    private int xval;
    private int yval;
    private int oldX;
    private int oldY;

    public float speed = 20f;

    private Vector3 newRightHammerPosition;
    public static Quaternion endRotation;

    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list 

        if (MainMenu.handedness == "Left")
        {
            Destroy(this.gameObject);
        }

        pianoSounds = GetComponents<AudioSource>();

        neck1 = pianoSounds[2];
        neck2 = pianoSounds[2];
        neck3 = pianoSounds[2];
        neck4 = pianoSounds[2];
        neck5 = pianoSounds[2];

        head1 = pianoSounds[4];
        head2 = pianoSounds[4];
        head3 = pianoSounds[4];
        head4 = pianoSounds[4];
        head5 = pianoSounds[4];

        chest1 = pianoSounds[0];
        chest2 = pianoSounds[0];
        chest3 = pianoSounds[0];
        chest4 = pianoSounds[0];
        chest5 = pianoSounds[0];

        stomach1 = pianoSounds[1];
        stomach2 = pianoSounds[1];
        stomach3 = pianoSounds[1];
        stomach4 = pianoSounds[1];
        stomach5 = pianoSounds[1];

        hip1 = pianoSounds[3];
        hip2 = pianoSounds[3];
        hip3 = pianoSounds[3];
        hip4 = pianoSounds[3];
        hip5 = pianoSounds[3];

        head1.panStereo = 1;
        head2.panStereo = .5f;
        head3.panStereo = 0;
        head4.panStereo = -.5f;
        head5.panStereo = -1;

        neck1.panStereo = 1;
        neck2.panStereo = .5f;
        neck3.panStereo = 0;
        neck4.panStereo = -.5f;
        neck5.panStereo = -1;

        chest1.panStereo = 1;
        chest2.panStereo = .5f;
        chest3.panStereo = 0;
        chest4.panStereo = -.5f;
        chest5.panStereo = -1;

        stomach1.panStereo = 1;
        stomach2.panStereo = .5f;
        stomach3.panStereo = 0;
        stomach4.panStereo = -.5f;
        stomach5.panStereo = -1;

        hip1.panStereo = 1;
        hip2.panStereo = .5f;
        hip3.panStereo = 0;
        hip4.panStereo = -.5f;
        hip5.panStereo = -1;

        spaitializedSounds = new AudioSource[5, 5] { { head1, head2, head3, head4, head5 }, { neck1, neck2, neck3, neck4, neck5 }, { chest1, chest2, chest3, chest4, chest5 }, { stomach1, stomach2, stomach3, stomach4, stomach5 }, { hip1, hip2, hip3, hip4, hip5 } };

        moles = GameObject.FindObjectsOfType<Mole>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float height = GameObject.Find("GridManager").transform.localScale.y;
        transform.localScale = new Vector3(height, height, height);

        newRightHammerPosition = new Vector3(-BodySourceView.rightHandPosition.X, BodySourceView.rightHandPosition.Y, BodySourceView.rightHandPosition.Z);

        if (rb.CompareTag("rightHammer"))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newRightHammerPosition, Time.fixedDeltaTime * 13);
            RotateHammer(BodySourceView.rightHandPosition, BodySourceView.rightWristPosition, BodySourceView.rightElbowPosition);
        }

        if (MainMenu.handedness == "Right")
        {
            GivePianoSound();
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
                    mole.HitMole(gameObject);
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

    private int getYval()
    {
        if (transform.localPosition.x < -.4572)
        {
            return 0;
        }
        else if (transform.localPosition.x >= -.4572 && transform.localPosition.x < -.1524)
        {
            return 1;
        }
        else if (transform.localPosition.x >= -.1524 && transform.localPosition.x < .1524)
        {
            return 2;
        }
        else if (transform.localPosition.x >= .1524 && transform.localPosition.x < .4572)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }


    private (int, int) returnPianoNote()
    {
        if (transform.localPosition.y < -.4572)
        {
            xval = 4;
            yval = getYval();
            return (xval, yval);
        }
        else if (transform.localPosition.y >= -.4572 && transform.localPosition.y < -.1524)
        {
            xval = 3;
            yval = getYval();
            return (xval, yval);
        }
        else if (transform.localPosition.y >= -.1524 && transform.localPosition.y < .1524)
        {
            xval = 2;
            yval = getYval();
            return (xval, yval);
        }
        else if (transform.localPosition.y >= .1524 && transform.localPosition.y < .4572)
        {
            xval = 1;
            yval = getYval();
            return (xval, yval);
        }
        else
        {
            xval = 0;
            yval = getYval();
            return (xval, yval);
        }
    }

    IEnumerator playAudioSequentially(int x, int y)
    {
        // if new x and y are the same as the old values, then we do nothing
        if (x == this.oldX && y == this.oldY)
        {

        }
        else
        {
            /// if not, then stop current source and play new source
            spaitializedSounds[this.oldX, this.oldY].Stop();
            spaitializedSounds[x, y].loop = true;
            spaitializedSounds[x, y].Play();
            this.oldX = x;
            this.oldY = y;
        }
        yield return null;

    }

    public void GivePianoSound()
    {
        (int x1, int y1) = returnPianoNote();
        StartCoroutine(playAudioSequentially(x1, y1));
    }

}




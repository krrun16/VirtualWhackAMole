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

    public AudioSource chestNote;
    public AudioSource headNote;
    public AudioSource neckNote;
    public AudioSource stomachNote;
    public AudioSource hipsNote;
    //private AudioSource headNote;
    //private AudioSource hipNote;
    //private AudioSource neckNote;
    //private AudioSource stomachNote;
   // private AudioSource[] pianoSounds;

    public float speed = 20f;

    private Vector3 newLeftHammerPosition;
    private Vector3 newRightHammerPosition;
    public static Quaternion endRotation;

    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list 

        //pianoSounds = GetComponents<AudioSource>();
        //chestNote = pianoSounds[0];
        //stomachNote = pianoSounds[1];
        //neckNote = pianoSounds[2];
        //hipNote = pianoSounds[3];
        //headNote = pianoSounds[4];

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


    private void BucketSoundHorizontal(AudioSource usedSound)
    {
        if (-BodySourceView.rightHandPosition.X < -.4572)
        {
            usedSound.panStereo = 1;
        }
        else if (-BodySourceView.rightHandPosition.X >= -.4572 && -BodySourceView.rightHandPosition.X < -.1524)
        {
            usedSound.panStereo = 0.5f;
        }
        else if (-BodySourceView.rightHandPosition.X >= -.1524 && -BodySourceView.rightHandPosition.X < .1524)
        {
            usedSound.panStereo = 0;
        }
        else if (-BodySourceView.rightHandPosition.X >= .1524 && -BodySourceView.rightHandPosition.X < .4572)
        {
            usedSound.panStereo = -0.5f;
        }
        else if (-BodySourceView.rightHandPosition.X >= .4572)
        {
            usedSound.panStereo = -1;
        }
    }
    /*    
        private void BucketSoundHorizontal(AudioSource usedSound)
        {
            print("Sarah this is the x pos " + transform.localPosition.x);
            if (transform.localPosition.x < -.4572)
            {
                usedSound.panStereo = 1;
            }
            else if (transform.localPosition.x >= -.4572 && transform.localPosition.x < -.1524)
            {
                usedSound.panStereo = 0.5f;
            }
            else if (transform.localPosition.x >= -.1524 && transform.localPosition.x < .1524)
            {
                usedSound.panStereo = 0;
            }
            else if (transform.localPosition.x >= .1524 && transform.localPosition.x < .4572)
            {
                usedSound.panStereo = -0.5f;
            }
            else if (transform.localPosition.x >= .4572)
            {
                usedSound.panStereo = -1;
            }
        }
    */
    //Returns spatialized piano notes for the correct elevation.
    private AudioSource GetPianoNotes()
    {
        if (BodySourceView.rightHandPosition.Y < -.4572)
        {
            BucketSoundHorizontal(hipsNote);
            //myNote = hipNote;
            return hipsNote;
        }
        else if (BodySourceView.rightHandPosition.Y >= -.4572 && BodySourceView.rightHandPosition.Y < -.1524)
        {
            BucketSoundHorizontal(stomachNote);
            //myNote = stomachNote;
            return stomachNote;
        }
        else if (BodySourceView.rightHandPosition.Y >= -.1524 && BodySourceView.rightHandPosition.Y < .1524)
        {
            BucketSoundHorizontal(chestNote);
            return chestNote;
            //myNote = chestNote;
        }
        else if (BodySourceView.rightHandPosition.Y >= .1524 && BodySourceView.rightHandPosition.Y < .4572)
        {
            BucketSoundHorizontal(neckNote);
            return neckNote;
            //myNote = neckNote;
        }
        else
        {
            BucketSoundHorizontal(headNote);
            // myNote = headNote;
            return headNote;
        }
    }

    /*
    //Returns spatialized piano notes for the correct elevation.
    private AudioSource GetPianoNotes()
    {
        if (transform.localPosition.y < -.4572)
        {
            BucketSoundHorizontal(hipsNote);
            //myNote = hipNote;
            return hipsNote;
        }
        else if (transform.localPosition.y >= -.4572 && transform.localPosition.y < -.1524)
        {
            BucketSoundHorizontal(stomachNote);
            //myNote = stomachNote;
            return stomachNote;
        }
        else if (transform.localPosition.y >= -.1524 && transform.localPosition.y < .1524)
        {
            BucketSoundHorizontal(chestNote);
            return chestNote;
            //myNote = chestNote;
        }
        else if (transform.localPosition.y >= .1524 && transform.localPosition.y < .4572)
        {
            BucketSoundHorizontal(neckNote);
            return neckNote;
            //myNote = neckNote;
        }
        else
        {
            BucketSoundHorizontal(headNote);
            // myNote = headNote;
            return headNote;
        }
    }
    */

    // Plays audio sources in order and without overlap
    IEnumerator playAudioSequentially(AudioSource hint)
    {
        hint.Play();
        while (hint.isPlaying)
        {
            yield return null;
        }
        /*
        yield return null;

        //1.Loop through each AudioSource
        foreach (AudioSource currAudio in hints)
        {
            // Play Audio
            currAudio.Play();
            // Wait for it to finish playing
            while (currAudio.isPlaying)
            {
                yield return null;
            }
        } 
        */
    }


    public void GivePianoSound()
    {
        //GetPianoNotes();
        List<AudioSource> thePianoSound = new List<AudioSource>();
        AudioSource currentSound = GetPianoNotes();
       // thePianoSound.Add(GetPianoNotes());
        StartCoroutine(playAudioSequentially(currentSound));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrainingController : MonoBehaviour
{
    private Mole[] moles;
    private Mole targetMole;
    private rightHammer[] hammers;
    private rightHammer rightHammerForSound;
    private leftHammer[] leftHammers;
    private leftHammer leftHammerForSound;
   // private static int score = 0;
    private string hintType;
    private GameObject[] leftHammer;
   // private GameObject[] rightHammer;
    private int molesLeft = 40;
    private float timer = 0f;

    private float oldLeftHipX;
    private float oldRightHipX;

    string moleName;
    private static string moleHit;

    public GameObject textToSpeech;
    public AudioSource outOfBounds;


    // Start is called before the first frame update
    void Start()
    {
        hammers = GameObject.FindObjectsOfType<rightHammer>();
        leftHammers = GameObject.FindObjectsOfType<leftHammer>();
        foreach (rightHammer hammer in hammers)
        {
            rightHammerForSound = hammer; 
        }
        foreach (leftHammer hammer in leftHammers)
        {
            leftHammerForSound = hammer;
        }

        // GameLogic();
        // StartCoroutine(GameLogic());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (MainMenu.handedness == "Right")
        {
            rightHammerForSound.GivePianoSound();
        }
        else
        {
            leftHammerForSound.GivePianoSound();
         //   leftHammerForSound.getAudio();
        }
        */
      //  timer += Time.deltaTime;
       // CsvReadWrite.writeData();
    }



    private IEnumerator GameLogic()
    {
       // int counter = 0;
        float audioTimer = 0f;
       // int totalMoles = 0;
        //MoleCap used to determine if player is within a given 10-mole window.
      //  int moleCap = 10;
       // int firstWindowMoleHit = 0;
       // int levelCounter = 1;
        

        while (molesLeft > -1)
        {

            // Record the old hip positions, if they are the same after an iteration the hips have moved off camera and are no longer tracked
            // If the hips never began to track they will be 0 from the start (Might be able to get away with removing the zero part
            oldLeftHipX = BodySourceView.leftHipPosition.X;
            oldRightHipX = BodySourceView.rightHipPosition.X;
            while ((BodySourceView.leftHipPosition.X == 0 && BodySourceView.rightHipPosition.X == 0) || (BodySourceView.leftHipPosition.X == oldLeftHipX && BodySourceView.rightHipPosition.X == oldRightHipX))
            {
                //each time mole
                Debug.Log("Out of bounds");
                audioTimer += Time.deltaTime;
                if (audioTimer >= 3.5f)
                {
                   // outOfBounds.Play();
                    audioTimer -= 3.5f;
                }
                yield return new WaitForSeconds(1.5f);
                yield return null;
            }


            // if no moles left, we can write our data to an excel file
            if (molesLeft == 0)
            {
            }


            // otherwise, continue providing moles as usual
            else
            {
                yield return new WaitForSeconds(1.0f);

                if (MainMenu.handedness == "Right")
                {
                    rightHammerForSound.GivePianoSound();
                }
                else
                {
                    leftHammerForSound.GivePianoSound();
                }

                //get time that the mole was shown
                DateTime dateTime = DateTime.Now;
               // timeSent = dateTime.TimeOfDay.TotalMilliseconds;
                timer = 0f;
            }
            molesLeft -= 1;

        }
    }
}
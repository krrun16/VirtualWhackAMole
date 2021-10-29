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
    private static int score = 0;
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
        if (MainMenu.handedness == "Right")
        {
            rightHammerForSound.GivePianoSound();
        } else
        {
            leftHammerForSound.GivePianoSound();
        }
        timer += Time.deltaTime;
        //print("hello sarah " + timer);
        CsvReadWrite.writeData();
        //rightHammerForSound.GivePianoSound();
    }

    private IEnumerator GameLogic()
    {
        int counter = 0;
        float audioTimer = 0f;
        int totalMoles = 0;
        //MoleCap used to determine if player is within a given 10-mole window.
        int moleCap = 10;
        int firstWindowMoleHit = 0;
        int levelCounter = 1;

        while (true)
        {
            // Place your method calls
            
            if (MainMenu.handedness == "Right") {
                rightHammerForSound.GivePianoSound();
            } else
            {
                leftHammerForSound.GivePianoSound();
            } 
            yield return new WaitForSeconds(1.5f);
        }
        /*

        while (molesLeft > -1)
        {
            // rightHammerForSound.GivePianoSound();
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
              //  endMusic.Play();
                yield return new WaitForSeconds(3.0f);
                textToSpeech.SetActive(true);
            }


            // otherwise, continue providing moles as usual
            else
            {
                yield return new WaitForSeconds(1.0f);
                // if player chose declarative hints
                if (hintType == "Declarative")
                {
                    //if level is 2 or is >=4, mole pops up without any verbal hints.
                    if (levelCounter == 2 || levelCounter >= 4)
                    {
                    }
                    //else if level is not 2 and < 4, mole pops up with verbal hints.
                    else
                    {
                    }
                }
                //if player chose imperative hints
                else if (hintType == "Imperative")
                {
                    //if level is 2 or >=4, mole pops up without any verbal hints.
                    if (levelCounter == 2 || levelCounter >= 4)
                    {
                    }
                    //else if level is not 2 and < 4, mole pops up with verbal hints.
                    else
                    {
                    }
                }

                //get time that the mole was shown
                DateTime dateTime = DateTime.Now;
               // timeSent = dateTime.TimeOfDay.TotalMilliseconds;
                timer = 0f;
                //Makes mole pop up/go down faster at levels 3 and up.
                if (levelCounter >= 3)
                {
                }
                else
                {
                }
                //if mole wasn't hit.
                if (true)
                {
                    totalMoles++;
                }
                //else mole was hit
            }
            if (UnityEngine.Random.Range(1, 4) == 1 && molesLeft > 1)
            {
                yield return new WaitForSeconds(1.0f);
                textToSpeech.SetActive(true);
                yield return new WaitForSeconds(3.0f);
                textToSpeech.SetActive(false);
            }
            molesLeft -= 1;

            //check if first mole in each 10-mole window was hit.
            if (counter == 1 && totalMoles == 1)
            {
                firstWindowMoleHit = 1;
            }

            //if player hit 7 moles in a window, they are within the 10 mole window, and the moles left are not 0.
            if (counter == 7 && totalMoles <= moleCap && molesLeft != 0)
            {
                //lets player know level of difficulty is increasings
                if (levelCounter <= 3)
                {
                   // nextLevel.Play();
                    yield return new WaitForSeconds(1f);
                    if (levelCounter == 1)
                    {
                      //  hints_removed.Play();
                    }
                    else if (levelCounter == 2)
                    {
                       // hints_and_quicker.Play();
                        yield return new WaitForSeconds(.5f);
                    }
                    else if (levelCounter == 3)
                    {
                       // hints_removed.Play();
                    }
                }

                //reset number of moles hit to 0.
                counter = 0;
                //reset total number of moles out of 5 that have appeared to 0.
                totalMoles = 0;
                levelCounter++;
            }

            //if player has not hit 7 moles in a window, and all the moles in a window have appeared, and molesLeft are not 0.
            else if (counter < 7 && totalMoles >= moleCap && molesLeft != 0)
            {
                //if player had hit the first mole in the window, we subtract 1 hit mole from the counter
                if (firstWindowMoleHit == 1)
                {
                    counter = counter - 1;
                }
                //reset total number of moles out of 5 that have appeared to 0.
                totalMoles = 0;
            }
        }*/
    }

    public static int getScore()
    {
        return score;
    }

}
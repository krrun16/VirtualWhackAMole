using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    private Mole[] moles;
    private Mole targetMole;
    private static int score = 0;
    private string hintType;
    private GameObject[] leftHammer;
    private GameObject[] rightHammer;
    private int molesGiven;
    private int molesLeft;
    private float timer = 0f;

    private float oldLeftHipX;
    private float oldRightHipX;

    string moleName;
    private static string moleHit;
    private static double timeTaken;
    private static int totalHit;
    private double timeSent;

    public GameObject textToSpeech;
    public AudioSource endMusic;
    public AudioSource nextLevel;
    public AudioSource level_notinclude_imperative;
    public AudioSource level_notinclude_declarative;
    public AudioSource level_include_imperative;
    public AudioSource level_include_declarative;
    public AudioSource outOfBounds;
    public AudioSource soClose;
   

    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list
        moles = GameObject.FindObjectsOfType<Mole>();
        leftHammer = GameObject.FindGameObjectsWithTag("leftHammer");
        rightHammer = GameObject.FindGameObjectsWithTag("rightHammer");
        molesLeft = 40;
        hintType = PlayerPrefs.GetString("HintType");

        StartCoroutine(GameLogic());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        CsvReadWrite.writeData();
    }

    private IEnumerator GameLogic()
    {
        int counter = 0;
        float audioTimer = 0f;
        int totalMoles = 0;
        int molesGiven = 0;
        //MoleCap used to determine if player is within a given 5-mole window.
        int moleCap = 5;
        int firstWindowMoleHit = 0;
        int levelCounter = 1;

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
                    outOfBounds.Play();
                    audioTimer -= 3.5f;
                }
                yield return new WaitForSeconds(1.5f);
                yield return null;
            }


            // if no moles left, we can write our data to an excel file
            if (molesLeft == 0)
            {
                endMusic.Play();
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
                    if (levelCounter == 2 || levelCounter >=4)
                    {
                        targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
                        if (totalMoles == 0)
                        {
                            level_notinclude_declarative.Play();
                            yield return new WaitForSeconds(3.75f);
                        }
                        moleName = targetMole.name;
                        targetMole.GiveDeclarativeNoHint();
                        targetMole.playingHint = true;
                    }
                    //else if level is not 2 and < 4, mole pops up with verbal hints.
                    else
                    {
                        targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
                        if (totalMoles == 0)
                        {
                            level_include_declarative.Play();
                            yield return new WaitForSeconds(3.5f);
                        }
                        moleName = targetMole.name;
                        targetMole.GiveDeclarativeHint();
                        targetMole.playingHint = true;
                    }
                }
                //if player chose imperative hints
                else if (hintType == "Imperative")
                {
                    //if level is 2 or >=4, mole pops up without any verbal hints.
                    if ( levelCounter ==2 || levelCounter >= 4)
                    {
                        targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
                        if (totalMoles == 0)
                        {
                            level_notinclude_imperative.Play();
                            yield return new WaitForSeconds(3.75f);
                        }
                        moleName = targetMole.name;
                        targetMole.GiveDeclarativeNoHint();
                        targetMole.playingHint = true;
                    }
                    //else if level is not 2 and < 4, mole pops up with verbal hints.
                    else
                    {
                        targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
                        if (totalMoles == 0)
                        {
                            level_include_imperative.Play();
                            yield return new WaitForSeconds(3.5f);
                        }
                        moleName = targetMole.name;
                        targetMole.GiveImperativeHint();
                        targetMole.playingHint = true;
                    }
                }
                //when hint done playing, show mole.
                yield return new WaitUntil(() => targetMole.playingHint == false); 
                targetMole.ShowMole();

                //get time that the mole was shown
                DateTime dateTime = DateTime.Now;
                timeSent = dateTime.TimeOfDay.TotalMilliseconds;
                timer = 0f;
                //Makes mole pop up/go down faster at levels 3 and up.
                if (levelCounter >=3)
                {
                    yield return new WaitUntil(() => timer > 1.25 || targetMole.isHit == true);
                }
                else
                {
                    yield return new WaitUntil(() => timer > 2 || targetMole.isHit == true);
                }
                //if mole wasn't hit.
                if (targetMole.isHit != true)
                {
                    moleHit = "no";
                    //if mole wasn't hit, set timeTaken to -1 
                    timeTaken = -1;
                    targetMole.MissMole();
                    targetMole.HideMole();
                    totalMoles++;
                    molesGiven++;

                    // when hideMole happens, the mole wasn't hit, so should look at location of closest hammer
                    Vector3 leftPos = leftHammer[0].transform.position;
                    Vector3 rightPos = rightHammer[0].transform.position;
                    float dist = Vector3.Distance(targetMole.transform.position, leftPos);
                    float dist2 = Vector3.Distance(targetMole.transform.position, rightPos);

                    //check if hammer was in neighboring square. 
                    if ((dist <= dist2) & dist < .4572f)
                    {
                        incrementScore();
                        soClose.Play();
                    }
                    else if (dist2 <= .4572f)
                    {
                        incrementScore();
                        soClose.Play();
                    }
                    // add data to row
                    CsvReadWrite.addRow(moleName, moleHit, "Wasn't hit", timeTaken, totalHit, molesGiven, score, levelCounter);
                }
                //else mole was hit
                else
                {
                    moleHit = "yes";
                    totalHit++;
                    counter++;
                    totalMoles++; // inc total moles
                    molesGiven++;
                    yield return new WaitUntil(() => (targetMole.timeHit != 0));
                    timeTaken = (targetMole.timeHit - timeSent) * .001f;
                    CsvReadWrite.addRow(moleName, moleHit, targetMole.getHandHit(), timeTaken, totalHit, molesGiven, score, levelCounter);
                    targetMole.isHit = false;
                }
            }
            if (UnityEngine.Random.Range(1, 4) == 1 && molesLeft > 1)
            {
                yield return new WaitForSeconds(1.0f);
                textToSpeech.SetActive(true);
                yield return new WaitForSeconds(3.0f);
                textToSpeech.SetActive(false);
            }

            targetMole.timeHit = 0;
            molesLeft -= 1;

            //check if first mole in each 5-mole window was hit.
            if (counter == 1 && totalMoles == 1)
            {
                firstWindowMoleHit = 1;
            }

            //if player hit 3 moles in a window, they are within the 5 mole window, and the moles left are not 0.
            if (counter == 3 && totalMoles <= moleCap && molesLeft != 0)
            {
                //lets player know level of difficulty is increasings
                if(levelCounter <= 3)
                {
                    nextLevel.Play();
                }
               
                //reset number of moles hit to 0.
                counter = 0;
                //reset total number of moles out of 5 that have appeared to 0.
                totalMoles = 0;
                levelCounter++;
            }

            //if player has not hit 3 moles in a window, and all the moles in a window have appeared, and molesLeft are not 0.
            else if (counter < 3 && totalMoles >= moleCap && molesLeft != 0)
            {
                //if player had hit the first mole in the window, we subtract 1 hit mole from the counter
                if (firstWindowMoleHit == 1)
                {
                    counter = counter - 1;
                }
                //reset total number of moles out of 5 that have appeared to 0.
                totalMoles = 0;
            }
        }
    }

    public static void incrementScore()
    {
        score += 1;
    }

    public static int getScore()
    {
        return score;
    }
}
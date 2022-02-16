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
    private int molesLeft;
    private float timer = 0f;

    private float oldLeftHipX;
    private float oldRightHipX;

    string moleName;
    private static string moleHit;
    private static double timeTaken;
    private static int totalHit;
    private double timeSent;
    private static int numMissedInRow;
    public static bool orient = false;

    public GameObject textToSpeech;
    public AudioSource endMusic;
    public AudioSource nextLevel;
    public AudioSource hints_and_quicker;
    public AudioSource hints_removed;
    public AudioSource outOfBounds;
    public AudioSource soClose;
    public AudioSource Awesome;
    public AudioSource Congrats;
    public AudioSource Fantastic;
    public AudioSource Great;
    public AudioSource Success;
    public AudioSource KeepGoing;


    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list
        moles = GameObject.FindObjectsOfType<Mole>();
        leftHammer = GameObject.FindGameObjectsWithTag("leftHammer");
        rightHammer = GameObject.FindGameObjectsWithTag("rightHammer");
        molesLeft = 20;
        hintType = PlayerPrefs.GetString("HintType");
        StartCoroutine(GameLogic());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        CsvReadWrite.writeData();
    }

    private bool checkAnkles()
    {
        double minAnkleX = -0.75, maxAnkleX = 0.75, minAnkleZ = 7.7, maxAnkleZ = 10.7;
        double anklesX = (BodySourceView.leftAnklePosition.X + BodySourceView.rightAnklePosition.X) / 2;
        double anklesZ = (BodySourceView.leftAnklePosition.Z + BodySourceView.rightAnklePosition.Z) / 2;
        double shoulderSymm = BodySourceView.rightShoulderPosition.Z - BodySourceView.leftShoulderPosition.Z;
        int distance = 0;

        if (shoulderSymm > .1)
        {
            soClose.Play();
            // playSoundResourceSync("twist_left");
            orient = true;
            return false;

        }
        else if (shoulderSymm < -.1)
        {
            soClose.Play();
            //playSoundResourceSync("twist_right");
            orient = true;
            return false;
        }
        else if (anklesX * 3.28084 < minAnkleX)
        {
            distance = (int)Math.Floor(minAnkleX - (anklesX * 3.28084));
            if (distance < 1) soClose.Play(); // playSoundResourceSync("take_step_right");
            else if (distance == 1) soClose.Play();  //playSoundResourceSync("move_one_right");
            else if (distance == 2) KeepGoing.Play(); // playSoundResourceSync("move_two_right");
            else if (distance == 3) Great.Play(); //playSoundResourceSync("move_three_right");
            else Great.Play();  //playSoundResourceSync("move_more_than_three_right");
            orient = true;
            return false;
        }
        else if (anklesX * 3.28084 > maxAnkleX)
        {
            distance = (int)Math.Floor((anklesX * 3.28084) - maxAnkleX);
            if (distance < 1) soClose.Play(); //playSoundResourceSync("take_step_left");
            else if (distance == 1) soClose.Play(); //playSoundResourceSync("move_one_left");
            else if (distance == 2) KeepGoing.Play();  //playSoundResourceSync("move_two_left");
            else if (distance == 3) Great.Play();  //playSoundResourceSync("move_three_left");
            else Great.Play();  //playSoundResourceSync("move_more_than_three_left");
            orient = true;
            return false;
        }
        else if (anklesZ * 3.28084 < minAnkleZ)
        {
            distance = (int)Math.Floor(minAnkleZ - (anklesZ * 3.28084));
            if (distance < 1) soClose.Play(); //playSoundResourceSync("take_step_backward");
            else if (distance == 1) soClose.Play(); //playSoundResourceSync("move_one_backward");
            else if (distance == 2) KeepGoing.Play();  //playSoundResourceSync("move_two_backward");
            else if (distance == 3) Great.Play();  // playSoundResourceSync("move_three_backward");
            else Great.Play();  //playSoundResourceSync("move_more_than_three_backward");
            orient = true;
            return false;
        }
        else if (anklesZ * 3.28084 > maxAnkleZ)
        {
            distance = (int)Math.Floor((anklesZ * 3.28084) - maxAnkleZ);
            if (distance < 1) soClose.Play();  //playSoundResourceSync("take_step_forward");
            else if (distance == 1) soClose.Play(); // playSoundResourceSync("move_one_forward");
            else if (distance == 2) KeepGoing.Play();  //playSoundResourceSync("move_two_forward");
            else if (distance == 3) Great.Play();  //playSoundResourceSync("move_three_forward");
            else Great.Play();  //playSoundResourceSync("move_more_than_three_forward");
            orient = true;
            return false;
        }
        else
        {
            orient = false;
            Fantastic.Play();
            return true;
            
            //playSoundResourceSync("good_position");
                               // othread = new Thread(startGame);
                               // othread.Start();
           // StartCoroutine(GameLogic());
        }
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
        numMissedInRow = 0;

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
                yield return new WaitForSeconds(.5f);
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
               // yield return new WaitForSeconds(1.0f);
                // if player chose declarative hints
                if (hintType == "Declarative")
                {
                    //if level is 2 or is >=4, mole pops up without any verbal hints.
                    if (levelCounter == 2 || levelCounter >=4)
                    {
                        targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
                        moleName = targetMole.name;
                        targetMole.GiveNoHint();
                        targetMole.playingHint = true;
                    }
                    //else if level is not 2 and < 4, mole pops up with verbal hints.
                    else
                    {
                        targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
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
                        moleName = targetMole.name;
                        targetMole.GiveNoHint();
                        targetMole.playingHint = true;
                    }
                    //else if level is not 2 and < 4, mole pops up with verbal hints.
                    else
                    {
                        targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
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
                    numMissedInRow++;
                    moleHit = "no";
                    //if mole wasn't hit, set timeTaken to -1 
                    timeTaken = -1;
                    targetMole.MissMole();
                    targetMole.HideMole();
                    yield return new WaitForSeconds(.5f);
                    totalMoles++;

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
                    if (numMissedInRow == 5 || numMissedInRow == 10 || numMissedInRow == 15)
                    {
                        KeepGoing.Play();
                    }
                    // add data to row
                    CsvReadWrite.addRow(moleName, moleHit, "Wasn't hit", timeTaken, totalHit, score, levelCounter);
                }
                //else mole was hit
                else
                {
                    numMissedInRow = 0;
                    switch (UnityEngine.Random.Range(0, 4))
                    {
                        case 0:
                            Awesome.Play();
                            break;
                        case 1:
                            Fantastic.Play();
                            break;
                        case 2:
                            Congrats.Play();
                            break;
                        case 3:
                            Great.Play();
                            break;
                        case 4:
                            Success.Play();
                            break;
                    }
                    yield return new WaitForSeconds(1f);
                    moleHit = "yes";
                    totalHit++;
                    counter++;
                    totalMoles++; // inc total moles

                    yield return new WaitUntil(() => (targetMole.timeHit != 0));
                    timeTaken = (targetMole.timeHit - timeSent) * .001f;
                    CsvReadWrite.addRow(moleName, moleHit, targetMole.getHandHit(), timeTaken, totalHit, score, levelCounter);
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
                    nextLevel.Play();
                    yield return new WaitForSeconds(1f);
                    if (levelCounter == 1)
                    {
                        hints_removed.Play();
                        yield return new WaitForSeconds(2f);
                    }
                    else if (levelCounter == 2)
                    {
                        hints_and_quicker.Play();
                        yield return new WaitForSeconds(2.5f);
                    }
                    else if (levelCounter == 3)
                    {
                        hints_removed.Play();
                        yield return new WaitForSeconds(2f);
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
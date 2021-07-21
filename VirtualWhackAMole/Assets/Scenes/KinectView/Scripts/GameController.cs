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

    private float oldLeftHipX = 0;
    private float oldRightHipX = 0;

    string moleName;
    private static string moleHit;
    private static double timeTaken;
    private static int totalHit;
    private double timeSent;

    public GameObject textToSpeech;
    public AudioSource endMusic;
    public AudioSource nextLevel;
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
        
    }

    private IEnumerator GameLogic()
    {
        int counter = 0;
        float audioTimer = 0f;
        int totalMoles = 0;
        int moleCap = 5;
        int[] moleArray = new int[40]; // outer array used to determine which mole is hit.
        int[] moleArrayOutput = new int[40]; //array to determine to put mole info into.
      //  int firstMoleHit = 0;

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

                    yield return null;
                }
                for (int i = 0; i < moleArray.Length; i++)
                {

                    yield return new WaitForSeconds(1.5f);
              

                    // if(counter ==1 && totalMoles ==1)
                    // {
                    //    firstMoleHit = 1;
                    // }

                    //if (player hits 3 moles within a window of 5 moles, increase level.
                    //   if (counter == 3 && totalMoles <= moleCap && molesLeft !=0)
                    //  {
                    //nextLevel.Play();
                    // counter = 0;
                    // }

                    //  else if(counter <3 && totalMoles>moleCap && molesLeft !=0 )
                    // {
                    //   if(firstMoleHit !=0)
                    // {
                    //     counter = counter - 1;
                    // }
                    //moleCap++;
                    // }



                    // if no moles left, we can write our data to an excel file
                    if (molesLeft == 0)
                    {
                        CsvReadWrite.writeData();
                        endMusic.Play();
                        yield return new WaitForSeconds(3.0f);
                        textToSpeech.SetActive(true);
                    }

                    // otherwise, continue providing moles as usual
                    else
                    {
                        yield return new WaitForSeconds(1.0f);

                        if (hintType == "Declarative")
                        {

                            targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
                            moleName = targetMole.name;
                            targetMole.GiveDeclarativeHint();
                            targetMole.playingHint = true;

                        }
                        else if (hintType == "Imperative")
                        {

                            targetMole = moles[UnityEngine.Random.Range(0, moles.Length)];
                            moleName = targetMole.name;
                            targetMole.GiveImperativeHint();
                            targetMole.playingHint = true;

                        }

                        yield return new WaitUntil(() => targetMole.playingHint == false);  //when hint done playing, show mole.
                        targetMole.ShowMole();

                        //get time that the mole was shown
                        DateTime dateTime = DateTime.Now;
                        timeSent = dateTime.TimeOfDay.TotalMilliseconds;
                        timer = 0f;

                        yield return new WaitUntil(() => timer > 2 || targetMole.isHit == true);

                        if (targetMole.isHit != true)
                        {

                            moleHit = "no";

                            //if mole wasn't hit, set timeTaken to -1 
                            timeTaken = -1;
                            targetMole.MissMole();
                            targetMole.HideMole();
                            totalMoles++; //inc totalMoles

                            // when hideMole happens, the mole wasn't hit, so should look at location of closest hammer
                            Vector3 leftPos = leftHammer[0].transform.position;
                            Vector3 rightPos = rightHammer[0].transform.position;
                            float dist = Vector3.Distance(targetMole.transform.position, leftPos);
                            float dist2 = Vector3.Distance(targetMole.transform.position, rightPos);

                            //check if hammer was in neighboring square
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
                            CsvReadWrite.addRow(moleName, moleHit, "Wasn't hit", timeTaken, totalHit, score);

                        }
                        else
                        {

                            moleHit = "yes";
                            totalHit++;
                            counter++;
                            totalMoles++; // inc total moles
                            yield return new WaitUntil(() => (targetMole.timeHit != 0));
                            timeTaken = (targetMole.timeHit - timeSent) * .001f;
                            CsvReadWrite.addRow(moleName, moleHit, targetMole.getHandHit(), timeTaken, totalHit, score);

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

                    if(targetMole.isHit)
                    {
                        moleArrayOutput[i] = 1;
                    }
                    else
                    {
                        moleArrayOutput[i] = 0;
                    }

                    //if player hits 3 moles in 5 mole window, next level
                    if(counter ==3 && totalMoles <= moleCap && molesLeft !=0)
                    {
                        nextLevel.Play();
                        counter = 0;
                    }
                    else if(counter < 3 && totalMoles >= moleCap && molesLeft !=0)
                    {
                        soClose.Play();
                        if(moleArrayOutput[i-4]==0) // this is actually checking the index, not value at index
                        {
                            counter = counter - 1;
                        }

                        moleCap= moleCap+ 2;
                    }






                }
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

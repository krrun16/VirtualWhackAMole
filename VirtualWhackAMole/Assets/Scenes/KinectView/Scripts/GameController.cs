using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    /*Game lasts 15 moles, first mole shows after 5 seconds.
    * Mole is shown for 2 seconds, there is a 1 second break, and another
    * mole appears. 
    */
    private Mole[] moles;
    private Mole targetMole;
    private static int score = 0;
    private string hintType;
    private GameObject[] leftHammer;
    private GameObject[] rightHammer;
    private int molesLeft;
    private float timer = 0f;

    string moleName;
    private static string moleHit;
    private static double timeTaken;
    private static int totalHit;
    private double timeSent;

    public GameObject textToSpeech;
    public AudioSource endMusic;
    public AudioSource nextLevel;

    

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
        yield return new WaitForSeconds(5.0f);
        while (molesLeft > -1)
        {
            //if player hits 5 moles, they advance to another window.
           if(counter ==5 && molesLeft != 0) 
           {
               nextLevel.Play();
               counter = 0;   
           }

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
                    // when hideMole happens, the mole wasn't hit, so should look at location of closest hammer
                    Vector3 leftPos = leftHammer[0].transform.position;
                    Vector3 rightPos = rightHammer[0].transform.position;
                    float dist = Vector3.Distance(targetMole.transform.position, leftPos);
                    float dist2 = Vector3.Distance(targetMole.transform.position, rightPos);
                    //check if hammer was in neighboring square
                    if ((dist <= dist2) & dist < .4572f)
                    {
                        incrementScore();
                    }
                    else if (dist2 <= .4572f) 
                    {
                        incrementScore();
                    }
                    // add data to row
                    CsvReadWrite.addRow(moleName, moleHit, timeTaken, totalHit, score);  
                } else 
                {
                  
                        moleHit = "yes";
                        totalHit++;
                        counter++;
                        yield return new WaitUntil(() => (targetMole.timeHit != 0));
                        timeTaken = (targetMole.timeHit - timeSent) * .001f;
                        CsvReadWrite.addRow(moleName, moleHit, timeTaken, totalHit, score);

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

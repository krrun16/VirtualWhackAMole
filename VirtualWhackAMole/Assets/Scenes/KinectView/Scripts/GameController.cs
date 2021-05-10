using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list 
        moles = GameObject.FindObjectsOfType<Mole>();
        leftHammer = GameObject.FindGameObjectsWithTag("leftHammer");
        rightHammer = GameObject.FindGameObjectsWithTag("rightHammer");
        molesLeft = 15;
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
        yield return new WaitForSeconds(5.0f);
        while (molesLeft > 0)
        {
            targetMole = moles[Random.Range(0, moles.Length)];
            yield return new WaitForSeconds(1.0f);
            if (hintType == "Declarative")
            {
                targetMole.GiveDeclarativeHint();
                targetMole.playingHint = true;
            }
            else if (hintType == "Imperative")
            {
                targetMole.GiveImperativeHint();
                targetMole.playingHint = true;
            }
            
            yield return new WaitUntil(() => targetMole.playingHint == false);
            targetMole.ShowMole();
            timer = 0f;
            yield return new WaitUntil(() => timer > 2 || targetMole.isHit == true);
            if (targetMole.isHit != true)
            {
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
                } else if (dist2 <= .4572f)
                {
                    incrementScore();
                }
            }
            else
            {
                targetMole.isHit = false;
            }
            molesLeft -= 1;
        }      
    }

    public static void incrementScore()
    {
        score += 1;
    }
}

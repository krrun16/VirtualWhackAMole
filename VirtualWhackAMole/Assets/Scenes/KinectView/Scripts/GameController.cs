using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /*Game lasts 30 seconds, first mole shows after 1 second.
    * Mole is shown for 2 seconds, there is a .8 second break, and another
    * mole appears. 
    */
    private float gameTimer = 30f;
    private float showMoleTimer = 1f;
    private float hideMoleTimer;
    private Mole[] moles;
    private Mole targetMole;
    private bool showingMole = false;
    private static int score = 0;
    private bool inGame;

    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list 
        moles = GameObject.FindObjectsOfType<Mole>();
        inGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(inGame)
        {
            gameTimer -= Time.deltaTime; 
            if (gameTimer > 0f)
            {
                showMoleTimer -= Time.deltaTime;
                hideMoleTimer -= Time.deltaTime;
        
                if (showMoleTimer < 0f && showingMole == false)
                {
                    targetMole = moles[Random.Range(0, moles.Length)];
                    targetMole.ShowMole();
                    showingMole = true;
                    hideMoleTimer = 2f;
                }
                if (hideMoleTimer < 0f && showingMole == true)
                {
                    targetMole.HideMole();
                    showMoleTimer = .8f;
                    showingMole = false;
                }
            }
            else 
            {
                targetMole.HideMole();
                targetMole.StopShowSound();
                Debug.Log("Score (moles hit) : " + score);
                inGame = false;
            }
        }
    }

    public static void incrementScore()
    {
        score += 1;
    }

}

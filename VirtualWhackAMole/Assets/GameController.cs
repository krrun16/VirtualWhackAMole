using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float gameTimer;

    //mole Variables
    public GameObject MoleContainer;
    private Mole[] moles;
    public float showMoletimer = 2f; //shows mole every 5 seconds... need to look up how often we want to show a mole 

    // Start is called before the first frame update
    void Start()
    {
        // adding Moles into list 
        moles = MoleContainer.GetComponentsInChildren<Mole>();
        Debug.Log("This is the number of moles: " + moles.Length);
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime; 
        if (gameTimer > 0f)
        {
            showMoletimer -= Time.deltaTime;
            //show mole if showMoleTimer is 0;
            if (showMoletimer < 0f)
            {
                moles[Random.Range(0, moles.Length)].ShowMole();
                showMoletimer = 2f;
            }
        }
    }
}

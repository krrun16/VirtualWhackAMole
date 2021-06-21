using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;




public class MainMenu : MonoBehaviour
{
    public Button play;
    public Button options;
    public Button exit;
    public AudioSource Play;
    public AudioSource Options;
    public AudioSource Exit;
    int counter = 0;
    void Start()
    {
        // Default Player Preferences
        PlayerPrefs.SetString("HintType", "Declarative");
        PlayerPrefs.SetString("DominantHand", "Right");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // yield return wait and check to see if they pressed shift

    private void Update()           //CREATED 18JUN21
    {
        Debug.Log(counter);
        // Notes: Make this its own method and call it everytime tab is pressed but not in update,
        // find a way to put a pause after tab is pressed               
        Button[] buttonArray = { play, options, exit };
        AudioSource[] audioArray = { Play, Options, Exit };

        // checking if our menu scene still exists
        // while(mainmenu){
      
  
        //If counter goes out of array bounds, return to 1st element of arrays.
        if(counter > 3)
        {
            buttonArray.ElementAt(0).Select();
            audioArray.ElementAt(0).Play();
        }

        //If person presses tab, we go down page upon tab release.
        // ERROR: we have to press tab to go back up, but this executes and increases
        // count to 4
        if (Input.GetKeyUp("tab"))
        {
             buttonArray.ElementAt(counter).Select();
             audioArray.ElementAt(counter).Play();
             counter++;
        }

        //if person presses tab and shift, we go backwards
        // check out difference between getkey and getkeydown
        if (Input.GetKey("tab"))
        {
            if (Input.GetKeyDown("left shift"))
            {
                counter = counter - 2;
                buttonArray.ElementAt(counter).Select();
                audioArray.ElementAt(counter).Play();
            }
        }
    }
}


    


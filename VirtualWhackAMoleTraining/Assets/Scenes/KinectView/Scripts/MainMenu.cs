using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource pleaseEnter;
    public static string handedness;
    public static string inputFieldText;


    void Start()
    {
        intro.Play();
        PlayerPrefs.SetString("Training", "Sound");
        PlayerPrefs.SetString("HintType", "Declarative");
        PlayerPrefs.SetString("DominantHand", "Right");
    }

    private void Update()
    {
        inputFieldText = stringReciever.getPartNumber();
    }
    public void PlayGame() 
    {
        inputFieldText = stringReciever.getPartNumber();
        if (inputFieldText == null || inputFieldText == "") {
            pleaseEnter.Play();
        }
        else
        {
            handedness = PlayerPrefs.GetString("DominantHand");

            if (PlayerPrefs.GetString("Training") == "Sound")
            {
                SceneManager.LoadScene("SoundTraining");
            } else
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

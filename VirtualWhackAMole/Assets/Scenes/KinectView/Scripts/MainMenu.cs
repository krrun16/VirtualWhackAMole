﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public AudioSource pleaseEnter;
    public static string inputFieldText;

    void Start()
    {
        // Default Player Preferences
        PlayerPrefs.SetString("HintType", "Declarative");
        PlayerPrefs.SetString("DominantHand", "Right");
    }


    public void PlayGame() 
    {
        inputFieldText = stringReciever.getPartNumber();
        if (inputFieldText == null) {
            pleaseEnter.Play();
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

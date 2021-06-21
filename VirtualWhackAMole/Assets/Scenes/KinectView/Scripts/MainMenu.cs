﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    AudioSource intro;
    
    void Start()
    {
        intro.Play();
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

}

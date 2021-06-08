using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreAudio : MonoBehaviour
{
    public static AudioSource[] scoreSounds;

    // Start is called before the first frame update
    void Start()
    {
        scoreSounds = GetComponents<AudioSource>();
    }

    public static void playScore(int score)
    {
        scoreSounds[score].Play();
    }
  
}

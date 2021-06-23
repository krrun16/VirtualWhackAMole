using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenReader : MonoBehaviour
{
    private AudioSource[] srMenu;
    private AudioSource participantInput;
    private AudioSource declarativeBox;
    private AudioSource imperativeBox;
    private AudioSource leftHandBox;
    private AudioSource rightHandBox;
    private AudioSource playButton;
    private AudioSource exitButton;

    // Start is called before the first frame update
    void Start()
    {
        // Get all the audio sources attatched to the object
        srMenu = GetComponents<AudioSource>();

        participantInput = srMenu[0];
        declarativeBox = srMenu[1];
        imperativeBox = srMenu[2];
        leftHandBox = srMenu[3];
        rightHandBox = srMenu[4];
        playButton = srMenu[5];
        exitButton = srMenu[6];
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            Debug.Log("we got tab here");
        }
    }
}

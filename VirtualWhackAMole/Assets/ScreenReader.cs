using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenReader : MonoBehaviour
{
    // Eventually want to find a way to put these in an easily indexable array, but because they're different
    // types itd be hard (I think everything in unity is a game object so maybe I can do something with that
    public InputField participantInput;
    public Toggle declarativeBox;
    public Toggle imperativeBox;
    public Toggle leftHandBox;
    public Toggle rightHandBox;
    public Button playButton;
    public Button exitButton;


    private AudioSource[] srMenu;
    private AudioSource participantInputAudio;
    private AudioSource declarativeBoxAudio;
    private AudioSource imperativeBoxAudio;
    private AudioSource leftHandBoxAudio;
    private AudioSource rightHandBoxAudio;
    private AudioSource playButtonAudio;
    private AudioSource exitButtonAudio;
    [SerializeField]
    private int counter;


    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        // Get all the audio sources attatched to the object
        srMenu = GetComponents<AudioSource>();

        participantInputAudio = srMenu[0];
        declarativeBoxAudio = srMenu[1];
        imperativeBoxAudio = srMenu[2];
        leftHandBoxAudio = srMenu[3];
        rightHandBoxAudio = srMenu[4];
        playButtonAudio = srMenu[5];
        exitButtonAudio = srMenu[6];

        participantInput.Select();
        participantInputAudio.PlayDelayed(6);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            if (counter == 0)
            {
                counter = 6;
                SelectMenu(counter);
            }
            else
            {
                counter--;
                SelectMenu(counter);
            }
         
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !Input.GetKey(KeyCode.LeftShift))
        { 
            if (counter == 6)
            {
                counter = 0;
                SelectMenu(counter);
            }
            else
            {
                counter++;
                SelectMenu(counter);
            }
            
        }
        
    }
    void SelectMenu(int idx)
    {
        // Allows us to cycle through the menu
        if (counter < 0 || counter > 6)
        {
            counter = 0;
        }

        switch (idx)
        {
            case 0:
                if (exitButtonAudio.isPlaying)
                {
                    exitButtonAudio.Stop();
                }
                if (declarativeBoxAudio.isPlaying)
                {
                    declarativeBoxAudio.Stop();
                }

                participantInput.Select();
                participantInputAudio.Play();
                break;
            case 1:
                if (participantInputAudio.isPlaying)
                {
                    participantInputAudio.Stop();
                }
                if (imperativeBoxAudio.isPlaying)
                {
                    imperativeBoxAudio.Stop();
                }

                declarativeBox.Select();
                declarativeBoxAudio.Play();
                break;
            case 2:
                if (declarativeBoxAudio.isPlaying)
                {
                    declarativeBoxAudio.Stop();
                }
                if (leftHandBoxAudio.isPlaying)
                {
                    leftHandBoxAudio.Stop();
                }

                imperativeBox.Select();
                imperativeBoxAudio.Play();
                break;
            case 3:
                if (imperativeBoxAudio.isPlaying)
                {
                    imperativeBoxAudio.Stop();
                }
                if (rightHandBoxAudio.isPlaying)
                {
                    rightHandBoxAudio.Stop();
                }

                leftHandBox.Select();
                leftHandBoxAudio.Play();
                break;
            case 4:
                if (leftHandBoxAudio.isPlaying)
                {
                    leftHandBoxAudio.Stop();
                }
                if (playButtonAudio.isPlaying)
                {
                    playButtonAudio.Stop();
                }

                rightHandBox.Select();
                rightHandBoxAudio.Play();
                break;
            case 5:
                if (rightHandBoxAudio.isPlaying)
                {
                    rightHandBoxAudio.Stop();
                }
                if (exitButtonAudio.isPlaying)
                {
                    exitButtonAudio.Stop();
                }

                playButton.Select();
                playButtonAudio.Play();
                break;
            case 6:
                if (participantInputAudio.isPlaying)
                {
                    participantInputAudio.Stop();
                }
                else
                {
                    playButtonAudio.Stop();
                }

                exitButton.Select();
                exitButtonAudio.Play();
                break;
        }
    }
}

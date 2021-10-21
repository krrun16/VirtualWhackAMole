using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenReader : MonoBehaviour
{
    // Eventually want to find a way to put these in an easily indexable array, but because they're different
    // types itd be hard (I think everything in unity is a game object so maybe I can do something with that)
    public InputField participantInput;
    public Toggle declarativeBox;
    public Toggle imperativeBox;
    public Toggle soundBox;
    public Toggle moleBox;
    public Toggle leftHandBox;
    public Toggle rightHandBox;
    public Button playButton;
    public Button exitButton;
    // Reading what is in the inputfield
    public GameObject textToSpeech;
    public static string inputFieldText;
    public static int state = 0;
    

    private AudioSource[] srMenu;
    private AudioSource participantInputAudio;
    private AudioSource declarativeBoxAudio;
    private AudioSource imperativeBoxAudio;
    private AudioSource leftHandBoxAudio;
    private AudioSource rightHandBoxAudio;
    private AudioSource playButtonAudio;
    private AudioSource exitButtonAudio;
    private AudioSource isChecked;
    private AudioSource isUnchecked;
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
        rightHandBoxAudio = srMenu[3];
        leftHandBoxAudio = srMenu[4];
        playButtonAudio = srMenu[5];
        exitButtonAudio = srMenu[6];
        isChecked = srMenu[7];
        isUnchecked = srMenu[8];
        participantInput.Select();
        participantInputAudio.PlayDelayed(6);
    }

    // Update is called once per frame
    void Update()
    {
        // check if someone has left shift down and is trying to tab backwards
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
        // if someone is just pressing tab then they are going forward
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
                if (isChecked.isPlaying)
                {
                    isChecked.Stop();
                }
                if (isUnchecked.isPlaying)
                {
                    isUnchecked.Stop();
                }

                participantInput.Select();
                participantInputAudio.Play();
                inputFieldText = stringReciever.getPartNumber();
                if (!string.IsNullOrEmpty(inputFieldText))
                {
                    Debug.Log(inputFieldText);
                    state = 1;
                    textToSpeech.SetActive(true);
                }

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
                if (isChecked.isPlaying)
                {
                    isChecked.Stop();
                }
                if (isUnchecked.isPlaying)
                {
                    isUnchecked.Stop();
                }

                
                if (declarativeBox.isOn)
                {
                    isChecked.PlayDelayed(2f);
                }
                else
                {
                    isUnchecked.PlayDelayed(2f);
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
                if (isChecked.isPlaying)
                {
                    isChecked.Stop();
                }
                if (isUnchecked.isPlaying)
                {
                    isUnchecked.Stop();
                }

                if (imperativeBox.isOn)
                {
                    isChecked.PlayDelayed(2f);
                }
                else
                {
                    isUnchecked.PlayDelayed(2f);
                }

                imperativeBox.Select();
                imperativeBoxAudio.Play();
                break;
            case 3:
                // Check for audio stopping
                if (imperativeBoxAudio.isPlaying)
                {
                    imperativeBoxAudio.Stop();
                }
                if (rightHandBoxAudio.isPlaying)
                {
                    rightHandBoxAudio.Stop();
                }
                if (isChecked.isPlaying)
                {
                    isChecked.Stop();
                }
                if (isUnchecked.isPlaying)
                {
                    isUnchecked.Stop();
                }

                if (rightHandBox.isOn)
                {
                    isChecked.PlayDelayed(2f);
                }
                else
                {
                    isUnchecked.PlayDelayed(2f);
                }

                rightHandBox.Select();
                rightHandBoxAudio.Play();

                break;
            case 4:
                // Check for audio stopping
                if (rightHandBoxAudio.isPlaying)
                {
                    rightHandBoxAudio.Stop();
                }
                if (exitButtonAudio.isPlaying)
                {
                    exitButtonAudio.Stop();
                }
                if (isChecked.isPlaying)
                {
                    isChecked.Stop();
                }
                if (isUnchecked.isPlaying)
                {
                    isUnchecked.Stop();
                }

                if (leftHandBox.isOn)
                {
                    isChecked.PlayDelayed(2f);
                }
                else
                {
                    isUnchecked.PlayDelayed(2f);
                }

                leftHandBox.Select();
                leftHandBoxAudio.Play();
                break;
            case 5:
                if (leftHandBoxAudio.isPlaying)
                {
                    leftHandBoxAudio.Stop();
                }
                if (playButtonAudio.isPlaying)
                {
                    playButtonAudio.Stop();
                }
                if (isChecked.isPlaying)
                {
                    isChecked.Stop();
                }
                if (isUnchecked.isPlaying)
                {
                    isUnchecked.Stop();
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
    public static int getState()
    {
        return state;
    }
}

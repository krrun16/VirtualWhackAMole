using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{

    private ToggleGroup hintGroup;
    private ToggleGroup handGroup;
    public Toggle declarativeButton;
    public Toggle imperativeButton;
    public Toggle rightButton;
    public Toggle leftButton;
    public AudioSource Declarative;
    public AudioSource Imperative;
    public AudioSource Right;
    public AudioSource Left;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        hintGroup = GameObject.Find("ToggleHintGroup").GetComponent<ToggleGroup>();
        handGroup = GameObject.Find("ToggleHandGroup").GetComponent<ToggleGroup>();
    }


    private void Awake()
    {
        Declarative.Play();
    }
    private void Update()
    {
        Toggle[] optionsButtons = { declarativeButton, imperativeButton, rightButton, leftButton };
        AudioSource[] optionsAudio = { Declarative, Imperative, Right, Left };

        //If counter goes out of array bounds, return to 1st element of arrays.
        if (counter > 4)
        {
            optionsButtons.ElementAt(0).Select();
            optionsAudio.ElementAt(0).Play();
        }

        if (Input.GetKeyUp("tab"))
        {
            optionsButtons.ElementAt(counter).Select();
            optionsAudio.ElementAt(counter).Play();
            counter++;
        }

        if (Input.GetKey("tab"))
        {
           if (Input.GetKeyDown("left shift"))
           {

                counter = counter - 2;
                optionsButtons.ElementAt(counter).Select();
                optionsAudio.ElementAt(counter).Play();
            }
        }

    }

    public void HintChange()
    {
        PlayerPrefs.SetString("HintType", hintGroup.ActiveToggles().First().name);        
    }

    public void HandChange()
    {
        PlayerPrefs.SetString("DominantHand", handGroup.ActiveToggles().First().name);
      
    }
    
    
}

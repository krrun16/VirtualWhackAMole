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
       // if down arrow key pressed, switch from declarative button to imperative button.
       if (Input.GetKeyDown("down") && PlayerPrefs.GetString("HintType") == "Declarative")
        {
            PlayerPrefs.SetString("HintType", "Imperative");
            declarativeButton.isOn = false;
            imperativeButton.isOn = true;
            Debug.Log(PlayerPrefs.GetString("HintType"));
            Imperative.Play();
        }

       // if up arrow key pressed, switch from imperative button to declarative button.
        if (Input.GetKeyDown("up") && PlayerPrefs.GetString("HintType") == "Imperative")
        {
            PlayerPrefs.SetString("HintType", "Declarative");
            declarativeButton.isOn = true;
            imperativeButton.isOn = false;
            Debug.Log(PlayerPrefs.GetString("HintType"));
            Declarative.Play();
        }

        //if left arrow key pressed, right hand selected
        if(Input.GetKeyDown("left") && PlayerPrefs.GetString("DominantHand") == "Right" )
        {
            PlayerPrefs.SetString("DominantHand", "Left");
            rightButton.isOn = false;
            leftButton.isOn = true;
            Debug.Log(PlayerPrefs.GetString("DominantHand"));
            Left.Play();
        }

        //if right arrow key pressed, left hand selected
        if (Input.GetKeyDown("right") && PlayerPrefs.GetString("DominantHand") == "Left")
        {
            PlayerPrefs.SetString("DominantHand", "Right");
            rightButton.isOn = true;
            leftButton.isOn = false;
            Debug.Log(PlayerPrefs.GetString("DominantHand"));
            Right.Play();
        }

    }

    public void HintChange()
    {
        PlayerPrefs.SetString("HintType", hintGroup.ActiveToggles().First().name);        
    }

    public void HandChange()
    {
        PlayerPrefs.SetString("DominantHand", handGroup.ActiveToggles().First().name);
        //DO NOT DELETE THE 2 LINES BELOW!!! WE MAY STILL NEED THEM. LEFT/RIGHT OPTIONS MENU BUTTONS WORK WHEN THEY'RE COMMENTED OUT.
       //rightButton.isOn = false;
       //leftButton.isOn = true;
    }
    
    
}

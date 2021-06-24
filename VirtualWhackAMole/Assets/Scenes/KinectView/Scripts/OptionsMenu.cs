using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OptionsMenu : MonoBehaviour
{

    private ToggleGroup hintGroup;
    private ToggleGroup handGroup;

    // Start is called before the first frame update
    void Start()
    {
        hintGroup = GameObject.Find("ToggleHintGroup").GetComponent<ToggleGroup>();
        handGroup = GameObject.Find("ToggleHandGroup").GetComponent<ToggleGroup>();
    }

    public void HintChange()
    {
        PlayerPrefs.SetString("HintType", hintGroup.ActiveToggles().First().name);
        Debug.Log(PlayerPrefs.GetString("HintType"));

    }

    public void HandChange()
    {
        PlayerPrefs.SetString("DominantHand", handGroup.ActiveToggles().First().name);
        Debug.Log(PlayerPrefs.GetString("DominantHand"));
    }
}

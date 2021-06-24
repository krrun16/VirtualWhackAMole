using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OptionsMenu : MonoBehaviour
{

    private ToggleGroup hintGroup;
    private ToggleGroup handGroup;
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        hintGroup = GameObject.Find("ToggleHintGroup").GetComponent<ToggleGroup>();
        handGroup = GameObject.Find("ToggleHandGroup").GetComponent<ToggleGroup>();
    }

<<<<<<< Updated upstream
    private void Update()
    {
        Debug.Log(PlayerPrefs.GetString("HintType"));
        Debug.Log(PlayerPrefs.GetString("DominantHand"));
    }

    public void HintChange()
    {
        PlayerPrefs.SetString("HintType", hintGroup.ActiveToggles().First().name); 
=======
    public void HintChange()
    {
        PlayerPrefs.SetString("HintType", hintGroup.ActiveToggles().First().name);
        Debug.Log(PlayerPrefs.GetString("HintType"));
>>>>>>> Stashed changes
    }

    public void HandChange()
    {
        PlayerPrefs.SetString("DominantHand", handGroup.ActiveToggles().First().name);
<<<<<<< Updated upstream
=======
        Debug.Log(PlayerPrefs.GetString("DominantHand"));
>>>>>>> Stashed changes
    }
}

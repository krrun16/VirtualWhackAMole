using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OptionsMenu : MonoBehaviour
{

    private ToggleGroup hintGroup;
    private ToggleGroup handGroup;
    private ToggleGroup trainingGroup;
    public AudioSource check;
    // Start is called before the first frame update
    void Start()
    {
        hintGroup = GameObject.Find("ToggleHintGroup").GetComponent<ToggleGroup>();
        handGroup = GameObject.Find("ToggleHandGroup").GetComponent<ToggleGroup>();
        trainingGroup = GameObject.Find("ToggleTrainingGroup").GetComponent<ToggleGroup>();
    }

    public void HintChange()
    {
        PlayerPrefs.SetString("HintType", hintGroup.ActiveToggles().First().name);
        check.Play();
        Debug.Log(PlayerPrefs.GetString("HintType"));
    }

    public void HandChange()
    {
        PlayerPrefs.SetString("DominantHand", handGroup.ActiveToggles().First().name);
        check.Play();
        Debug.Log(PlayerPrefs.GetString("DominantHand"));
    }

    public void TrainingChange()
    {
        PlayerPrefs.SetString("Training", trainingGroup.ActiveToggles().First().name);
        check.Play();
        Debug.Log(PlayerPrefs.GetString("Training"));
    }
}

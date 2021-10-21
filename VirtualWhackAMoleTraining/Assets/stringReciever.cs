using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringReciever : MonoBehaviour
{

    // Input field reading
    public GameObject textToSpeech;
    private static string partNumber;
    private static char typedChar;
    private int backspaceCheck;
    public void readString (string s)
    {
        partNumber = s;
    }

    public void readChar(string s)
    {
        // check if we deleted something
        if (s.Length < backspaceCheck)
        {
            return;
        }
        typedChar = s[s.Length - 1];
        textToSpeech.SetActive(true);
        backspaceCheck = s.Length;
    }

    public static string getPartNumber()
    {
        return partNumber;
    }

    public static char getCharNumber()
    {
        return typedChar;
    }

}

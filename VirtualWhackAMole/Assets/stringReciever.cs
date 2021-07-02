using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringReciever : MonoBehaviour
{
    private static string partNumber;
    private static string oldString;
    private static char typedChar;
    public void readString (string s)
    {
        partNumber = s;
    }

    public void readChar(string s)
    {
        // first character of the string
        if (oldString == null)
        {
            oldString = s;
            typedChar = s[0];
            return;
        }
        //can find the character at the end of the new string
        int index = oldString.Length - s.Length;
        typedChar = s[index - 1];
        oldString = s;
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

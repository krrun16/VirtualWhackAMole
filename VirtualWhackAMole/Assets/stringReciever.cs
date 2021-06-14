using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringReciever : MonoBehaviour
{
    private static string partNumber;
    
    public void readString (string s)
    {
        partNumber = s;
    }

    public static string getPartNumber()
    {
        return partNumber;
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

//https://sushanta1991.blogspot.com/2015/02/how-to-write-data-to-csv-file-in-unity.html - used this website to create this script

public class CsvReadWrite : MonoBehaviour
{


    public static List<string[]> rowData = new List<string[]>();
    public static int currentRow = 0;


    // Use this for initialization
    void Start()
    {
    }

    public static void headerFields()
    {
        string[] rowDataTemp = new string[5];
        //Logging to excel document: 
        //1) where the mole emerges, 
        //2) whether the mole is hit 
        //3) if 2 is yes, then time from the mole emerging to the time it is hit
        //4) total moles hit
        rowDataTemp[0] = "Location Of Mole";
        rowDataTemp[1] = "Mole Hit?";
        rowDataTemp[2] = "Time Taken (emerge - hit)";
        rowDataTemp[3] = "Total Moles Hit";
        rowDataTemp[4] = "Score";
        rowData.Add(rowDataTemp);
    }

    public static void addRow(string location, string wasHit, double time, int total, int Score)
    {
        // 1 second = 1000 milliseconds
        double ms = time / 1000.00; //Converts time in seconds to milliseconds
        if (currentRow == 0)
        {
            headerFields();
        }
        string[] rowDataTemp = new string[5];
        rowDataTemp[0] = location;
        rowDataTemp[1] = wasHit;
        if (wasHit == "no")
        {
            rowDataTemp[2] = "X";
        }
        else
        {
            rowDataTemp[2] = ms.ToString(); // replaced time.ToString() with ms.ToString().
        }
        rowDataTemp[3] = total.ToString();
        rowDataTemp[4] = Score.ToString();
        rowData.Add(rowDataTemp);
        currentRow++;
    }

    public static void writeData() 
    {
        string[][] output = new string[rowData.Count][]; 

        for (int i = 0; i < output.Length; i++)             
            {
            output[i] = rowData[i];
            }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)       
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    public static string getPath()
    {
#if UNITY_EDITOR
        print(Application.dataPath + "/CSV/" + "Saved_data.csv");
        return Application.dataPath + "/CSV/" + "Saved_data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath + "/" + "Saved_data.csv";
#endif
    }


}


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


    // Used for file name creation
    public static string partNumber = stringReciever.getPartNumber();
    public static string hintType = PlayerPrefs.GetString("HintType");
    public static string domHand = PlayerPrefs.GetString("DominantHand");
    public static string time = System.DateTime.UtcNow.ToLocalTime().ToString("MM_dd hh_mm");

    // Use this for initialization
    void Start()
    {
    }

    public static void headerFields()
    {
        string[] rowDataTemp = new string[7];
        //Logging to excel document: 
        //1) where the mole emerges, 
        //2) whether the mole is hit 
        //3) if 2 is yes, then time from the mole emerging to the time it is hit
        //4) total moles hit
        //5) hand that hit the mole
        rowDataTemp[0] = "Level";
        rowDataTemp[1] = "Mole Hit?";
        rowDataTemp[2] = "Time Taken (emerge - hit)";
        rowDataTemp[3] = "Total Moles Hit";
        rowDataTemp[4] = "Score";
        rowDataTemp[5] = "Hand Hit With";
        rowDataTemp[6] = "Location of Mole";
        rowData.Add(rowDataTemp);
    }

    public static void addRow(string location, string wasHit, string handHit, double time, int total, int Score, int level)
    {

        if (currentRow == 0)
        {
            headerFields();
        }
        string[] rowDataTemp = new string[7];
        rowDataTemp[0] = level.ToString();
        rowDataTemp[1] = wasHit;
        if (wasHit == "no")
        {
            rowDataTemp[2] = "X";
            rowDataTemp[5] = "X";
        }
        else
        {
            rowDataTemp[2] = time.ToString();
            rowDataTemp[5] = handHit;
        }
        rowDataTemp[3] = total.ToString();
        rowDataTemp[4] = Score.ToString();
        rowDataTemp[6] = location;
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
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
#if UNITY_EDITOR
        print(desktopPath + "\\" + partNumber + hintType + domHand + " " + time + ".csv");
        return desktopPath + "\\" + partNumber + hintType + domHand + " " + time + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath + "/" + "Saved_data.csv";
#endif
    }


}


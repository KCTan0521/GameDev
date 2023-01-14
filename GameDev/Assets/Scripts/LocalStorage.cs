using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Globalization;

public class LocalStorage : MonoBehaviour
{
    public static LocalStorage instance;
    static string path = Application.persistentDataPath + "/game-record.txt";
    static string bestGamePath = Application.persistentDataPath + "/best-game-record.txt";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    static public void WriteRecord(float distance, float time)
    {

        distance = Mathf.Round(distance);
        time = Mathf.Round(time * 100) / 100;

        string bestDistance = null;
        string bestTime = null;

        // write the record into text file
        // string path = Application.persistentDataPath + "/game-record.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(distance);
        writer.WriteLine(time);
        writer.Close();

        // string bestGamePath = Application.persistentDataPath + "/best-game-record.txt";
        
        if (File.Exists(bestGamePath))
        {
            StreamReader bestGameReader = new StreamReader(bestGamePath);
            bestDistance = bestGameReader.ReadLine();
            bestTime = bestGameReader.ReadLine();
            bestGameReader.Close();

            if (bestDistance != null && bestTime != null)
            {
                float floatBestDistance = float.Parse(bestDistance, CultureInfo.InvariantCulture.NumberFormat);
                float floatBestTime = float.Parse(bestTime, CultureInfo.InvariantCulture.NumberFormat);

                if (distance > floatBestDistance)
                {
                    floatBestDistance = distance;
                }
                if (time > floatBestTime)
                {
                    floatBestTime = time;
                }
                StreamWriter bestGameWriter = new StreamWriter(bestGamePath, false);
                bestGameWriter.WriteLine(floatBestDistance);
                bestGameWriter.WriteLine(floatBestTime);
                bestGameWriter.Close();

            }
            else
            {
                StreamWriter bestGameWriter = new StreamWriter(bestGamePath, false);
                bestGameWriter.WriteLine(distance);
                bestGameWriter.WriteLine(time);
                bestGameWriter.Close();
            }
        }
        else
        {
            StreamWriter bestGameWriter = new StreamWriter(bestGamePath, false);
            bestGameWriter.WriteLine(distance);
            bestGameWriter.WriteLine(time);
            bestGameWriter.Close();
        }


        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log("Distance  : " + reader.ReadLine());
        Debug.Log("Time : " + reader.ReadLine());

        reader.Close();
    }


    public static string[] ReadRecord()
    {        
        // string path = Application.persistentDataPath + "/game-record.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string distance = reader.ReadLine();
        string time = reader.ReadLine();
        reader.Close();


        string bestDistance = null;
        string bestTime = null;
        if (File.Exists(bestGamePath))
        {
            StreamReader bestGameReader = new StreamReader(bestGamePath, false);
            bestDistance = bestGameReader.ReadLine();
            bestTime = bestGameReader.ReadLine();
            bestGameReader.Close();
        }

        string[] data = { distance, time, bestDistance, bestTime };
        return data;
    }

}

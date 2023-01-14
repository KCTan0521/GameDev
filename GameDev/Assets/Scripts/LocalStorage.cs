using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class LocalStorage : MonoBehaviour
{
    public static LocalStorage instance;

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

        string path = Application.persistentDataPath + "/game-record.txt";
        
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(distance);
        writer.WriteLine(time);
        writer.Close();
        
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log("Distance  : " + reader.ReadLine());
        Debug.Log("Time : " + reader.ReadLine());

        reader.Close();
    }


    public static string[] ReadRecord()
    {
        
        string path = Application.persistentDataPath + "/game-record.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string distance = reader.ReadLine();
        string time = reader.ReadLine();

        reader.Close();

        string[] data = { distance, time};
        return data;
    }

}

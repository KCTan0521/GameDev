using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    // the Home Button and Restart Button do not override all back to the prefab
    
    [SerializeField]
    private GameObject distanceTravelled;

    [SerializeField]
    private GameObject timeSurvived;

    [SerializeField]
    private GameObject bestDistanceTravelled;

    [SerializeField]
    private GameObject bestTimeSurvived;


    private void Awake()
    {

    }
    private void Start()
    {
        string[] data;
        data = LocalStorage.ReadRecord();
        textDisplay(data[0], data[1], data[2], data[3]);
        
    }


    void textDisplay(string distance, string time, string bestDistance, string bestTime) {
        distanceTravelled.GetComponent<TextMeshProUGUI>().text = "Distance : " +  distance + "m";
        timeSurvived.GetComponent<TextMeshProUGUI>().text = "Time Survived : " + time + "s";

        if (bestDistance != "")
        {
            bestDistanceTravelled.GetComponent<TextMeshProUGUI>().text = "Best Distance : " + bestDistance + "m";
        }
        if (bestTime != "")
        {
            bestTimeSurvived.GetComponent<TextMeshProUGUI>().text = "Best Time Survived : " + bestTime + "s";
        }
    }


    private void Update()
    {

    }



    public void RestartGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("GamePlay");
    }

    public void GoBackHomePage()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("MainMenu");
    }
}



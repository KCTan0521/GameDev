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

    [SerializeField]
    private GameObject encourageText;

    private string[] encourageTextArr = 
        {
            "Wishing you the best of luck during today’s game",
            "When the going gets tough, the tough gets going!",
            "Play hard tonight and don't leave anything out there",
            "Let's see how far you can go",
            "Good luck and play hard",
            "The game tonight is going to be a good one",
            "Best of luck out there today!",
            "You were born to be a player. You were meant to be here. This moment is yours",
            "Have fun tonight",
            "Be sure to run a smart race and to try your hardest",
            "Put on a good show and good luck",
            "Play hard, play smart",
            "The game goes fast. Play hard and good luck",
            "Stay calm, stay focused, and try your hardest",
            "Have a great game and play hard",
            "Good luck in today's game",
            "Stay confident in your abilities and give it your all",
            "It's hard to beat a person who never gives up",
            "A champion is someone who gets up when he can’t"
        };


    private void Start()
    {
        string[] data;
        data = LocalStorage.ReadRecord();
        textDisplay(data[0], data[1], data[2], data[3]);
        encourageText.GetComponent<TextMeshProUGUI>().text = encourageTextArr[Random.Range(0, encourageTextArr.Length)];
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
        checkMobileBackButton();
    }

    private void checkMobileBackButton()
    {
        if (MainMenuController.mobileBackButtonStatus())
        {
            GoBackHomePage();
        }
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



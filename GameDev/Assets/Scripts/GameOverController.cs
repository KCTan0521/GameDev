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

    private bool isDisplayed = false;
    private void Awake()
    {

    }
    private void Start()
    {
        string[] data;
        data = LocalStorage.ReadRecord();
        textDisplay(data[0], data[1]);
        /*
        distanceTravelled.GetComponent<TextMeshProUGUI>().text += playerBehaviour.GetComponent<Transform>().position.x;
        timeSurvived.GetComponent<TextMeshProUGUI>().text += "88s";*/
    }


    void textDisplay(string distance, string time) {
        Debug.Log("Game Over Scene func call");
        distanceTravelled.GetComponent<TextMeshProUGUI>().text += distance + "m";
        timeSurvived.GetComponent<TextMeshProUGUI>().text += time + "s";
    }


    private void Update()
    {

    }



    public void RestartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void GoBackHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }
}



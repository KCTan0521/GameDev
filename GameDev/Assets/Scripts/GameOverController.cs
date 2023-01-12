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
        /*
        distanceTravelled.GetComponent<TextMeshProUGUI>().text += playerBehaviour.GetComponent<Transform>().position.x;
        timeSurvived.GetComponent<TextMeshProUGUI>().text += "88s";*/
    }

    private void OnEnable()
    {
        GamePlayController.gameOverData += textDisplay;
    }

    private void OnDisable()
    {
        GamePlayController.gameOverData -= textDisplay;
    }

    void textDisplay(float distance, float time) {
        Debug.Log("Game Over Scene func call");
        distanceTravelled.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", distance) + "m";
        timeSurvived.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", time) + "s";
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



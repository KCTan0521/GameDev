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

    private PlayerBehaviour playerBehaviour;


    private void Start()
    {
        // playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        distanceTravelled.GetComponent<TextMeshProUGUI>().text += playerBehaviour.GetComponent<Transform>().position.x;
        timeSurvived.GetComponent<TextMeshProUGUI>().text += "88s";
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

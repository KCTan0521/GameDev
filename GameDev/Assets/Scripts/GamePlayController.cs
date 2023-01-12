using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    private bool isGamePaused;
    private PlayerBehaviour playerBehaviour;
    private GameObject[] gameSetting;
    
    void Awake()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        gameSetting = GameObject.FindGameObjectsWithTag("GameSetting");

    }

    void Start()
    {
        gameSettingStatus(false);
        playerBehaviour.enabled = true;
        isGamePaused = false;
        Time.timeScale = 1;
    }

    void gameSettingStatus(bool status)
    {
        foreach (GameObject gs in gameSetting)
        {
            gs.SetActive(status);
        }
    }

    public void PauseGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1;
            isGamePaused = false;
            playerBehaviour.enabled = true;
            gameSettingStatus(false);
        }
        else
        {
            Time.timeScale = 0;
            isGamePaused = true;
            playerBehaviour.enabled = false;
            gameSettingStatus(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoBackHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

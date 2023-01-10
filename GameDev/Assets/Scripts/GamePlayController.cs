using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    private bool isGamePaused = false;
    private PlayerBehaviour playerBehaviour;
    private GameObject gameSetting;
    private void Awake()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        gameSetting = GameObject.FindGameObjectWithTag("GameSetting");
    }

    public void PauseGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1;
            isGamePaused = false;
            playerBehaviour.enabled = true;
            gameSetting.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            isGamePaused = true;
            playerBehaviour.enabled = false;
            gameSetting.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScreen");
    }

    public void GoBackHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

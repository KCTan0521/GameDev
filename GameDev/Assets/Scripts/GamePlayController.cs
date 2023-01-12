using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    private bool isGamePaused;
    private PlayerBehaviour playerBehaviour;
    private GameObject[] gameSetting;

    public delegate void gameOver(float distanceTraveller, float timeSurvived);
    public static event gameOver gameOverData;

    void Awake()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        gameSetting = GameObject.FindGameObjectsWithTag("GameSetting");
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
        DontDestroyOnLoad(gameObject);
        
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
        
        SceneManager.LoadScene("GameOver"); 
    }


    void executeGameOverData()
    {
        Debug.Log("hello Im");
        if (gameOverData != null)
        {
            gameOverData(playerBehaviour.transform.position.x, Time.realtimeSinceStartup);
        }
    }

    private void Update()
    {
        executeGameOverData();
    }
}

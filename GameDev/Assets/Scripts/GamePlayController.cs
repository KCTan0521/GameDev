using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public GameObject pauseButton;
    public Sprite unPauseUI;
    public Sprite pauseUI;

    private bool isGamePaused;
    private PlayerBehaviour playerBehaviour;
    private GameObject[] gameSetting;
    private float startTime;

    private void OnEnable()
    {
        Health.echoGameOver += gameOver;
        MonsterChasingMob.echoEnterBossMode += bossMode;
    }

    private void OnDisable()
    {
        Health.echoGameOver -= gameOver;
        MonsterChasingMob.echoEnterBossMode -= bossMode;
    }


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
        startTime = Time.time;
        pauseButton.GetComponent<Image>().sprite = pauseUI;
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
            FindObjectOfType<AudioManager>().Play("Menu - Button1");
            Time.timeScale = 1;
            isGamePaused = false;
            playerBehaviour.enabled = true;
            gameSettingStatus(false);
            pauseButton.GetComponent<Image>().sprite = pauseUI;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Menu - Button2");
            Time.timeScale = 0;
            isGamePaused = true;
            playerBehaviour.enabled = false;
            gameSettingStatus(true);
            pauseButton.GetComponent<Image>().sprite = unPauseUI;
        }
    }

    public void RestartGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button2");
        FindObjectOfType<AudioManager>().Play("Song2");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoBackHomePage()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button2");
        SceneManager.LoadScene("MainMenu"); 
    }

    public void gameOver()
    {
        LocalStorage.WriteRecord(playerBehaviour.transform.position.x, Time.time - startTime);
        SceneManager.LoadScene("GameOver");
    }

    void bossMode()
    {
        Debug.Log("Enter Boss Mode");
    }
}

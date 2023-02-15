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
    public GameObject flashScreen;
    public float animationDelayTime;

    private bool isGamePaused;
    private PlayerBehaviour playerBehaviour;
    private MonsterChasingMob chaseMob; 
    private GameObject[] gameSetting;
    private float startTime;
    private int flashScreenColorValue = 0;
    private bool isEnterBossMode = false;


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
        chaseMob = GameObject.FindObjectOfType<MonsterChasingMob>();
        gameSetting = GameObject.FindGameObjectsWithTag("GameSetting");
       
    }

    void Start()
    {
        gameSettingStatus(false);
        playerBehaviour.enabled = true;
        chaseMob.enabled = true;
        isGamePaused = false;
        Time.timeScale = 1;
        startTime = Time.time;
        pauseButton.GetComponent<Image>().sprite = pauseUI;
        isEnterBossMode = false;
    }

    void gameSettingStatus(bool status)
    {
        foreach (GameObject gs in gameSetting)
        {
            gs.SetActive(status);
        }
    }

    public void PauseGameBackup()
    {
        if (isGamePaused)
        {
            FindObjectOfType<AudioManager>().Play("Menu - Button1");
            Time.timeScale = 1;
            isGamePaused = false;
            playerBehaviour.enabled = true;
            chaseMob.enabled = true;
            gameSettingStatus(false);
            pauseButton.GetComponent<Image>().sprite = pauseUI;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Menu - Button2");
            Time.timeScale = 0;
            isGamePaused = true;
            playerBehaviour.enabled = false;
            chaseMob.enabled = false;
            gameSettingStatus(true);
            pauseButton.GetComponent<Image>().sprite = unPauseUI;
        }
    }

    void PauseGame()
    {
        


        if (isGamePaused)
        {
            FindObjectOfType<AudioManager>().Play("Menu - Button1");
            Time.timeScale = 1;
            isGamePaused = false;

            if (isEnterBossMode)
            {
                playerBehaviour.enabled = false;
                chaseMob.enabled = false;
            }
            else
            {
                playerBehaviour.enabled = true;
                chaseMob.enabled = true;
            }
            gameSettingStatus(false);
            pauseButton.GetComponent<Image>().sprite = pauseUI;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Menu - Button2");
            Time.timeScale = 0;
            isGamePaused = true;
            playerBehaviour.enabled = false;
            chaseMob.enabled = false;
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
        if (!isEnterBossMode)
        {
            isEnterBossMode = true;
            pauseGameBossMode();
            Debug.Log("Enter Boss Mode");

            StartCoroutine(animationTimeDelay(animationDelayTime));

        }

    }

    void pauseGameBossMode()
    {
        if (isEnterBossMode)
        {
            // stop the previous game play
            // Time.timeScale = 0;
            playerBehaviour.enabled = false;
            chaseMob.enabled = false;
        }
        else
        {
            // resume the previous game play
            playerBehaviour.enabled = true;
            chaseMob.enabled = true;
        }
    }



    IEnumerator animationTimeDelay(float waitTime)
    {
        // preferred waitTime = 0.004
        flashScreenColorValue = 0;
        flashScreen.GetComponent<Image>().color = new Color32((byte)flashScreenColorValue, (byte)flashScreenColorValue, (byte)flashScreenColorValue, 255);
        for (int cycle = 0; cycle < 3; cycle++)
        {
            
            for (flashScreenColorValue = 0; flashScreenColorValue <= 255; flashScreenColorValue += 10)
            {
                
                flashScreen.GetComponent<Image>().color = new Color32((byte)flashScreenColorValue, (byte)flashScreenColorValue, (byte)flashScreenColorValue, 255);
                yield return new WaitForSeconds(waitTime);
                
            }

            for (flashScreenColorValue = 255; flashScreenColorValue >= 0; flashScreenColorValue -= 10)
            {
                flashScreen.GetComponent<Image>().color = new Color32((byte)flashScreenColorValue, (byte)flashScreenColorValue, (byte)flashScreenColorValue, 255);
                yield return new WaitForSeconds(waitTime);
            }
        }
        StartCoroutine(bossModeGamePlay());
                
    }

    IEnumerator bossModeGamePlay()
    {
        // your boss mode code goes here
        yield return new WaitForSeconds(0.5f);




        // the following code is called after the boss mode end
        Debug.Log("end animation boss mode");
        isEnterBossMode = false;
        pauseGameBossMode();
        chaseMob.resetMonsterPlayerDistance();
        flashScreen.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
    }





}

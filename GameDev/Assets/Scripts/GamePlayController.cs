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
    public float distanceValue;

    [SerializeField]
    private float MIN_MOB_DISTANCE;
    [SerializeField]
    private GameObject warningScreen;
    [SerializeField]
    private float timeValueDeductRatio;
    [SerializeField]
    private float distanceToIncreaseDistanceValue;
    [SerializeField]
    private float MAX_DISTANCE_VALUE;

    private bool isGamePaused;
    private PlayerBehaviour playerBehaviour;
    
    private GameObject[] gameSetting;
    private float startTime;    
    private bool isEnterBossMode = false;   
    private static float runTimeDistanceValue;
    private float redScreenIntensity = 0f;
    private static float runTimeTimeValueDeductRatio;    
    private float DISTANCE_TO_START_ALERT;    
    private Animator flashScreenAnim;
    private const string FLASH_ANIMATION = "Flash";

    private void OnEnable()
    {
        Health.echoGameOver += gameOver;
    }

    private void OnDisable()
    {
        Health.echoGameOver -= gameOver;
    }

    private void Awake()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        
        gameSetting = GameObject.FindGameObjectsWithTag("GameSetting");

        flashScreenAnim = flashScreen.GetComponent<Animator>();
    }

    private void Start()
    {
        gameSettingStatus(false);
        playerBehaviour.enabled = true;
        isGamePaused = false;
        Time.timeScale = 1;
        startTime = Time.time;
        pauseButton.GetComponent<Image>().sprite = pauseUI;
        isEnterBossMode = false;
        setFlashScreenColor(0, 0, 0, 0);
        resetDistanceValue();
        setWarningScreenColor(255, 0, 0, 0);
        runTimeTimeValueDeductRatio = timeValueDeductRatio;
        DISTANCE_TO_START_ALERT = runTimeDistanceValue * 0.7f;
        flashScreenAnim.SetBool(FLASH_ANIMATION, false);
    }

    private void Update()
    {
        distancePlayerAlert();
        checkMaxDistanceValue();
        checkMobileBackButton();
    }



    // GAME MENU RELATED    
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

    private void checkMobileBackButton()
    {
        if (MainMenuController.mobileBackButtonStatus())
        {
            PauseGame();
        }
    }

    void gameSettingStatus(bool status)
    {
        foreach (GameObject gs in gameSetting)
        {
            gs.SetActive(status);
        }
    }



    // BOSS MODE RELATED
    void checkIsBossMode()
    {
        if (!isEnterBossMode)
        {
            isEnterBossMode = true;
            Debug.Log("Enter Boss Mode");
            StartCoroutine(animationTimeDelay(animationDelayTime, 1));
        }
    }

    void bossModeGamePlay()
    {
        GameObject.Find("MainCamera").GetComponent<ChasingMobSpawner>().SpawnChasingMob();
        
        // the following code is called after the boss mode is called
        Debug.Log("end animation boss mode");
        isEnterBossMode = false;
        setFlashScreenColor(0,0,0,0);
        setWarningScreenColor(255, 0, 0, 0);
        resetDistanceValue();
    }



    // DISTANCE VALUE RELATED
    public static void changeDistanceValueBy(float value, bool isTimeValue = false)
    {
        if (isTimeValue)
        {
            // the value is given based on time
            // exp: from the wehman struggle & strangle
            // need special calculation to deduct the distance
            
            value = value * -1 * runTimeTimeValueDeductRatio;
            runTimeDistanceValue += value;

            Debug.Log("time distance value : " + runTimeDistanceValue + "\nmodify : " + value);
        }
        else
        {
            // it will add or deduct the distance 
            // based on the value (negative or positive)

            runTimeDistanceValue += value;
            Debug.Log("distance value : " + runTimeDistanceValue + "\nmodify : " + value);
        }
    }
    
    void distancePlayerAlert()
    {
        // this will change the intensity of red screen,
        // depending on the distance value

        // this will call the boss mode function when the distance value <= 0

        // Debug.Log("Mob & Player distance : " + runTimeDistanceValue);
        
        if (runTimeDistanceValue <= MIN_MOB_DISTANCE)
        {
            setWarningScreenColor(255, 0, 0, 100);
            checkIsBossMode();
        }

        else if (runTimeDistanceValue <= DISTANCE_TO_START_ALERT)
        {
            redScreenIntensity = 1 - (runTimeDistanceValue / DISTANCE_TO_START_ALERT);
            redScreenIntensity = Mathf.Round(redScreenIntensity * 100);
            if (redScreenIntensity > 255)
            {
                setWarningScreenColor(255, 0, 0, 255);
            }
            else
            {

                setWarningScreenColor(255, 0, 0, (int)redScreenIntensity);
            }

        }
        
    }

    public void checkMaxDistanceValue()
    {
        if (runTimeDistanceValue > MAX_DISTANCE_VALUE)
        {
            runTimeDistanceValue = MAX_DISTANCE_VALUE;
            Debug.Log("Reach Max Distance Distance, reset to : " + runTimeDistanceValue);
        }
    }

    public void resetDistanceValue()
    {
        runTimeDistanceValue = distanceValue;
        Debug.Log("Reset distance value");
    }


    // SCREEN & COLOR RELATED
    void setWarningScreenColor(int red, int green, int blue, int transparency)
    {
        warningScreen.GetComponent<Image>().color = new Color32((byte)red, (byte)green, (byte)blue, (byte)transparency);
    }
    
    void setFlashScreenColor(int red, int green, int blue, int transparency)
    {
        flashScreen.GetComponent<Image>().color = new Color32((byte)red, (byte)green, (byte)blue, (byte)transparency);
    }

    IEnumerator animationTimeDelay(float waitTime, int selection)
    {
        // start flash animation        
        flashScreenAnim.SetBool(FLASH_ANIMATION, true);
        yield return new WaitForSeconds(waitTime);

        // end flash animation
        flashScreenAnim.SetBool(FLASH_ANIMATION, false);
        switch (selection)
        {
            case 1:
                bossModeGamePlay();
                break;
            case 2:

                break;
            default:
                break;
        }
    }

}

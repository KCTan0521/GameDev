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
    private float DISTANCE_TO_START_ALERT;
    [SerializeField]
    private float timeValueDeductRatio;


    private bool isGamePaused;
    private PlayerBehaviour playerBehaviour;
    
    private GameObject[] gameSetting;
    private float startTime;
    private int flashScreenColorValue = 0;
    private bool isEnterBossMode = false;
    private int flashScreenTransparency = 255;
    private static float runTimeDistanceValue;
    private float redScreenIntensity = 0f;
    private static float runTimeTimeValueDeductRatio;

    private void OnEnable()
    {
        Health.echoGameOver += gameOver;
    }

    private void OnDisable()
    {
        Health.echoGameOver -= gameOver;
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
        isEnterBossMode = false;
        setFlashScreenColor(0, 0, 0, 0);
        resetDistanceValue();
        setWarningScreenColor(255, 0, 0, 0);
        runTimeTimeValueDeductRatio = timeValueDeductRatio;
    }

    private void Update()
    {
        distancePlayerAlert();
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

        if (runTimeDistanceValue <= DISTANCE_TO_START_ALERT)
        {
            redScreenIntensity = 1 - (runTimeDistanceValue / DISTANCE_TO_START_ALERT);
            redScreenIntensity = Mathf.Round(redScreenIntensity * 100);
            setWarningScreenColor(255, 0, 0, (int)redScreenIntensity);

        }
    }

    void setWarningScreenColor(int red, int green, int blue, int transparency)
    {
        warningScreen.GetComponent<Image>().color = new Color32((byte)red, (byte)green, (byte)blue, (byte)transparency);
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




    void checkIsBossMode()
    {
        if (!isEnterBossMode)
        {
            isEnterBossMode = true;
            pauseGameForBossMode();
            Debug.Log("Enter Boss Mode");

            StartCoroutine(animationTimeDelay(animationDelayTime));
        }
    }

    IEnumerator animationTimeDelay(float waitTime)
    {
        // preferred waitTime = 0.004
        flashScreenColorValue = 0;
        setFlashScreenColor(flashScreenColorValue, flashScreenColorValue, flashScreenColorValue, flashScreenTransparency);
        for (int cycle = 0; cycle < 3; cycle++)
        {
            
            for (flashScreenColorValue = 0; flashScreenColorValue <= 255; flashScreenColorValue += 10)
            {

                setFlashScreenColor(flashScreenColorValue, flashScreenColorValue, flashScreenColorValue, flashScreenTransparency);
                yield return new WaitForSeconds(waitTime);
                
            }

            for (flashScreenColorValue = 255; flashScreenColorValue >= 0; flashScreenColorValue -= 10)
            {
                setFlashScreenColor(flashScreenColorValue, flashScreenColorValue, flashScreenColorValue, flashScreenTransparency);
                yield return new WaitForSeconds(waitTime);
            }
        }
        StartCoroutine(bossModeGamePlay());
                
    }

    void setFlashScreenColor(int red, int green, int blue, int transparency)
    {
        flashScreen.GetComponent<Image>().color = new Color32((byte) red, (byte)green, (byte)blue, (byte)transparency);
    }

    IEnumerator bossModeGamePlay()
    {

        // call wei zhong boss mode function

        // temperarily use wait for second to replace
        yield return new WaitForSeconds(0.5f);




        // the following code is called after the boss mode end
        Debug.Log("end animation boss mode");
        isEnterBossMode = false;
        pauseGameForBossMode();
        setFlashScreenColor(0,0,0,0);
        resetDistanceValue();
    }

    public static void changeDistanceValueBy(float value, bool isTimeValue = false)
    {
        if (isTimeValue)
        {
            // the value is given based on time
            // exp: from the wehman struggle
            // need special calculation to deduct the distance
            
            value = value * -1 * runTimeTimeValueDeductRatio;
            runTimeDistanceValue += value;

            Debug.Log("time distance value : " + runTimeDistanceValue + "\nmodify : " + value);
        }
        else
        {
            // it will add or deduct the distance 
            // based on the value (negative or positive0

            runTimeDistanceValue += value;
            Debug.Log("distance value : " + runTimeDistanceValue + "\nmodify : " + value);
        }
    }

    public void resetDistanceValue()
    {
        runTimeDistanceValue = distanceValue;
        Debug.Log("Reset distance value");
    }

   void pauseGameForBossMode()
    {

    }

}

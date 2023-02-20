using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject messageBox;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;
    [SerializeField]
    private GameObject messageBoxText;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private int MIN_PLAY_BUTTON_TRANSPARENCY;


    private const string readTutorialText = "Do you want to read the tutorial first?";
    private const string quitGameText = "Are you sure to quit game?";
    private bool isShowingFirstTimePlayMenu = false;
    private bool isShowingQuitGameMenu = false;

    
    private void Start()
    {
        FindObjectOfType<AudioManager>().Pause("Song2");
        FindObjectOfType<AudioManager>().Play("Song1");
        messageBox.SetActive(false);
        isShowingFirstTimePlayMenu = false;
    }

    private void Update()
    {
        checkMobileBackButton();
    }

    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        if (LocalStorage.ReadIsFirstTimePlayRecord())
        {
            messageBoxText.GetComponent<TextMeshProUGUI>().text = readTutorialText;
            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(yesBtnForFirstTimePlay);
            noButton.onClick.AddListener(noBtnForFirstTimePlay);
            messageBox.SetActive(true);
            isShowingFirstTimePlayMenu = true;
            isShowingQuitGameMenu = false;
        }
        else
        {
            loadPlayGameScene();
        }
        
    }
    
    private void yesBtnForFirstTimePlay()
    {
        OpenTutorial();
    }

    private void noBtnForFirstTimePlay()
    {
        loadPlayGameScene();
    }

    private void yesBtnForExitGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        Debug.Log("Exit Game");
        Application.Quit();
    }

    private void noBtnForExitGame()
    {
        messageBox.SetActive(false);
        isShowingQuitGameMenu = false;
    }

    private void loadPlayGameScene()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        FindObjectOfType<AudioManager>().Play("Song2");
        SceneManager.LoadScene("GamePlay");
    }


    public void OpenCreditPage()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("CreditPage");
    }

    public void OpenTutorial()
    {
        LocalStorage.WriteIsFirstTimePlayRecord();
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("TutorialPlay");
    }

    public void ExitGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        messageBoxText.GetComponent<TextMeshProUGUI>().text = quitGameText;
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesBtnForExitGame);
        noButton.onClick.AddListener(noBtnForExitGame);
        messageBox.SetActive(true);
        isShowingQuitGameMenu = true;
    }


    private void checkMobileBackButton()
    {
        if (MainMenuController.mobileBackButtonStatus() && isShowingFirstTimePlayMenu == false)
        {
            if (isShowingQuitGameMenu == true)
            {
                messageBox.SetActive(false);
                isShowingQuitGameMenu = false;
            }
            else
            {
                ExitGame();
            }
            
        }
        else if (MainMenuController.mobileBackButtonStatus() && isShowingFirstTimePlayMenu == true)
        {
            // if the menu for asking whether show tutorial for first time play
            // return to main menu if back button is pressed
            messageBox.SetActive(false);
            isShowingFirstTimePlayMenu = false;
        }
        
    }

    public static bool mobileBackButtonStatus()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                return true;
            }
        }
        return false;
        

        // remove the code when processing into apk
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            return true;            
        }
        return false;
        */
    }
}

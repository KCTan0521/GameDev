using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject messageBox;
   
    private void Start()
    {
        FindObjectOfType<AudioManager>().Pause("Song2");
        FindObjectOfType<AudioManager>().Play("Song1");
        messageBox.SetActive(false);
    }

    private void Update()
    {
        checkMobileBackButton();
    }

    public void PlayGame()
    {
        // isFirstTimePlay();
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        FindObjectOfType<AudioManager>().Play("Song2");
        SceneManager.LoadScene("GamePlay");
    }

    private void isFirstTimePlay()
    {
        messageBox.SetActive(true);
    }

    public void OpenCreditPage()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("CreditPage");
    }

    public void OpenTutorial()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        Debug.Log("Exit Game");
        Application.Quit();
    }


    private void checkMobileBackButton()
    {
        if (MainMenuController.mobileBackButtonStatus())
        {
            ExitGame();
        }
    }

    public static bool mobileBackButtonStatus()
    {
        // uncomment the code when processing into apk
        /*
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                return true;
            }
        }
        return false;
        */

        // remove the code when processing into apk
        if (Input.GetKeyDown(KeyCode.B))
        {
            return true;            
        }
        return false;
    }
}

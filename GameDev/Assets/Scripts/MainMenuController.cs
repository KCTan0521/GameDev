using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
   
    private void Start()
    {
        FindObjectOfType<AudioManager>().Pause("Song2");
        FindObjectOfType<AudioManager>().Play("Song1");
    }

    private void Update()
    {
        checkMobileBackButton();
    }

    public void PlayGame()
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

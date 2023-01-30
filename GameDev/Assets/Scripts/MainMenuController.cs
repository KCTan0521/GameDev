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

}

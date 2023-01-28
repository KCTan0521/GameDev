using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Song1");
    }
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("GamePlay");
    }

    public void OpenCreditPage()
    {
        SceneManager.LoadScene("CreditPage");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

}

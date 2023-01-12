using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    // the Home Button and Restart Button do not override all back to the prefab



    private void Start()
    {

    }


    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void GoBackHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

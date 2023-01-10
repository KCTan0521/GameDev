using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    private bool isGamePaused = false;
    private PlayerBehaviour playerBehaviour;

    private void Awake()
    {
        playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
    }

    public void PauseGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1;
            isGamePaused = false;
            playerBehaviour.enabled = true;
        }
        else
        {
            Time.timeScale = 0;
            isGamePaused = true;
            playerBehaviour.enabled = false;
        }
    }

}

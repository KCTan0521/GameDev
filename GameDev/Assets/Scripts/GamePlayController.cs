using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    private bool isGamePaused = false;
    public void PauseGame()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1;
            isGamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            isGamePaused = true;
        }
    }
}

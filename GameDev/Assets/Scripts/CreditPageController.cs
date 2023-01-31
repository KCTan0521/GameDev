using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditPageController : MonoBehaviour
{
    public GameObject canvas;

    private CanvasScaler canvasScaler;
    private const float DEFAULT_SCREEN_WIDTH = 1920f;
    private float scale = 1;

    private void Awake()
    {
        canvasScaler = canvas.GetComponent<CanvasScaler>();
    }

    private void Start()
    {
        ModifyCanvasScaler();
    }

    public void BackHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void ModifyCanvasScaler()
    {
        scale = Mathf.Round(Screen.width / DEFAULT_SCREEN_WIDTH * 1000) / 1000;
        scale = scale >= 1.0f ? 1.0f : scale;
        canvasScaler.scaleFactor = scale;
    }
}

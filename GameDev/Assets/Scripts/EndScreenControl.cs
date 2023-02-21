using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenControl : MonoBehaviour
{
    public GameObject _endScreen;
    public Button m_Exit, m_Unpause;

    private void Start()
    {
        m_Exit.onClick.AddListener(GoBackHomePage);
        m_Unpause.onClick.AddListener(Unpause);
    }

    private void GoBackHomePage()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button2");
        SceneManager.LoadScene("MainMenu");
    }

    private void Unpause()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button2");
        Time.timeScale = 1;
        _endScreen.SetActive(false);
    }
}

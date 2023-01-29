using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditPageController : MonoBehaviour
{
    public void BackHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

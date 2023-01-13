using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private static bool isMute = false;

    public GameObject soundButton;
    public Sprite muteUI;
    public Sprite unmuteUI;

    public void Start()
    {
        if (isMute)
        {
            soundButton.GetComponent<Image>().sprite = muteUI;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = unmuteUI;
        }
    }

    public void OnButtonPress()
    {
        if(isMute)
        {
            AudioManager.UnMuteAllSound();
            soundButton.GetComponent<Image>().sprite = unmuteUI;
        }
        else
        {
            AudioManager.MuteAllSound();
            soundButton.GetComponent<Image>().sprite = muteUI;
        }

        isMute = !isMute;
    }

    public void OnSceneLoad()
    {
        if (isMute)
        {
            soundButton.GetComponent<Image>().sprite = muteUI;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = unmuteUI;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialController : MonoBehaviour
{

    public Object[] tutMaterial = {};

    public string[] description = {
    };

    //        "Tap to Jump\nTap twice to Double Jump"    

    public GameObject tutBoard;

    private Image tutImage;
    private VideoPlayer tutVideo;
    public TextMeshProUGUI tutText;

    public GameObject previousButton;
    public GameObject nextButton;

    // private
    public int index = 0;
    public int maxIndex = 0;
    public bool isFirstTime = true;
    public void BackHomePage()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake()
    {
        tutImage = tutBoard.GetComponent<Image>();
        tutVideo = tutBoard.GetComponent<VideoPlayer>();
        
    }

    void Start()
    {
        maxIndex = tutMaterial.Length - 1;
        previousButton.SetActive(false);
        displayTutMaterial();
    }

    public void previousSection()
    {
        enableAndDisableButton(true);
        
        displayTutMaterial();
        
    }

    public void nextSection()
    {
        enableAndDisableButton(false);
        
        displayTutMaterial();

    }

    void enableAndDisableButton(bool isPreviousButton)
    {

        if (isPreviousButton)
        {
            index--;
        }
        else
        {
            index++;
        }

        if (index == 0)
        {
            previousButton.SetActive(false);
        }
        else
        {
            previousButton.SetActive(true);
        }


        if (index == maxIndex)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
            
        }        
    }

    void displayTutMaterial()
    {
        if (tutMaterial[index].GetType() == typeof(Image))
        {
            tutVideo.enabled = false;
            tutImage.enabled = true;

            tutImage.sprite = (Sprite) tutMaterial[index];
            
        }
        if (tutMaterial[index].GetType() == typeof(VideoClip))
        {
            tutVideo.enabled = true;
            tutImage.enabled = false;

            Debug.Log(tutMaterial[index].GetType().Name);

            // tutVideo.source = tutMaterial[index];
            tutVideo.Play();
        }
        tutText.text = description[index];
    }

}

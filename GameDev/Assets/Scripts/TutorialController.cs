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

    public string[] description = {};

    public GameObject tutBoard;

    private Image tutImage;
    private VideoPlayer tutVideo;
    private SpriteRenderer spriteRender;
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
        tutVideo = GetComponent<VideoPlayer>();
        spriteRender =tutBoard.GetComponent<SpriteRenderer>();
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
        
        if (tutMaterial[index].GetType() == typeof(Texture2D))
        {
            tutVideo.enabled = false;
            spriteRender.enabled = false;
            tutImage.enabled = true;

            Texture2D sprites = (Texture2D) tutMaterial[index];
            Rect rec = new Rect(0, 0, sprites.width, sprites.height);
            tutImage.sprite = Sprite.Create(sprites, rec, new Vector2(0, 0), 1);

        }
        if (tutMaterial[index].GetType() == typeof(VideoClip))
        {

            tutVideo.enabled = true;
            spriteRender.enabled = true;
            tutImage.enabled = false;

            tutVideo.clip = (VideoClip) tutMaterial[index];
            tutVideo.Play();
        }
        tutText.text = description[index];
    }

}

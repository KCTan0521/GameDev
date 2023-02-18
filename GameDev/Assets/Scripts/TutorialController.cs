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
    public GameObject canvas;
    // private
    private int index = 0;
    private int maxIndex = 0;
    private const float DEFAULT_SCREEN_WIDTH = 1920f;
    private float scale = 1;
    private CanvasScaler canvasScaler;

    private void Update()
    {
        checkMobileBackButton();
    }

    private void checkMobileBackButton()
    {
        if (MainMenuController.mobileBackButtonStatus())
        {
            BackHomePage();
        }
    }

    public void BackHomePage()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake()
    {
        tutImage = tutBoard.GetComponent<Image>();
        tutVideo = GetComponent<VideoPlayer>();
        spriteRender = tutBoard.GetComponent<SpriteRenderer>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();
    }

    void Start()
    {
        maxIndex = tutMaterial.Length - 1;
        previousButton.SetActive(false);
        ModifyCanvasScaler();
        displayTutMaterial();
    }

    public void previousSection()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
        enableAndDisableButton(true);
        displayTutMaterial();        
    }

    public void nextSection()
    {
        FindObjectOfType<AudioManager>().Play("Menu - Button1");
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

    void ModifyCanvasScaler()
    {
        scale = Mathf.Round(Screen.width / DEFAULT_SCREEN_WIDTH * 1000) / 1000;
        scale = scale >= 1.0f ? 1.0f : scale;
        canvasScaler.scaleFactor = scale;
    }

}

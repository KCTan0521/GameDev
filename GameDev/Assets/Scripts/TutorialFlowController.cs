using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TutorialFlowController : MonoBehaviour
{
    [SerializeField] private GameObject _endScreen;
    [SerializeField] private GameObject _chasingMob;
    public Cinemachine.CinemachineVirtualCamera playerCam;
    public GameObject _player;
    public GameObject _boss;
    public GameObject slidingGiant;
    public GameObject dashGiant;
    public GameObject jumpWoman;
    public GameObject finger;
    public GameObject areaDash;
    public GameObject areaJump;
    public GameObject highlight;
    public GameObject blur;

    private GameObject[] tutorialUI;

    private bool resetControl = false;
    private bool startCutScene = false;
    private bool slideTutorial = false;
    private bool dashTutorial = false;
    private bool jumpTutorial = false;
    private bool doubleJumpTutorial = false;
    private bool struggleTutorial = false;
    private bool isBossSpawned = false;
    private bool isRegressing;
    private bool isTutorialEnded;

    private Camera mainCam;
    private CinemachineVirtualCamera player_Cam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private GameObject chasingMob;

    // Start is called before the first frame update
    void Start()
    {
        tutorialUI = GameObject.FindGameObjectsWithTag("Tutorial");
        showTutorialUI(false);
        _endScreen.SetActive(false);
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        player_Cam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = mainCam.GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
    }

    // Update is called once per frame
    void Update()
    {
        if(resetControl)
        {
            resetControl = false;
            ResetControl();
            return;
        }

        if (!startCutScene)
            ShowMonsterScene();

        if (slidingGiant != null && slidingGiant.transform.position.x <= (_player.transform.position.x + 4f) && !slideTutorial)
            SlideTutorial();

        if (dashGiant != null && dashGiant.transform.position.x <= (_player.transform.position.x + 4f) && !dashTutorial)
            DashTutorial();

        if (jumpWoman != null && jumpWoman.transform.position.x <= (_player.transform.position.x + 35.5f) && !jumpTutorial)
            JumpTutorial();

        if (jumpWoman != null && jumpWoman.transform.position.x <= (_player.transform.position.x + 33f) && !doubleJumpTutorial)
            DoubleJumpTutorial();

        if (_player.GetComponent<PlayerBehaviour>().isStrangled && !struggleTutorial)
            StruggleTutorial();

        if (struggleTutorial && _player.GetComponent<PlayerBehaviour>().isBreakFree)
            resetControl = true;

        if (_player.transform.position.x >= 150f && !isRegressing)
        {
            if (playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX < 0.5f)
            {
                playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX += Time.deltaTime / 2.5f;
            }

            else if (!isBossSpawned)
            {
                chasingMob = Instantiate(_chasingMob, new Vector2(mainCam.transform.position.x - leftScreen, 4.5f), Quaternion.identity); 
                isBossSpawned = true;
            }

            else
            {
                if (_player.transform.position.x < mainCam.transform.position.x + leftScreen - 4f)
                {
                    chasingMob.transform.position = new Vector2(mainCam.transform.position.x - leftScreen, 4.5f);
                    chasingMob.GetComponent<ChasingMobBehavior>().attackTimer = 0f;
                    _player.GetComponent<TouchDetector>().enabled = true;
                    _player.GetComponent<PlayerBehaviour>().isBossFight = true;

                    finger.GetComponent<Animator>().SetBool("dash", true);
                    showTutorialUI(true);
                    blur.SetActive(false);
                }

                else
                {
                    _player.GetComponent<PlayerBehaviour>().isBossFight = false;
                    chasingMob.GetComponent<ChasingMobBehavior>().attackTimer = 0f;
                    isRegressing = true;

                    showTutorialUI(false);
                }
            }
        }

        if (isRegressing && !isTutorialEnded)
        {
            if (playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX > 0.15f)
            {
                chasingMob.GetComponent<ChasingMobBehavior>().attackTimer = 0f;
                chasingMob.GetComponent<ChasingMobBehavior>().attackTimer = 0f;
                playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX -= Time.deltaTime / 2.5f;
            }

            else
            {
                Destroy(chasingMob);
                isTutorialEnded = true;
                _endScreen.SetActive(true);
                Time.timeScale = 0;
                showTutorialUI(false);
            }
        }
    }


    private void ShowMonsterScene()
    {
        if (playerCam.GetComponent<Transform>().position.x <= _boss.transform.position.x)
        {
            playerCam.m_Follow = _player.transform;
            Destroy(_boss);
            startCutScene = true;
        }
    }

    private void SlideTutorial()
    {
        Time.timeScale = 0;
        showTutorialUI(true);

        _player.GetComponent<TouchDetector>().enabled = true;

        if (_player.GetComponent<PlayerBehaviour>().isSliding)
        {
            Time.timeScale = 1;
            slideTutorial = true;
            showTutorialUI(false);
            resetControl = true;
        } 
        else
        {
            _player.GetComponent<PlayerBehaviour>().isDashing = false;
            _player.GetComponent<PlayerBehaviour>().canJump = false;
            _player.GetComponent<PlayerBehaviour>().canDoubleJump = false;
        }

    }

    private void DashTutorial()
    {
        Time.timeScale = 0;
        finger.GetComponent<Animator>().SetBool("dash", true);
        showTutorialUI(true);
        areaDash.SetActive(true);

        _player.GetComponent<TouchDetector>().enabled = true;

        if (_player.GetComponent<PlayerBehaviour>().isDashing)
        {
            Time.timeScale = 1;
            dashTutorial = true;
            showTutorialUI(false);
            areaDash.SetActive(false);
            resetControl = true;
        }
        else
        {
            _player.GetComponent<PlayerBehaviour>().isSliding = false;
            _player.GetComponent<PlayerBehaviour>().canJump = false;
            _player.GetComponent<PlayerBehaviour>().canDoubleJump = false;
        }
    }

    private void JumpTutorial()
    {
        Time.timeScale = 0;
        finger.GetComponent<Animator>().SetBool("jump", true);
        finger.GetComponent<Animator>().SetBool("dash", false);
        showTutorialUI(true);
        areaJump.SetActive(true);

        _player.GetComponent<TouchDetector>().enabled = true;
        if (_player.GetComponent<PlayerBehaviour>().canJump)
        {
            Time.timeScale = 1;
            jumpTutorial = true;
            showTutorialUI(false);
            areaJump.SetActive(false);
            resetControl = true;
        }
        else
        {
            _player.GetComponent<PlayerBehaviour>().isDashing = false;
            _player.GetComponent<PlayerBehaviour>().isSliding = false;
            _player.GetComponent<PlayerBehaviour>().canDoubleJump = false;
        }
    }

    private void DoubleJumpTutorial()
    {
        Time.timeScale = 0;
        finger.GetComponent<Animator>().SetBool("jump", true);
        finger.GetComponent<Animator>().SetBool("dash", false);
        showTutorialUI(true);
        areaJump.SetActive(true);

        _player.GetComponent<TouchDetector>().enabled = true;

        if (_player.GetComponent<PlayerBehaviour>().canDoubleJump)
        {
            Time.timeScale = 1;
            doubleJumpTutorial = true;
            showTutorialUI(false);
            areaJump.SetActive(false);
            resetControl = true;
        }
        else
        {
            _player.GetComponent<PlayerBehaviour>().isDashing = false;
            _player.GetComponent<PlayerBehaviour>().isSliding = false;
        }
    }

    private void StruggleTutorial()
    {
        Time.timeScale = 0;
        finger.GetComponent<Animator>().SetBool("strangle", true);
        finger.GetComponent<Animator>().SetBool("jump", false);
        finger.GetComponent<Animator>().SetBool("dash", false);
        showTutorialUI(true);
        highlight.SetActive(true);

        _player.GetComponent<TouchDetector>().enabled = true;

        if (_player.GetComponent<PlayerBehaviour>().struggleCount >= 6f) 
        { 
            Time.timeScale = 1;
            struggleTutorial = true;
            showTutorialUI(false);
            highlight.SetActive(false);
        }
    }

    private void showTutorialUI(bool status)
    {
        foreach (GameObject tutorialObject in tutorialUI)
        {
            tutorialObject.SetActive(status);
        }
    }

    private void ResetControl()
    {
        _player.GetComponent<TouchDetector>().slideButton = false;
        _player.GetComponent<TouchDetector>().dashButton = false;
        _player.GetComponent<TouchDetector>().sprintButton = false;
        _player.GetComponent<TouchDetector>().jumpButton = false;
        _player.GetComponent<TouchDetector>().enabled = false;
    }
}

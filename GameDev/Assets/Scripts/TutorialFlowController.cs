using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlowController : MonoBehaviour
{

    public Cinemachine.CinemachineVirtualCamera playerCam;
    public GameObject _player;
    public GameObject _boss;
    public GameObject slidingGiant;
    public GameObject dashGiant;
    public GameObject jumpWoman;
    public GameObject finger;
    public GameObject areaDash;
    public GameObject areaJump;

    private GameObject[] tutorialUI;

    private bool resetControl = false;
    private bool startCutScene = false;
    private bool slideTutorial = false;
    private bool dashTutorial = false;
    private bool jumpTutorial = false;
    private bool doubleJumpTutorial = false;
    private bool struggleTutorial = false;


    // Start is called before the first frame update
    void Start()
    {
        tutorialUI = GameObject.FindGameObjectsWithTag("Tutorial");
        showTutorialUI(false);
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

        if (jumpWoman != null && jumpWoman.transform.position.x <= (_player.transform.position.x + 20.5f) && !jumpTutorial)
            JumpTutorial();

        if (jumpWoman != null && jumpWoman.transform.position.x <= (_player.transform.position.x + 19f) && !doubleJumpTutorial)
            DoubleJumpTutorial();

        if (_player.GetComponent<PlayerBehaviour>().isStrangled && !struggleTutorial)
            StruggleTutorial();
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
    }

    private void StruggleTutorial()
    {
        Time.timeScale = 0;
        finger.GetComponent<Animator>().SetBool("dash", true);
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

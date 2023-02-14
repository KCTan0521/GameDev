using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ChasingMobSpawner : MonoBehaviour
{
    private Camera mainCam;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    public Button m_Button;

    void Start()
    {
        mainCam = this.GetComponent<Camera>();
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
        m_Button.onClick.AddListener(SpawnChasingMob);
    }

    void Update()
    {

    }

    void SpawnChasingMob()
    {
        PlayerPosShift();
        ClearMobs();
    }

    void PlayerPosShift()
    {
        playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
    }

    void ClearMobs()
    {
        gameObject.GetComponent<MonsterController>().enabled = false;
        foreach (GameObject giant in GameObject.FindGameObjectsWithTag("Monster"))
        {
            Destroy(giant);
        }

        foreach (GameObject wahmen in GameObject.FindGameObjectsWithTag("Wahmen"))
        {
            Destroy(wahmen);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }
    }
}

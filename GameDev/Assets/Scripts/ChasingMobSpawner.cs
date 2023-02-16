using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ChasingMobSpawner : MonoBehaviour
{
    [SerializeField] GameObject _chasingMob;
    private GameObject chasingMob;
    private Camera mainCam;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;

    private void Start()
    {
        mainCam = this.GetComponent<Camera>();
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SpawnChasingMob();
        }
    }

    private void SpawnChasingMob()
    {
        GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBossFight = true;
        PlayerPosShift();
        ClearMobs();
        ChasingMobPos();
    }

    private void PlayerPosShift()
    {
        playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
    }

    private void ClearMobs()
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

    private void ChasingMobPos()
    {
        Instantiate(_chasingMob, new Vector2(mainCam.transform.position.x - leftScreen, 0f), Quaternion.identity);
    }
}

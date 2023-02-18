using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ChasingMobSpawner : MonoBehaviour
{
    [SerializeField] GameObject _chasingMob;
    private PlayerBehaviour _player;
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
        _player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_player.isBossFight)
        {
            SpawnChasingMob();
        }
    }

    private void SpawnChasingMob()
    {
        PlayerPosShift();
        ClearMobs();
        ChasingMobPos();
        GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBossFight = true;
        GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled = false;
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
        Instantiate(_chasingMob, new Vector2(mainCam.transform.position.x - leftScreen, 4.5f), Quaternion.identity);
    }
}

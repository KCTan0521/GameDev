using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ChasingMobSpawner : MonoBehaviour
{
    [SerializeField] GameObject _chasingMob;
    private GameObject chasingMob;
    private PlayerBehaviour _player;
    private Camera mainCam;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    public bool isRegressing;

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

        if (_player.isBossFight)
        {
            chasingMob.transform.position = new Vector2(mainCam.transform.position.x - leftScreen - 2f, 4.5f);
        }

        if (!_player.isBossFight && isRegressing)
        {
            chasingMob.transform.position = new Vector2(chasingMob.transform.position.x, chasingMob.transform.position.y);

            foreach (GameObject wave in GameObject.FindGameObjectsWithTag("Wave"))
            {
                Destroy(wave);
            }

            foreach (GameObject wind in GameObject.FindGameObjectsWithTag("Wind"))
            {
                Destroy(wind);
            }

            if (playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX > 0.15f)
            {
                playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX -= Time.deltaTime / 2.5f;
            }

            else
            {
                playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.15f;
                gameObject.GetComponent<MonsterController>().enabled = true;
                Destroy(chasingMob);
                isRegressing = false;
            }
        }
    }

    public void SpawnChasingMob()
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
            _player.isBeingAttacked = false;
            Destroy(wahmen);
        }

        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }
    }

    private void ChasingMobPos()
    {
        chasingMob = Instantiate(_chasingMob, new Vector2(mainCam.transform.position.x - leftScreen, 4.5f), Quaternion.identity);
    }
}

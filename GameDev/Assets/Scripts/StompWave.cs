using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StompWave : MonoBehaviour
{
    private Rigidbody2D _wave;
    private Rigidbody2D _player;
    private Camera mainCam;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;

    private void Start()
    {
        _wave = this.GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GameObject.Find("MainCamera").GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
    }

    private void Update()
    {
        if (this.transform.position.x > mainCam.transform.position.x + leftScreen)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            this.GetComponent<EdgeCollider2D>().isTrigger = enabled;
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().isHitByBoss = true;
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().slowTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        _wave.velocity = new Vector2(20f, 0f);
    }
}

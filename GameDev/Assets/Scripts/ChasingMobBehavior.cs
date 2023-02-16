using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChasingMobBehavior : MonoBehaviour
{
    [SerializeField] GameObject _stompAttack;
    private Rigidbody2D _chasingMob;
    private Camera mainCam;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private int stompCount;
    private int randStomp;
    private float stompTimer;
    private bool isAttacking;

    void Start()
    {
        _chasingMob = this.GetComponent<Rigidbody2D>();
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GameObject.Find("MainCamera").GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
        isAttacking = false;
        stompCount = 0;
    }

    void Update()
    {
        _chasingMob.transform.position = new Vector2(mainCam.transform.position.x - leftScreen - 2f, 4.5f);
        Attack();
    }

    void Attack()
    {
        StompAttack();
    }

    void StompAttack()
    {
        if (!isAttacking)
        {
            int[] stomp = { 3, 4, 5 };
            System.Random rnd = new System.Random();
            int randIndex = rnd.Next(stomp.Length);
            randStomp = stomp[randIndex];
            isAttacking = true;
        }

        if (isAttacking)
        {
            stompTimer += Time.deltaTime;

            if (stompCount >= randStomp)
            {
                stompCount = 0;
                isAttacking = false;
            }

            else if (stompTimer > 1f)
            {
                stompCount += 1;
                Instantiate(_stompAttack, new Vector2(mainCam.transform.position.x - leftScreen, 1.5f), Quaternion.identity);
                stompTimer = 0f;
            }
        }
    }
}

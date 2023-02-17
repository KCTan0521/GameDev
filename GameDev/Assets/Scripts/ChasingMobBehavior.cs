using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChasingMobBehavior : MonoBehaviour
{
    [SerializeField] GameObject _stompAttack;
    private Rigidbody2D _chasingMob;
    private Rigidbody2D _player;
    private Camera mainCam;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private float warmupTimer;
    private int stompCount;
    private int randStomp;
    private float stompTimer;
    private float suckTimer;
    private float suckLength;
    private bool isAttacking;
    private bool suckTargetSet;
    private float attackRangeMax;
    private float attackRangeMin;

    private void Start()
    {
        _chasingMob = this.GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        orthoSize = playerCam.m_Lens.OrthographicSize;
        aspectRatio = GameObject.Find("MainCamera").GetComponent<Camera>().aspect;
        leftScreen = orthoSize * aspectRatio;
        warmupTimer = 0f;
        isAttacking = false;
        stompCount = 0;
    }

    private void Update()
    {
        _chasingMob.transform.position = new Vector2(mainCam.transform.position.x - leftScreen - 2f, 4.5f);
        Attack();
    }

    private void Attack()
    {
        warmupTimer += Time.deltaTime;

        if (warmupTimer >= 1f)
        {
            if (_player.position.x < mainCam.transform.position.x - leftScreen + 4f)
            {
                GameObject.Find("Player").GetComponent<Health>().Damage(3f);
            }
            SuckAttack();
        }
    }

    private void StompAttack()
    {
        if (!isAttacking)
        {
            int[] stomp = { 3, 4, 5 };
            System.Random rnd = new System.Random();
            int randIndex = rnd.Next(stomp.Length);
            randStomp = stomp[randIndex];
            isAttacking = true;
        }

        else if (isAttacking)
        {
            stompTimer += Time.deltaTime;

            if (stompCount >= randStomp)
            {
                stompCount = 0;
                isAttacking = false;
            }

            else if (stompTimer > 2f)
            {
                stompCount += 1;
                Instantiate(_stompAttack, new Vector2(mainCam.transform.position.x - leftScreen + 2f, 1.75f), Quaternion.identity);
                stompTimer = 0f;
            }
        }
    }

    private void SuckAttack()
    {
        if (!isAttacking)
        {
            suckTimer += Time.deltaTime;

            if (suckTimer >= 0.5f && !suckTargetSet)
            {
                attackRangeMax = _player.transform.position.y + 1.5f;
                attackRangeMin = _player.transform.position.y - 1.5f;
                suckTargetSet = true;
            }

            if (suckTimer >= 1f)
            {
                isAttacking = true;
                suckTimer = 0f;
                suckTargetSet = false;
            }
        }

        else if (isAttacking)
        {
            if (suckLength < 3f && _player.transform.position.y <= attackRangeMax && _player.transform.position.y >= attackRangeMin)
            {
                suckLength += Time.deltaTime;
                GameObject.Find("Player").GetComponent<PlayerBehaviour>().isSuckedByBoss = true;
            }

            else if (suckLength < 3f && (_player.transform.position.y >= attackRangeMax || _player.transform.position.y <= attackRangeMin))
            {
                suckLength += Time.deltaTime;
                GameObject.Find("Player").GetComponent<PlayerBehaviour>().isSuckedByBoss = false;
            }

            else
            {
                suckLength = 0f;
                isAttacking = false;
                GameObject.Find("Player").GetComponent<PlayerBehaviour>().isSuckedByBoss = false;
            } 
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChasingMobBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _stompAttack;
    [SerializeField] private GameObject _windPressure;
    private GameObject windPressure;
    private Rigidbody2D _chasingMob;
    private Rigidbody2D _player;
    private Camera mainCam;
    private CinemachineVirtualCamera playerCam;
    private float leftScreen;
    private float orthoSize;
    private float aspectRatio;
    private float warmupTimer;
    private float attackTimer;
    private int randAttack;
    private int stompCount;
    private int randStomp;
    private float stompTimer;
    private bool isStomped;
    private float suckTimer;
    private float suckLength;
    private bool isAttacking;
    private bool suckTargetSet;
    private bool suckAttack;
    private float attackRangeMax;
    private float attackRangeMin;
    private float windPressureYPos;
    public Animator animator;

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
        warmupTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (warmupTimer >= 1f)
        {
            if (_player.position.x < mainCam.transform.position.x - leftScreen + 4f)
            {
                GameObject.Find("Player").GetComponent<Health>().Damage(3f);
            }

        }

        if (attackTimer >= 2f)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            int[] attackType = { 1, 2 };
            System.Random rnd = new System.Random();
            int randIndex = rnd.Next(attackType.Length);
            randAttack = attackType[randIndex];
            
            if (randAttack == 1)
            {
                StompAttack();
            }

            else
            {
                SuckAttack();
            }
        }

        else if (isAttacking)
        {
            if (randAttack == 1)
            {
                StompAttack();
            }
            else
            {
                SuckAttack();
            }
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
                attackTimer = 0f;
            }

            if (stompTimer >= 1.25f && !isStomped)
            {
                animator.SetBool("isStomping", true);
                isStomped = true;
            }

            if (stompTimer > 2f)
            {
                stompCount += 1;
                Instantiate(_stompAttack, new Vector2(mainCam.transform.position.x - leftScreen + 2f, 1.75f), Quaternion.identity);
                stompTimer = 0f;
                animator.SetBool("isStomping", false);
                isStomped = false;
            }
        }
    }

    private void SuckAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isSucking", true);
        }

        else if (isAttacking)
        {
            if (!suckAttack)
            {
                suckTimer += Time.deltaTime;

                if (suckTimer >= 1f && !suckTargetSet)
                {
                    attackRangeMax = _player.transform.position.y + 1.5f;
                    attackRangeMin = _player.transform.position.y - 1.5f;
                    windPressureYPos = _player.transform.position.y;
                    suckTargetSet = true;
                }

                if (suckTimer >= 1.5f)
                {
                    suckTimer = 0f;
                    suckTargetSet = false;
                    suckAttack = true;
                    windPressure = Instantiate(_windPressure, new Vector2(_player.transform.position.x - 1f, windPressureYPos), Quaternion.identity);
                }
            }
            
            if (suckAttack)
            {
                if (suckLength < 2f)
                {
                    windPressure.transform.position = new Vector2(_player.transform.position.x - 1f, windPressureYPos);
                    if (_player.transform.position.y <= attackRangeMax && _player.transform.position.y >= attackRangeMin)
                    {
                        suckLength += Time.deltaTime;
                        GameObject.Find("Player").GetComponent<PlayerBehaviour>().isSuckedByBoss = true;
                    }
                    else
                    {
                        suckLength += Time.deltaTime;
                        GameObject.Find("Player").GetComponent<PlayerBehaviour>().isSuckedByBoss = false;
                    }
                }

                else
                {
                    suckLength = 0f;
                    isAttacking = false;
                    animator.SetBool("isSucking", false);
                    attackTimer = 0f;
                    GameObject.Find("Player").GetComponent<PlayerBehaviour>().isSuckedByBoss = false;
                    suckAttack = false;
                }
            } 
        }        
    }

    private void StompSound()
    {
        FindObjectOfType<AudioManager>().Play("Boss - Stomp");
    }
}

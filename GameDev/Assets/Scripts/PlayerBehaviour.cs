using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask ground;
    [SerializeField] private BoxCollider2D _col;
    [SerializeField] private BoxCollider2D standing;
    [SerializeField] private BoxCollider2D sliding;
    private CinemachineVirtualCamera playerCam;
    public Animator animator;
    public float moveSpeed = 10f;
    private float targetedSpeed;
    public float jumpForce = 10f;
    public float dashTranslate = 10f;
    public float slideDuration = 1f;

    private float groundAndTopCheck = 0.2f;
    private float firstTap;
    public bool canJump;
    public bool canDoubleJump;
    private int jumpCount;
    public bool isDashing;
    public bool isSliding;
    public bool isStrangled;
    private float struggleDuration = 0;
    public float struggleCount = 0;
    public bool isBeingAttacked;
    public bool isBreakFree;
    public bool pull;
    public bool isPulled;
    public bool isPulling;
    public bool isBossFight;
    private bool isCameraShift;
    private float shiftTimer;

    private bool isJumpBoost = false;
    private float jumpBoostDuration = 0;
    private bool isHealthRegen = false;
    private float healthRegenDuration = 0;
    private float addHealthDuration = 0;

    private bool jumpButton;
    private bool dashButton;
    private bool slideButton;
    private bool sprintButton;

    public bool isHitByBoss;
    public float slowTimer;
    private bool isPlunged;
    public bool isSuckedByBoss;
    private bool isSprinting;
    private float sprintTimer;

    private Rigidbody2D _rb;
    private float playerStamina;

    private void Awake()
    {
        gameObject.AddComponent<Stamina>();
        gameObject.AddComponent<Health>();
        gameObject.AddComponent<TouchDetector>();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        isBossFight = false;
        targetedSpeed = moveSpeed;
        standing.enabled = true;
        sliding.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        jumpButton = gameObject.GetComponent<TouchDetector>().jumpButton;
        dashButton = gameObject.GetComponent<TouchDetector>().dashButton;
        slideButton = gameObject.GetComponent<TouchDetector>().slideButton;
        sprintButton = gameObject.GetComponent<TouchDetector>().sprintButton;
        playerStamina = gameObject.GetComponent<Stamina>().stamina;

        IsDashing();
        IsGrounded();
        gameObject.GetComponent<Health>().PalpitatingAnimation();

        if (isStrangled)
        {
            struggleDuration += Time.deltaTime;
            GamePlayController.changeDistanceValueBy(Time.deltaTime, true);
            if (struggleCount <= 0f)
            {
                pull = true;
                if (isPulled)
                {
                    isPulled = false;
                    isPulling = true;
                    isStrangled = false;
                    Physics2D.IgnoreLayerCollision(6, 10, true);
                    GamePlayController.changeDistanceValueBy(-10f);
                    animator.SetBool("IsTangled", false);
                }
            }

            else if (struggleDuration < 6f)
            {
                if (struggleCount < 10f)
                {
                    if (jumpButton || dashButton || Input.GetKeyDown(KeyCode.Space))
                    {
                        struggleCount += 2f;
                    }
                }

                else
                {
                    isStrangled = false;
                    isBreakFree = true;
                    Physics2D.IgnoreLayerCollision(6, 10, true);
                    animator.SetBool("IsTangled", false);
                }
            }

            else
            {
                isStrangled = false;
                isBreakFree = true;
                Physics2D.IgnoreLayerCollision(6, 10, true);
                gameObject.GetComponent<Stamina>().Exhaust(1f);
                gameObject.GetComponent<Health>().Damage(.5f);
                animator.SetBool("IsTangled", false);
            }
        }

        else if (!isStrangled && !isPulling)
        {
            animator.SetBool("IsTangled", false);

            if ((jumpButton || Input.GetKeyDown(KeyCode.Space)) && !canDoubleJump && !IsGrounded() && jumpCount == 1 && playerStamina >= 1)
            {
                canDoubleJump = true;
                gameObject.GetComponent<Stamina>().Exhaust(1f);
            }

            else if ((Input.GetKeyDown(KeyCode.Space) || jumpButton) && IsGrounded())
            {
                canJump = true;
                animator.SetBool("IsJumping", true);
                FindObjectOfType<AudioManager>().Play("Player - Jump");
            }

            if ((sprintButton || Input.GetKey(KeyCode.RightArrow)) && isSuckedByBoss && playerStamina > 0f)
            {
                isSprinting = true;
                sprintTimer = 0f;
            }

            if (isCameraShift)
            {
                shiftTimer += Time.deltaTime;
                if (shiftTimer < 0.2f)
                {
                    playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX += Time.deltaTime/4.75f;
                }

                else
                {
                    shiftTimer = 0f;
                    isCameraShift = false;
                }
            }
                

            if (Input.GetKeyDown(KeyCode.DownArrow) || slideButton)
            {
                FindObjectOfType<AudioManager>().Play("Player - Slide");
                isSliding = true;
            }

            else
            {
                if (_rb.velocity.x <= moveSpeed)
                {
                    animator.SetFloat("Speed", _rb.velocity.x);
                }

                if (!canJump && IsGrounded())
                {
                    animator.SetBool("IsJumping", false);
                    FindObjectOfType<AudioManager>().Play("Player - Land");
                }
            }
        }

        StopJumpBoost();
        StopHealthRegen();
    }

    void FixedUpdate()
    {
        if (isStrangled)
        {
            if (isSliding)
            {
                standing.enabled = true;
                sliding.enabled = false;
                slideDuration = 1f;
                isSliding = false;
                animator.SetBool("IsSliding", false);
            }
            animator.SetBool("IsTangled", true);

            _rb.velocity = new Vector2(0f, 0f);
            Physics2D.gravity = new Vector2(0f, 0f);
            struggleCount -= Time.fixedDeltaTime * 3.5f; // control struggle speed
        }

        else if (isPulling)
        {
            Physics2D.gravity = new Vector2(0f, 0f);
            Physics2D.IgnoreLayerCollision(6, 8, true);
        }

        else
        {
            Physics2D.gravity = new Vector2(0f, -9.81f);

            if (!isSuckedByBoss && !isHitByBoss && _rb.velocity.x <= moveSpeed && _rb.velocity.x > 0)
            {
                _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
            }

            if (isHitByBoss)
            {
                slowTimer += Time.fixedDeltaTime;
                playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX -= Time.fixedDeltaTime / 15f;
                if (slowTimer < 1f)
                {
                    _rb.velocity = new Vector2(moveSpeed - 2f, _rb.velocity.y);
                }
                else
                {
                    slowTimer = 0f;
                    isHitByBoss = false;
                }
            }

            if (isPlunged)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
                isPlunged = false;
            }

            if (isSuckedByBoss)
            {
                if (isSprinting)
                {
                    sprintTimer += Time.deltaTime;
                    if (sprintTimer <= 0.1f)
                    {
                        _rb.velocity = new Vector2(8f, _rb.velocity.y);
                        gameObject.GetComponent<Stamina>().Exhaust(2f * Time.fixedDeltaTime);
                    }

                    else
                    {
                        isSprinting = false;
                        sprintTimer = 0f;
                    }     
                }
                else
                {
                    playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX -= Time.fixedDeltaTime / 10f;
                    _rb.velocity = new Vector2(6f, _rb.velocity.y);
                }
            }

            /*if (moveSpeed < 15f) //Set max speed here
            {
                targetedSpeed += .1f * Time.fixedDeltaTime;
                if (targetedSpeed >= Math.Truncate(targetedSpeed))
                {
                    moveSpeed = (float)Math.Truncate(targetedSpeed);
                }

            }*/

            if (canJump)
            {
                canJump = false;
                jumpCount = 1;
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
                if (isSliding)
                {
                    standing.enabled = true;
                    sliding.enabled = false;
                    slideDuration = 1f;
                    isSliding = false;
                    animator.SetBool("IsSliding", false);
                }
            }

            if (canDoubleJump)
            {
                if (_rb.velocity.x <= 0f && !isStrangled)
                {
                    canDoubleJump = false;
                    jumpCount = 2;
                    _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                    _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
                    _rb.AddForce(Vector2.right * 7f, ForceMode2D.Impulse);
                }

                else
                {
                    canDoubleJump = false;
                    jumpCount = 2;
                    _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                    _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
                }
            }

            if (isSliding)
            {
                IsSliding();
            }

            if (isDashing)
            {
                gameObject.GetComponent<Stamina>().Exhaust(2f);
                _rb.AddForce(Vector2.right * dashTranslate, ForceMode2D.Impulse);
                FindObjectOfType<AudioManager>().Play("Player - Run");
                if (isBossFight)
                {
                    isCameraShift = true;
                }
                isDashing = false;
                GamePlayController.changeDistanceValueBy(3f);
            }

            else if (_rb.velocity.x > moveSpeed)
            {
                _rb.AddForce(Vector2.left * 0.5f, ForceMode2D.Impulse);
            }

            if (_rb.velocity.x <= 0f)
            {
                if (IsGrounded())
                {
                    _rb.AddForce(Vector2.right * 0.5f, ForceMode2D.Impulse);
                }
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D grounded = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, groundAndTopCheck, ground);
        return grounded.collider != null;
    }

    private void IsDashing()
    {
        const float TIME_INTERVAL = 0.2f;

        if ((Input.GetKeyDown(KeyCode.RightArrow) || dashButton) && playerStamina >= 2 && !isSuckedByBoss)
        {
            float timeSinceLastTap = Time.time - firstTap;
            firstTap = Time.time;
            if (timeSinceLastTap <= TIME_INTERVAL)
            {
                isDashing = true;
            }
        }
    }

    private void IsSliding()
    {
        if (slideDuration > 0 && !isStrangled)
        {
            standing.enabled = false;
            sliding.enabled = true;
            slideDuration -= Time.fixedDeltaTime;
            animator.SetBool("IsSliding", true);
            
        }
        else
        {
            standing.enabled = true;
            sliding.enabled = false;
            slideDuration = 1f;
            isSliding = false;
            animator.SetBool("IsSliding", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wahmen"))
        {
            gameObject.GetComponent<Health>().Damage(.5f);
            isPulling = false;
            Physics2D.IgnoreLayerCollision(6, 8, false);
        }

        if (collision.gameObject.CompareTag("Monster"))
        {
            isPulling = false;
            isBeingAttacked = false;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            isStrangled = true;
            struggleCount = 4;
            struggleDuration = 0f;
        }

        if (collision.gameObject.CompareTag("Wave"))
        {
            isPlunged = true;
        }
    }

    public void JumpBoost(float duration)
    {
        if (!isJumpBoost)
        {
            isJumpBoost = true;
            jumpForce *= (float)1.5;
        }

        jumpBoostDuration = duration;
    }

    private void StopJumpBoost()
    {
        if (isJumpBoost)
        {
            jumpBoostDuration -= Time.deltaTime;

            if (jumpBoostDuration <= 0)
            {
                isJumpBoost = false;
                jumpForce /= (float)1.5;
            }
        }
    }

    public void HealthRegen(float duration)
    {
        if (!isHealthRegen)
        {
            isHealthRegen = true;
        }

        healthRegenDuration = duration;
    }


    private void StopHealthRegen()
    {
        if (isHealthRegen)
        {

            healthRegenDuration -= Time.deltaTime;
            addHealthDuration += Time.deltaTime;


            if (addHealthDuration >= 4)
            {
                gameObject.GetComponent<Health>().Regen();
                addHealthDuration = 0;
            }

            if (healthRegenDuration <= 0)
            {
                isHealthRegen = false;
            }
        }
    }

    private void TangledSound()
    {
        FindObjectOfType<AudioManager>().Play("Hair - Tighten");
    }

    private void UntangledSound()
    {
        FindObjectOfType<AudioManager>().Play("Hair - Snap");
    }

}
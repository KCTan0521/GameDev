using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask ground;
    [SerializeField] private BoxCollider2D _col;
    [SerializeField] private BoxCollider2D standing;
    [SerializeField] private BoxCollider2D sliding;
    public Animator animator;
    public float moveSpeed = 10f;
    private float targetedSpeed;
    public float jumpForce = 10f;
    public float dashTranslate = 10f;
    public float slideDuration = 1f;

    private float groundAndTopCheck = 0.2f;
    private float firstTap;
    private bool canJump;
    private bool canDoubleJump;
    private int jumpCount;
    private bool isDashing;
    private bool isSliding;
    public bool isStrangled;
    private float struggleDuration = 0;
    public float struggleCount = 0;
    public bool isBeingAttacked;
    public bool isBreakFree;
    public bool pull;
    public bool isPulled;
    private bool isPulling;

    private bool isJumpBoost = false;
    private float jumpBoostDuration = 0;
    private bool isHealthRegen = false;
    private float healthRegenDuration = 0;
    private float addHealthDuration = 0;

    private bool jumpButton;
    private bool dashButton;
    private bool slideButton;

    private Rigidbody2D _rb;
    private float playerStamina;
    private float playerHealth;

    private void Awake()
    {
        gameObject.AddComponent<Stamina>();
        gameObject.AddComponent<Health>();
        gameObject.AddComponent<TouchDetector>();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        playerStamina = gameObject.GetComponent<Stamina>().stamina;
        playerHealth = gameObject.GetComponent<Health>().health;

        IsDashing();
        IsGrounded();
        gameObject.GetComponent<Health>().PalpitatingAnimation();

        if (isStrangled)
        {
            struggleDuration += Time.deltaTime;
            if (struggleCount <= 0f)
            {
                pull = true;
                if (isPulled)
                {
                    isPulled = false;
                    isPulling = true;
                    isStrangled = false;
                    animator.SetBool("IsTangled", false);
                }
            }

            else if (struggleDuration < 5f)
            {
                if (struggleCount < 10f)
                {
                    if (jumpButton || dashButton || Input.GetKeyDown(KeyCode.Space))
                    {
                        struggleCount += 1f;
                    }
                }

                else
                {
                    isStrangled = false;
                    isBreakFree = true;
                    animator.SetBool("IsTangled", false);
                }
            }

            else
            {
                isStrangled = false;
                isBreakFree = true;
                animator.SetBool("IsTangled", false);
            }
        }

        else if (!isStrangled && !isPulling)
        {
            if ((jumpButton || Input.GetKeyDown(KeyCode.Space)) && !canDoubleJump && !IsGrounded() && jumpCount == 1 && playerStamina >= 1)
            {
                canDoubleJump = true;
                gameObject.GetComponent<Stamina>().Exhaust(1f);
            }

            else if (Input.GetKeyDown(KeyCode.Space) || jumpButton)
            {
                canJump = true;
                animator.SetBool("IsJumping", true);
                FindObjectOfType<AudioManager>().Play("Player - Jump");
            }

            if (dashButton && playerStamina >= 2)
            {
                isDashing = true;
                gameObject.GetComponent<Stamina>().Exhaust(2f);
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
            struggleCount -= Time.fixedDeltaTime * 5; // control struggle speed
        }

        else if (isPulling)
        {
            Physics2D.gravity = new Vector2(0f, 0f);
            Physics2D.IgnoreLayerCollision(6, 8, true);
        }

        else
        {
            Physics2D.gravity = new Vector2(0f, -9.81f);

            if (_rb.velocity.x <= moveSpeed && _rb.velocity.x > 0)
            {
                _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
            }

            /*if (moveSpeed < 15f) //Set max speed here
            {
                targetedSpeed += .1f * Time.fixedDeltaTime;
                if (targetedSpeed >= Math.Truncate(targetedSpeed))
                {
                    moveSpeed = (float)Math.Truncate(targetedSpeed);
                }

            }*/

            if (canJump && IsGrounded())
            {
                canJump = false;
                jumpCount = 1;
                _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
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
                _rb.AddForce(Vector2.right * dashTranslate, ForceMode2D.Impulse);
                FindObjectOfType<AudioManager>().Play("Player - Run");
                isDashing = false;
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

        if (Input.GetKeyDown(KeyCode.RightArrow) && playerStamina >= 2)
        {
            float timeSinceLastTap = Time.time - firstTap;
            firstTap = Time.time;
            if (timeSinceLastTap <= TIME_INTERVAL)
            {
                gameObject.GetComponent<Stamina>().Exhaust(2f);
                isDashing = true;
            }
        }
    }

    private void IsSliding()
    {
        if (slideDuration > 0 && !canJump && !isStrangled)
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
        if (collision.gameObject.tag == "Wahmen")
        {
            gameObject.GetComponent<Health>().Damage(.5f);
            isPulling = false;
            Physics2D.IgnoreLayerCollision(6, 8, false);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            isStrangled = true;
            struggleCount = 5;
            struggleDuration = 0f;
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
                Debug.Log("add health");
            }

            if (healthRegenDuration <= 0)
            {
                isHealthRegen = false;
            }
        }
    }

}
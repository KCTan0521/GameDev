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
    private bool isStunned;

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

        FindObjectOfType<AudioManager>().Play("Song1"); //works

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
            _rb.velocity = new Vector2(0f, 0f);
            struggleDuration += Time.deltaTime;

            if (struggleCount <= 0f)
            {
                isStrangled = false;
                // pull towards
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
                }
            }

            else
            {
                isStrangled = false;
            }
        }

        else if (!isStrangled)
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
            }

            if (dashButton)
            {
                isDashing = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || slideButton)
            {
                isSliding = true;
            }

            else
            {
                if (_rb.velocity.x <= moveSpeed)
                {
                    animator.SetFloat("Speed", _rb.velocity.x);
                }

                if (isSliding)
                {
                    IsSliding();
                }

                if (!canJump && IsGrounded())
                {
                    animator.SetBool("IsJumping", false);
                    FindObjectOfType<AudioManager>().Play("Player - Run"); // buggy (noisy at start, silent after)
                }
            }
        }
    }
    
    void FixedUpdate()
    {
        if (isStrangled)
        {
            struggleCount -= Time.fixedDeltaTime * 5;
        }

        if (_rb.velocity.x <= moveSpeed && _rb.velocity.x > 0 && !isStrangled)
        {
            _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
        }
        
        if (moveSpeed < 15f) //Set max speed here
        {
            targetedSpeed += .1f * Time.fixedDeltaTime;
            if (targetedSpeed >= Math.Truncate(targetedSpeed))
            {
                moveSpeed = (float)Math.Truncate(targetedSpeed);
            }

        }

        if (canJump && IsGrounded() && !isStrangled)
        {
            canJump = false;
            jumpCount = 1;
            _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
            FindObjectOfType<AudioManager>().Play("Player - Jump");
        }

        if (canDoubleJump && !isStrangled)
        {
            if (_rb.velocity.x <= 0f && !isStrangled)
            {
                canDoubleJump = false;
                jumpCount = 2;
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
                _rb.AddForce(Vector2.right * 7f, ForceMode2D.Impulse);
                Debug.Log("push");
            }

            else
            {
                canDoubleJump = false;
                jumpCount = 2;
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
            }
        }

        if (isDashing && !isStrangled)
        {
            _rb.AddForce(Vector2.right * dashTranslate, ForceMode2D.Impulse);
            isDashing = false;
        }

        if (isStunned)
        {
            _rb.velocity = new Vector2(0f, 0f);
            _rb.AddForce(new Vector2(-0.5f * moveSpeed, 9.81f), ForceMode2D.Impulse);
            isStunned = false;
        }
        
        else if (_rb.velocity.x > moveSpeed && !isStrangled)
        {
            _rb.AddForce(Vector2.left * 0.5f, ForceMode2D.Impulse);
        }

        if (_rb.velocity.x <= 0f && !isStrangled)
        {
            if (IsGrounded())
            {
                _rb.AddForce(Vector2.right * 0.5f, ForceMode2D.Impulse);
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
                isDashing = true;
                gameObject.GetComponent<Stamina>().Exhaust(2f);
                FindObjectOfType<AudioManager>().Play("Player - Dash"); // doesnt work
            }
        }
    }

    private void IsSliding()
    {
        if (slideDuration > 0 && !canJump)
        {
            standing.enabled = false;
            sliding.enabled = true;
            slideDuration -= Time.deltaTime;
            animator.SetBool("IsSliding", true);
            FindObjectOfType<AudioManager>().Play("Player - Slide"); // doesnt work
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
        if (collision.gameObject.name == "Wahmen")
        {
            gameObject.GetComponent<Health>().Damage(.5f);
            isStunned = true;
        }

        if (collision.gameObject.tag == "Bullet")
        {
            struggleCount = 5;
            isStrangled = true;
        }
    }
}
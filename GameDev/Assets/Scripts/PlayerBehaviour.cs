using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private BoxCollider2D _col;
    [SerializeField] private BoxCollider2D standing;
    [SerializeField] private BoxCollider2D sliding;
    public Animator animator;
    public float moveSpeed = 10f;
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

    private bool jumpButton;
    private bool dashButton;
    private bool slideButton;

    private Rigidbody2D _rb;
    private float playerStamina;

    private void Awake()
    {
        gameObject.AddComponent<Stamina>();
        gameObject.AddComponent<TouchDetector>();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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

        IsDashing();
        IsGrounded();

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
                _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
                animator.SetFloat("Speed", _rb.velocity.x);
            }

            if (isSliding)
            {
                IsSliding();
            }

            if (!canJump && IsGrounded())
            {
                animator.SetBool("IsJumping", false);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (canJump && IsGrounded())
        {
            canJump = false;
            jumpCount = 1;
            _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
        }

        if (canDoubleJump)
        {
            canDoubleJump = false;
            jumpCount = 2;
            _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
        }

        if (isDashing)
        {
            _rb.AddForce(Vector2.right * dashTranslate, ForceMode2D.Impulse);
            isDashing = false;
        }
        
        else if (_rb.velocity.x > moveSpeed)
        {
            _rb.AddForce(Vector2.left * 0.4f, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D grounded = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, groundAndTopCheck, platformLayerMask);
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
}
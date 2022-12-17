using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private BoxCollider2D standing;
    [SerializeField] private BoxCollider2D sliding;
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float sprintVelocity = 10f;
    public float dashTranslate = 20f;
    public float slideDuration = 1f;

    private float groundAndTopCheck = 0.5f;
    private float firstTap;
    private bool isJumping;
    private bool isDashing;
    private bool isSliding;

    private Rigidbody2D _rb;
    private BoxCollider2D _col;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        standing.enabled = true;
        sliding.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsDashing();
        IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || IsUnder())
        {
            isSliding = true;
        }

        else
        {
            if (_rb.velocity.x <= moveSpeed)
            {
                _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
            }

            if (isSliding)
            {
                IsSliding();
            }
        }
    }
    
    void FixedUpdate()
    {
        if (IsGrounded() && isJumping)
        {
            _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
            isJumping = false;
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

    private bool IsUnder()
    {
        RaycastHit2D under = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.up, groundAndTopCheck, platformLayerMask);
        return under.collider != null;
    }

    private void IsDashing()
    {
        const float TIME_INTERVAL = 0.2f;

        if (Input.GetKeyDown(KeyCode.RightArrow))
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
        if (slideDuration > 0)
        {
            standing.enabled = false;
            sliding.enabled = true;
            slideDuration -= Time.deltaTime;
        }

        else
        {
            standing.enabled = true;
            sliding.enabled = false;
            slideDuration = 1f;
            isSliding = false;
        }
    }
}
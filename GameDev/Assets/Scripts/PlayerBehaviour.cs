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
    public float jumpDuration = .25f;

    private float distanceToGround = 0.1f;
    private float firstTap;
    private bool isInAir;
    private float jumpTimeCounter;

    private Rigidbody2D _rb;
    private BoxCollider2D _col;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            isInAir = true;
            jumpTimeCounter = jumpDuration;
            _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.Space) && isInAir)
        {
            if (jumpTimeCounter > 0)
            {
                _rb.AddForce(Vector2.up * (jumpForce + 9.81f), ForceMode2D.Force);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isInAir = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isInAir = false;
        }

        if (IsDashing()/* && !IsCoolDown()*/)
        {
            _rb.AddForce(Vector2.right * dashTranslate, ForceMode2D.Impulse);
        }

        else
        {
            if (_rb.velocity.x <= moveSpeed)
            {
                _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);
            }
        }
    }
    
    void FixedUpdate()
    {
    }

    private bool IsGrounded()
    {
        RaycastHit2D grounded = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, distanceToGround, platformLayerMask);
        return grounded.collider != null;
    }

    private bool IsDashing()
    {
        bool isDashing = false;
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
        return isDashing;
    }
}
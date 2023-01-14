using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster_Giant : MonoBehaviour
{

    public float jumpForce;
    public float horizontalForce;
    public float impulseForceX;
    public float recoverTime;
    public float dropMultiplier;
    public float attackDistance;
    public Health healthSystem;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool isAttack;
    private bool isJumping;
    private bool isDrop = false;
    private bool isStop = false;
    private int ignoreFrame = 0;     //number of frame to ignore to avoid multiple detect collision in one real collision
    private float isStopTimer = 0;

    private Rigidbody2D _player;
    private Rigidbody2D myBody;
    private Animator anim;
    private Transform trans;
    
    private const string SMASH_ANIMATION = "Launch";
    private const float GRAVITY = 9.81f + 8f;
    

    public void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        isAttack = false;
    }

    public void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(6, 9, false);
    }
    
    public void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void Update()
    {
        

        if (isGrounded && !isAttack /*&& (myBody.transform.x - _player.transform.x) <= attackDistance*/)
        {
            anim.SetBool("isSmash", true);

            if(ignoreFrame <= 0)
                MonsterJump();
        }

        if(isJumping)
        {
            //check velocity y to know if dropping down
            anim.SetFloat("speed_y", myBody.velocity.y);
        }

        if (isJumping && !isGrounded && myBody.velocity.y < 0 && !isDrop)
        {
            myBody.AddForce(new Vector2(0, -1 * dropMultiplier), ForceMode2D.Impulse);
            isDrop = true;
        }

        if (isJumping && isGrounded && myBody.velocity.y == 0)
        {
            //Landing
            anim.SetBool("isSmash", false);
            anim.SetBool("isGround", true);

            myBody.velocity = new Vector2(0f, 0f);
        }
            

        DestroyMonster();
        ignoreFrame--;

        if (isStop)
        {
            Physics2D.IgnoreLayerCollision(6, 9, true);

            isStopTimer += Time.deltaTime;
            if (isStopTimer >= recoverTime)
            {
                isStop = false;
                GetComponent<BoxCollider2D>().enabled = true;
                Physics2D.IgnoreLayerCollision(6, 9, false);
                Debug.Log("Cancel ignore");
                isStopTimer = 0;
            }
        }
    }

    private void DestroyMonster()
    {
        if (trans.position.y < -10)
        {
            Destroy(gameObject);
        }
    }


    private void MonsterJump()
    {
        //Launching
        myBody.AddForce(new Vector2(horizontalForce, jumpForce), ForceMode2D.Impulse);

        isJumping = true;
        isAttack = true;
        ignoreFrame = 10;               //set ignore frame here
    }


    private void OnCollisionEnter2D(Collision2D collision) // need collision.gameObject
    {

        if (collision.gameObject.CompareTag("Player") && !isStop)
        {

            if (transform.position.x > _player.transform.position.x)
            {
                _player.velocity = new Vector2(0f, 0f);
                _player.AddForce(new Vector2(-1f * impulseForceX, GRAVITY), ForceMode2D.Impulse);
            }
            else if (transform.position.x < _player.transform.position.x)
            {
                _player.velocity = new Vector2(0f, 0f);
                _player.AddForce(new Vector2(-1f * impulseForceX, GRAVITY), ForceMode2D.Impulse);
            }

            _player.GetComponent<Health>().Damage(.5f);
            
            isStop = true;  

        }

           

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Platform"))
        {
            Destroy(collision.gameObject);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterGiant : MonoBehaviour
{

    public float highJumpX;
    public float highJumpY;
    public float longJumpX;
    public float longJumpY;

    public float impulseForceX;
    public float recoverTime;
    public float dropMultiplier;
    public float fictionMultiplier;
    public float attackDistance;
    public Health healthSystem;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool isAttack;
    private bool isJumping;
    private bool isDrop = false;
    private bool isLaunch = false;
    private bool isStop = false;
    private float isStopTimer = 0;

    private Rigidbody2D myBody;
    private Rigidbody2D _player;
    private Animator anim;
    private Transform trans;
    
    private const string SMASH_ANIMATION = "Launch";
    private const float GRAVITY = 9.81f + 8f;
    

    public void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        isAttack = false;
    }

    public void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 9, false);
    }
    
    public void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !isAttack /*&& (myBody.transform.x - _player.transform.x) <= attackDistance*/)
        {
            anim.SetBool("isSmash", true);

            if (!isLaunch)
            {
                //launch once and avoid jumping again
                MonsterJump();
                isLaunch = true;
            }

        }

        if (isJumping && !isGrounded && myBody.velocity.y < 0 && !isDrop)
        {
            //add drop acceleration
            myBody.AddForce(new Vector2(0, -1 * dropMultiplier), ForceMode2D.Impulse);
            isDrop = true;
        }

        if (isJumping && !isGrounded && myBody.velocity.x < 0)
        {
            //add horizontal friction
            myBody.AddForce(new Vector2(1 * fictionMultiplier, 0), ForceMode2D.Impulse);
        }

        if (isJumping && isGrounded && myBody.velocity.y == 0)
        {
            //Landing
            Debug.Log("land");
            anim.SetBool("isSmash", false);
            anim.SetBool("isGround", true);

            myBody.velocity = new Vector2(0f, 0f);
        }
    }

    public void Update()
    {
        //Debug.Log(gameObject.transform.position.x);


        if(isJumping)
        {
            //check velocity y to know if dropping down
            anim.SetFloat("speed_y", myBody.velocity.y);

            Debug.Log(myBody.velocity);
        }
            

        if (isStop)
        {
            Physics2D.IgnoreLayerCollision(6, 9, true);

            isStopTimer += Time.deltaTime;
            if (isStopTimer >= recoverTime)
            {
                isStop = false;
                GetComponent<BoxCollider2D>().enabled = true;
                Physics2D.IgnoreLayerCollision(6, 9, false);
                isStopTimer = 0;
            }
        }

        DestroyMonster();
    }

    private void DestroyMonster()
    {
        if (trans.position.y < -10)
        {
            Destroy(gameObject);
        }
    }


    private void MonsterJump(bool isHighJump = true)
    {
        //Launching
        if (isHighJump)
        {
            myBody.AddForce(new Vector2(highJumpX, highJumpY), ForceMode2D.Impulse);
        } else
        {
            myBody.AddForce(new Vector2(longJumpX, longJumpY), ForceMode2D.Impulse);
        }
        
        Debug.Log("Jump");

        isJumping = true;
        isAttack = true;
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

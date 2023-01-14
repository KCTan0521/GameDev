using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster_Giant : MonoBehaviour
{

    public float jumpForce;
    public float horizontalForce;
    public Health healthSystem;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private bool isDamaged = true;
    private bool isGrounded;
    private bool isAttack;
    private bool isJumping;
    private int ignoreFrame = 0;     //number of frame to ignore to avoid multiple detect collision in one real collision

    private Rigidbody2D myBody;
    private Animator anim;
    private Transform trans;
    

    private const string SMASH_ANIMATION = "Launch";


    public void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        isAttack = false;
    }

    public void Start()
    {
        
    }
    
    public void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void Update()
    {
        if (isGrounded && !isAttack)
        {
            anim.SetBool("isSmash", true);
        }

        if(isJumping)
        {
            //check velocity y to know if dropping down
            anim.SetFloat("speed_y", myBody.velocity.y);
            Debug.Log(myBody.velocity.y);
        }
        
        if (isJumping && isGrounded && ignoreFrame <= 0)
        {
            //Landing
            anim.SetBool("isSmash", false);
            anim.SetBool("isGround", true);
            Debug.Log("Smashh");
        }

        DestroyMonster();
        ignoreFrame--;
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
        Debug.Log("Launch");

        isJumping = true;
        isAttack = true;
        ignoreFrame = 10;               //set ignore frame here
    }


    private void OnCollisionEnter2D(Collision2D collision) // need collision.gameObject
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (isDamaged)
            {
                
            }
        }

    }

}

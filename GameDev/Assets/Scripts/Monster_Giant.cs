using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster_Giant : MonoBehaviour
{

    public float jumpForce = 50f;
    public float horizontalForce;
    public Health healthSystem;

    [SerializeField] private LayerMask ground;
    [SerializeField] private BoxCollider2D _col;
    private bool isDamaged = true;
    private float groundAndTopCheck = 0.2f;

    private Rigidbody2D myBody;
    private Animator anim;
    private Transform trans;
    

    private const string SMASH_ANIMATION = "Launch";


    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
    }

    private void Start()
    {
        
    }

    public void Update()
    {
        if (IsGrounded())
        {
            Debug.Log("Ground");
        }

        MonsterJump();
        AnimatePlayer();
        DestroyMonster();
    }

    private void DestroyMonster()
    {
        if (trans.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D grounded = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, groundAndTopCheck, ground);
        return grounded.collider != null;
    }

    private void AnimatePlayer()
    {
        /*if (isGrounded)
        {
            anim.SetBool(SMASH_ANIMATION, false);
        }
        else
        {
            anim.SetBool(SMASH_ANIMATION, true);
        }*/
    }

    private void MonsterJump()
    {
        if (IsGrounded())
        {
            myBody.AddForce(new Vector2(horizontalForce, jumpForce), ForceMode2D.Impulse);
        }
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

    IEnumerator MonsterDamage()
    {
        isDamaged = false;
        healthSystem.Damage(.5f);
        // Debug.Log("hurt");
        yield return new WaitForSeconds(0.5f);
        isDamaged = true;
    }


}

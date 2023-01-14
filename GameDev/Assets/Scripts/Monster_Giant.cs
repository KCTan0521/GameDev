using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster_Giant : MonoBehaviour
{

    [SerializeField]
    private float jumpForce = 50f;
    private bool isGrounded = true;
    private bool isDamaged = true;

    private Rigidbody2D myBody;
    private Animator anim;
    private Transform trans;
    private Health healthSystem;

    private const string SMASH_ANIMATION = "Smash";


    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        healthSystem =  GameObject.FindObjectOfType<Health>();
    }

    private void Start()
    {
        StartCoroutine(MonsterGiantJump());
    }

    IEnumerator MonsterGiantJump()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            MonsterJump();
        } 
    }

    void Update()
    {
        AnimatePlayer();
        DestroyMonster();
    }

    void DestroyMonster()
    {
        if (trans.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    void AnimatePlayer()
    {
        if (isGrounded)
        {
            anim.SetBool(SMASH_ANIMATION, false);
        }
        else
        {
            anim.SetBool(SMASH_ANIMATION, true);
        }
    }

    void MonsterJump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision) // need collision.gameObject
    {
        if (collision.gameObject.CompareTag("Ground")) // at here
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (isDamaged)
            {
                StartCoroutine(MonsterDamage());
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

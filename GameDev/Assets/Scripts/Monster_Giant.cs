using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster_Giant : MonoBehaviour
{

    [SerializeField]
    private float jumpForce = 30f;
    private bool isGrounded = true;

    private Rigidbody2D myBody;
    private Animator anim;

    private const string SMASH_ANIMATION = "Smash";


    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(MonsterGiantJump());
    }

    IEnumerator MonsterGiantJump()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            PlayerJump();
        } 
    }

    void Update()
    {
        AnimatePlayer();
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

    void PlayerJump()
    {

        if (isGrounded)
        {
            
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
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
            SceneManager.LoadScene("GameOver");
        }

    }
}

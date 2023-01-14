using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject _bullet;
    private Rigidbody2D _player;
    private float attackTimer;
    private bool attack;
    private bool isStrangling;
    private bool isAttacking;
    private bool isIncapacitate;
    private float incapacitateTimer;
    private bool isBreakFree;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isStrangling = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled;
        isAttacking = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked;
        isBreakFree = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBreakFree;
        
        if (isIncapacitate)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            incapacitateTimer += Time.deltaTime;
            if (incapacitateTimer >= 2f) // change recover time here
            {
                isIncapacitate = false;
                GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBreakFree = false;
                GetComponent<BoxCollider2D>().enabled = true;
                incapacitateTimer = 0;
            }
        }

        else if (isBreakFree)
        {
            isIncapacitate = true;
        }
    }
    private void FixedUpdate()
    {
        if (attack && !isStrangling && !isAttacking && !isIncapacitate)
        {
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer >= 1f)
            {
                GetComponent<Animator>().SetBool("attack", true);
                GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().woman = gameObject;

                GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked = true;
                attackTimer = 0;
            }
        }

        if (isStrangling || isAttacking || isIncapacitate)
        {
            attackTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            attack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            /*Destroy(this.gameObject);*/
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }

        if (collision.gameObject.name == "Player")
        {
            if (transform.position.x > _player.transform.position.x)
            {
                _player.velocity = new Vector2(0f, 0f);
                _player.AddForce(new Vector2(-0.5f * 15, 9.81f), ForceMode2D.Impulse);
            }

            else if (transform.position.x < _player.transform.position.x)
            {
                _player.velocity = new Vector2(0f, 0f);
                _player.AddForce(new Vector2(0.5f * 15, 9.81f), ForceMode2D.Impulse);
            }
            isIncapacitate = true;
        }
    }
}

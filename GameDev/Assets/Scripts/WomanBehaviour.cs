using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanBehaviour : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private GameObject _bullet;
    private Rigidbody2D _rb;
    private Rigidbody2D _player;
    private float attackTimer;
    private bool attack;
    private bool isStrangling;
    private bool isAttacking;
    private bool isPulled;
    private bool isIncapacitate;
    private float incapacitateTimer;
    private bool isBreakFree;
    private float distance;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isStrangling = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled;
        isAttacking = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked;
        isBreakFree = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBreakFree;
        isPulled = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isPulled;
        distance = transform.position.x - _player.transform.position.x;

        AttackRange();
        Animation();

        if (transform.position.x - _player.transform.position.x < -40f)
        {
            Destroy(_rb.gameObject);
        }

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

        DestroyMonster();
    }

    private void FixedUpdate()
    {
        if (attack && !isStrangling && !isAttacking && !isIncapacitate)
        {
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer >= 0.5f)
            {
                Instantiate(_bullet, transform.position, Quaternion.identity);
                GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked = true;
                attackTimer = 0;
            }
        }

        if (isStrangling || isAttacking || isIncapacitate || !attack)
        {
            attackTimer = 0;
        }

        if (distance < 0)
        {
            _rb.GetComponent<SpriteRenderer>().flipX = true;
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

    private void AttackRange()
    {
        if ((distance > 1f && distance <= 15f) || (distance < -1f && distance >= -15f))
        {
            attack = true;
        }

        else
        {
            attack = false;
        }
    }

    private void Animation()
    {
        if (attack)
        {
            animator.SetBool("Attack", true);
        }

        if (!attack)
        {
            animator.SetBool("Attack", false);
        }

        if (isAttacking)
        {
            animator.SetBool("IsAttacking", true);
        }

        if (!isAttacking)
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    private void DestroyMonster()
    {
        if ((transform.position.x - _player.transform.position.x) < -30)
        {
            Destroy(gameObject);
        }
    }

    private void AttackSound()
    {
        FindObjectOfType<AudioManager>().Play("Hair - Release");
    }
}

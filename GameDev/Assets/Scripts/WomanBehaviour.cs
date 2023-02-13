using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanBehaviour : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private GameObject _bullet;
    private Rigidbody2D _rb;
    private Rigidbody2D _player;
    private float loadingTimer;
    private bool attack;
    private bool isReloaded;
    private bool isStrangling;
    private bool isAttacking;
    private bool isPulled;
    private bool isIncapacitate;
    private float incapacitateTimer;
    private bool isBreakFree;
    private bool isCrying;
    private float distance;
    private GameObject bulletFired;

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

        if (attack && !isStrangling && !isAttacking && !isIncapacitate && isReloaded)
        {
            Instantiate(_bullet, transform.position, Quaternion.identity);
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked = true;
            isReloaded = false;
        }

        if (!attack)
        {
            isReloaded = true;
        }

        if (distance < 0)
        {
            _rb.GetComponent<SpriteRenderer>().flipX = true;
        }

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

        CryingSound();
        DestroyMonster();
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
        if ((distance > 1f && distance <= 15f) || (distance < -1f && distance >= -10f))
        {
            float loadingDuration = 0.4f;
            loadingTimer += Time.deltaTime;
            isCrying = true;

            if (isIncapacitate)
            {
                isCrying = false;
                loadingTimer = 0;
            }

            if (attack)
            {
                loadingTimer = 0;
            }

            if (loadingTimer >= loadingDuration)
            {
                attack = true;
                loadingTimer = 0f;
            }
        }

        else
        {
            isCrying = false;
            attack = false;
            loadingTimer = 0f;
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

    private void CryingSound()
    {
        if (isCrying)
        {
            FindObjectOfType<AudioManager>().PlayOnce("Women - Cry");
        }

        else
        {
            FindObjectOfType<AudioManager>().Stop("Women - Cry");
        }
    }
}

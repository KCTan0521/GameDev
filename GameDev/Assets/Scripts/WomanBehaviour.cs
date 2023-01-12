using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    private float attackTimer;
    private bool attack;
    private bool isStrangling;
    private bool isAttacking;
    private bool isIncapacitate;
    private float incapacitateTimer;

    private void Update()
    {
        isStrangling = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled;
        isAttacking = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked;

        if (isIncapacitate)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            incapacitateTimer += Time.deltaTime;
            if (incapacitateTimer >= 2f)
            {
                isIncapacitate = false;
                GetComponent<BoxCollider2D>().enabled = true;
                incapacitateTimer = 0;
            }
        }
    }
    private void FixedUpdate()
    {
        if (attack && !isStrangling && !isAttacking)
        {
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer >= 1f)
            {
                Instantiate(_bullet, transform.position, Quaternion.identity);
                GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked = true;
                attackTimer = 0;
            }
        }

        if (isStrangling && isAttacking)
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
            isIncapacitate = true;
        }
    }
}

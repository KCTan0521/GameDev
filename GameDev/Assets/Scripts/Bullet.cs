using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private Vector3 initialTarget;
    private Vector3 initialPos;
    private Vector2 direction;
    private bool isStrangled;
    private bool isReflected;
    private float timer;
    private bool hitTarget;

    public float attackSpeed = 1f;
    public float speed = 10f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        initialTarget = _player.transform.position + new Vector3(1f, 0f, 0f);
        initialPos = _rb.transform.position;
        direction = initialTarget - initialPos;
        isReflected = false;
        hitTarget = false;
        timer = 0f;
    }

    private void Update()
    {
        isStrangled = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled;
        Attack();
    }

    private void Attack()
    {
        if (isReflected)
        {
            if (initialTarget.x < initialPos.x)
            {
                Fire(false);
                if (_rb.transform.position.x > initialPos.x)
                {
                    GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked = false;
                    Destroy(_rb.gameObject);
                }
            }

            else
            {
                Fire(false);
                if (_rb.transform.position.x < initialPos.x)
                {
                    GameObject.Find("Player").GetComponent<PlayerBehaviour>().isBeingAttacked = false;
                    Destroy(_rb.gameObject);
                }
            }
        }

        else if (initialTarget.x < initialPos.x)
        {
            if (isStrangled)
            {
                _rb.velocity = new Vector2(0f, 0f);
            }

            if (!isStrangled && hitTarget)
            {
                isReflected = true;
            }

            else if (_rb.transform.position.x < initialTarget.x)
            {
                _rb.velocity = new Vector2(0f, 0f);
                timer += Time.deltaTime;
                if (timer >= attackSpeed)
                {
                    isReflected = true;
                    timer = 0;
                }
            }

            else if (!hitTarget)
            {
                Fire(true);
            }
        }

        else
        {
            if (isStrangled)
            {
                _rb.velocity = new Vector2(0f, 0f);
                Debug.Log("strangled");
            }

            if (!isStrangled && hitTarget)
            {
                isReflected = true;
            }

            else if (_rb.transform.position.x > initialTarget.x)
            {
                _rb.velocity = new Vector2(0f, 0f);
                timer += Time.deltaTime;
                if (timer >= attackSpeed)
                {
                    isReflected = true;
                    timer = 0;
                }
            }

            else if (!hitTarget)
            {
                Fire(true);
            }
        }
    }

    private void Fire(bool isLeft)
    {
        if (isLeft)
        {
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }

        else
        {
            _rb.velocity = new Vector2(-direction.x, -direction.y).normalized * speed;
            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            hitTarget = true;
        }
    }
}

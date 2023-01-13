using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Collider2D _col;
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private Vector3 initialTarget;
    private Vector3 initialPos;
    private Vector2 direction;
    private bool isStrangled;
    private bool isReflected;
    private float timer;
    private bool hitTarget;
    private bool pull;

    public float attackSpeed = 1f;
    public float speed = 20f;
    public GameObject woman;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        initialTarget = _player.transform.position;
        initialPos = _rb.transform.position;
        targetSetter();
        direction = initialTarget - initialPos;
        isReflected = false;
        hitTarget = false;
        timer = 0f;
    }

    private void Update()
    {
        pull = GameObject.Find("Player").GetComponent<PlayerBehaviour>().pull;
        if (hitTarget)
        {
            _col.isTrigger = enabled;
        }
    }

    private void FixedUpdate()
    {
        isStrangled = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled;
        Attack();
        if (pull)
        {
            _player.velocity = new Vector2(-direction.x, -direction.y).normalized * speed;
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().pull = false;
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().isPulled = true;
        }
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
                    woman.GetComponent<Animator>().SetBool("attack", false);
                    woman.GetComponent<Animator>().SetBool("endattack", true);
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

            if (_rb.transform.position.x < initialTarget.x && !hitTarget)
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

        else if (initialTarget.x > initialPos.x)
        {
            if (isStrangled)
            {
                _rb.velocity = new Vector2(0f, 0f);
            }

            if (!isStrangled && hitTarget)
            {
                isReflected = true;
            }

            else if (_rb.transform.position.x > initialTarget.x && !hitTarget)
            {
                _rb.velocity = new Vector2(0f, 0f);
                timer += Time.deltaTime;
                if (timer >= attackSpeed)
                {
                    Debug.Log("reflected");
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
            /*_rb.AddForce(new Vector2(direction.x, direction.y).normalized * 5f, ForceMode2D.Impulse);*/
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }

        else
        {
            /*_rb.AddForce(new Vector2(-direction.x, -direction.y).normalized * 5f, ForceMode2D.Impulse);*/
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Destroy(collision.gameObject);
        }
    }

    private void targetSetter()
    {
        if (_player.transform.position.x < _rb.transform.position.x)
        {
            if (_player.transform.position.y < _rb.transform.position.y - 1 || _player.transform.position.y > _rb.transform.position.y + 1)
            {
                initialTarget.x -= (initialTarget.x - initialPos.x) * 0.2f;
            }
        }
        else
        {
            if (_player.transform.position.y < _rb.transform.position.y - 1 || _player.transform.position.y > _rb.transform.position.y + 1)
            {
                initialTarget.x += (initialTarget.x - initialPos.x) * 0.4f;
            }

            else
            {
                initialTarget.x += (initialTarget.x - initialPos.x) * 0.2f;
            }
        }
    }
}

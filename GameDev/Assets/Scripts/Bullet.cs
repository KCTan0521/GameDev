using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Collider2D _col;
    private Rigidbody2D _player;
    private Rigidbody2D _rb;
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;

    private Vector3 initialTarget;
    private Vector3 initialPos;
    private Vector2 direction;
    private bool isStrangled;
    private bool isReflected;
    private float attackTimer;
    private float pullTimer;
    private bool hitTarget;
    private bool pull;

    public float attackSpeed = 1f;
    public float speed = 20f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(0f, 0f);
        initialTarget = _player.transform.position;
        initialPos = _rb.transform.position;
        targetSetter();
        direction = initialTarget - initialPos;
        isReflected = false;
        hitTarget = false;
        pullTimer = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.IgnoreLayerCollision(6, 10, false);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineMaterial.mainTexture = spriteRenderer.sprite.texture;
        lineRenderer.material = lineMaterial;
    }

    private void Update()
    {
        float aimDuration = 0.1f;
        isStrangled = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled;
        pull = GameObject.Find("Player").GetComponent<PlayerBehaviour>().pull;

        if (attackTimer < aimDuration)
        {
            attackTimer += Time.deltaTime;
        }

        else if (attackTimer >= aimDuration)
        {
            Attack();
        }

        lineRenderer.SetPosition(0, new Vector2(initialPos.x, initialPos.y + .5f));
        lineRenderer.SetPosition(1, gameObject.transform.position);
    }

    private void FixedUpdate()
    {      
        if (pull)
        {
            Vector2 pullDirection = initialPos - _player.transform.position;
            _player.velocity = pullDirection.normalized * speed;
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().pull = false;
            GameObject.Find("Player").GetComponent<PlayerBehaviour>().isPulled = true;
        }
    }

    private void Attack()
    {
        if (isReflected)
        {
            Physics2D.IgnoreLayerCollision(6, 10, true);
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
                attackTimer = 0f;
            }

            if (_rb.transform.position.x < initialTarget.x && !hitTarget)
            {
                _rb.velocity = new Vector2(0f, 0f);
                pullTimer += Time.deltaTime;
                if (pullTimer >= attackSpeed)
                {
                    isReflected = true;
                    attackTimer = 0f;
                    pullTimer = 0;
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
                attackTimer = 0f;
            }

            else if (_rb.transform.position.x > initialTarget.x && !hitTarget)
            {
                _rb.velocity = new Vector2(0f, 0f);
                pullTimer += Time.deltaTime;
                if (pullTimer >= attackSpeed)
                {
                    isReflected = true;
                    attackTimer = 0f;
                    pullTimer = 0;
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
            Physics2D.IgnoreLayerCollision(6, 10, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
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

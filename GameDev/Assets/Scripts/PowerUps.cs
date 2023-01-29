using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUps : MonoBehaviour
{

    private GameObject _player;
    public PowerUpType powerUpType;
    private Transform visual;

    private void Start()
    {
        visual = gameObject.transform;
        StartCoroutine(FloatingAnimation());
    }

    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        if((_player.transform.position.x - gameObject.transform.position.x) > 30)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (powerUpType == PowerUpType.JumpBoost)
                _player.GetComponent<PlayerBehaviour>().JumpBoost(15);

            if (powerUpType == PowerUpType.HealthRegen)
                _player.GetComponent<PlayerBehaviour>().HealthRegen(15);

            Destroy(gameObject);
        }

    }

    private IEnumerator FloatingAnimation()
    {
        while (true)
        {
            visual.position = new Vector2(visual.position.x, (visual.position.y + Mathf.Sin(Time.time * 5) * 0.005f));
            yield return null;
        }
    }
}


public enum PowerUpType
{
    JumpBoost,
    HealthRegen
}
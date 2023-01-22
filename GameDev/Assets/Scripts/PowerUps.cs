using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUps : MonoBehaviour
{

    private GameObject _player;
    public PowerUpType powerUpType;


    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.Find("Player");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (powerUpType == PowerUpType.JumpBoost)
                _player.GetComponent<PlayerBehaviour>().JumpBoost(15);

            Destroy(gameObject);
        }

    }
}


public enum PowerUpType
{
    JumpBoost,
    HealthRegen
}
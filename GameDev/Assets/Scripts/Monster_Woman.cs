using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Woman : MonoBehaviour
{
    private bool isExist = true;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {/*
        if (!player)
        {
            return;
        }

        if (transform.position.y <= -50 && isExist)
        {
            Debug.Log("Destroy Monster_Woman game object");
            Destroy(gameObject);
            isExist = false;
        }


        if (Mathf.Abs(player.position.x - transform.position.x) <= 5 || 
            Mathf.Abs(player.position.y - transform.position.y) <= 5)
        {
            Debug.Log("Hello too near");
        }*/
    }
}

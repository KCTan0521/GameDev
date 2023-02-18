using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChasingMob : MonoBehaviour
{
    private GameObject _player;
    public float distanceChasing;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(_player.transform.position.x - distanceChasing, 4);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private Collider2D _col;
    private Rigidbody2D _player;

    // Start is called before the first frame update
    void Start()
    {
        _col = this.GetComponent<Collider2D>();
        _player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.y - 0.625f < _col.transform.position.y + 1f )
        {
            _col.isTrigger = enabled;
        }

        else
        {
            _col.isTrigger = !enabled;
        }
    }
}

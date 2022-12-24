using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Giant : MonoBehaviour
{
    private bool isExist = true;
    private bool isGrounded = true;
    private Rigidbody2D myBody;

    private float jumpForce = 15f;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y <= -50 && isExist)
        {
            Debug.Log("Destroy Monster_Giant game object");
            Destroy(gameObject);
            isExist = false;
        }

        Jump();
    }

    void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("tempGround")) 
        {
            isGrounded = true;
        }
    }
}

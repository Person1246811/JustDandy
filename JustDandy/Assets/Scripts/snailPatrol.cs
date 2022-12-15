using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snailPatrol : MonoBehaviour
{
    public float speed;
    public float distance;
    public float health = 1;
    public Rigidbody2D myRb;

    private bool movingRight = true;

    public Transform groundDetechtion;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetechtion.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if(movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            health--;
        }
    }

}

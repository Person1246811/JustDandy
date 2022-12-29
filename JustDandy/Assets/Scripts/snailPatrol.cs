using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snailPatrol : MonoBehaviour
{
    public float speed;
    public float distance;
    public float health = 1;
    public Rigidbody2D myRb;
    private GameObject enemyDeath;

    private bool movingRight = true;

    public Transform groundDetection;

    private void Start()
    {
        enemyDeath = GameObject.Find("enemy death");
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
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
            enemyDeath.GetComponent<AudioSource>().Play();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleController : MonoBehaviour
{
    public bool isPatrol = true;
    public bool nonFollow = true;

    public Rigidbody2D myRb;

    public float moveSpeed;
    public float targetTime = 10.0f;
    public float patrolSpeed;
    public float health = 2;

    public Transform target;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;

        if (nonFollow == true)
        {
            if (isPatrol)
            {
                Patrol();

                if (targetTime <= 0.0f)
                {
                    Flip();
                    targetTime = 10.0f;
                }
            }
        }

        else if(nonFollow == false)
        {
            Vector2 lookPos = target.position - transform.position;
            myRb.rotation = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            
            myRb.velocity = lookPos * moveSpeed;
        }

        if (health == 0)
        {
            Destroy(gameObject);
        }
    }

    
    void Patrol()
    {
        myRb.velocity = new Vector2(patrolSpeed * Time.fixedDeltaTime, myRb.velocity.y);
    }

    void Flip()
    {
        isPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        patrolSpeed *= -1;
        isPatrol = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && collision.GetComponent<CircleCollider2D>() != null)
        {
            nonFollow = false;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.GetComponent<CircleCollider2D>() != null)
        {
            nonFollow = true;
            isPatrol = true;
            myRb.velocity = new Vector2(0, 0);
            gameObject.transform.rotation = Quaternion.identity;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleController : MonoBehaviour
{
    public float movePower = 300f;
    public float moveRate = -1f;
    [SerializeField] private float moveTimer = 3f;
    public Rigidbody2D myRB;
    public float health = 2;
    public Transform Player;

    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > moveRate)
        {
            myRB.velocity = Vector2.zero;
            Move();

            if (direction == 1)
                direction = -1;
            else if (direction == -1)
                direction = 1;
        }

        if(health == 0)
        {
            Destroy(gameObject);
        }

    }

    void Move()
    {
        myRB.AddForce(new Vector2(direction, 0) * movePower);
        moveRate = Time.time + moveTimer;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            health--;
        }
    }

    private void OnTriggerEnter2D(CircleCollider2D collision)
    {
        transform.LookAt(Player);

        transform.position += transform.forward * movePower * Time.deltaTime;
    }
}
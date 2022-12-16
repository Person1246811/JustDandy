using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidController : MonoBehaviour
{
    public float jumpPower = 10f;
    public float jumpRate = 1000f;
    public bool jumpTowards = false;
    [SerializeField] private float jumpTimer = 0.15f;
    public Rigidbody2D myRB;
    public float health = 1;
    public Transform player;
    public AudioClip deathClip;

    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > jumpRate)
        {
            if (jumpTowards)
            {
                if (transform.position.x > player.transform.position.x)
                    direction = -1;

                if (transform.position.x < player.transform.position.x)
                    direction = 1;
            }
            else
            {
                if (direction == 1)
                    direction = -1;
                else if (direction == -1)
                    direction = 1;
            }

            Jump();
        }

        if (myRB.velocity.x > .1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (myRB.velocity.x < -.1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathClip, transform.position);
        }
        
    }

    void Jump()
    {
        myRB.AddForce(new Vector2(direction*1, 1) * jumpPower);
        jumpRate = Time.time + jumpTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            jumpTowards = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            jumpTowards = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            Destroy(collision.gameObject);
            health -= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().damage;
        }
    }
}

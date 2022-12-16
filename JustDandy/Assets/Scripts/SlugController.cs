using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{

    public Rigidbody2D myRb;
    public Animator myAnim;

    public float health = 3;

    public  Transform player;

    public float playerRange = 2;
    public float yDistance = 12;

    public float moveSpeed = 10;

    public GameObject bullet;

    public float bulletSpeed = 5;
    public float bulletLife = 1;

    private bool canShoot = true;

    private float fireCountdown = 0;
    public float fireRate = 1;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        float dist = transform.position.x - player.transform.position.x;
        if (dist < playerRange && dist > -playerRange && player.transform.position.y - transform.position.y < yDistance)
        {
            if (transform.position.x < player.transform.position.x)
                GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * moveSpeed, 0);
            if (transform.position.x > player.transform.position.x)
                GetComponent<Rigidbody2D>().velocity = new Vector2(1 * moveSpeed, 0);
        }

        if (health <= 0)
            Destroy(gameObject);

        if (!canShoot)
        {
            fireCountdown += Time.deltaTime;
            if (fireCountdown >= fireRate)
            {
                fireCountdown = 0;
                canShoot = true;
            }
        }

        if (myRb.velocity.x > .1)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (myRb.velocity.x < -.1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canShoot)
        {
            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), b.GetComponent<CircleCollider2D>());
            Vector2 lookPos = player.position - transform.position;
            b.GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            b.GetComponent<Rigidbody2D>().velocity = lookPos * bulletSpeed;
            canShoot = false;
            Destroy(b, bulletLife);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRB;
    public GameObject bullet;
    public GameObject Crosshair;
    public GameObject slash;
    public Transform slashReach;
    public GameObject gameManager;

    public float hp = 5;
    public float maxhp = 5;
    public float damage = 1;
    private float angle = 0;

    public float slashLifespan = .2f;
    public float bulletSpeed = 1000;
    public float fireRate = .5f;
    public float bulletLifespan = 1;
    private float fireCountdown = 0;
    private bool canShoot = true;
    public float burstRate = .075f;
    public int burstSize = 2;

    public float moveSpeed = 10;
    public float jumpHeight = 15;
    public float groundDetectDistance = -.3f;

    private float gravityScaleBase = 0;

    private bool doubleJumpReady = true;

    public float Pollen = 0;
    public float Stage = 1;
    private bool GrowthDone = false;
    
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        Crosshair = GameObject.Find("Crosshair");
        gameManager = GameObject.Find("GameManager");
        gravityScaleBase = GetComponent<Rigidbody2D>().gravityScale;
    }
    
    void Update()
    {
        //mousePos on screen
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 distance = new Vector2(transform.position.y - mousePos.y, transform.position.x - mousePos.x);
        //rotation towards mouse
        angle = (Mathf.Atan2(distance.x, distance.y) * Mathf.Rad2Deg) + 180;
        //Right = direction
        myRB.rotation = angle;

        Crosshair.transform.position = new Vector2(mousePos.x, mousePos.y);

        Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y - .51f);
        Vector2 tempVelocity = myRB.velocity;
        tempVelocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && Physics2D.Raycast(raycastPos, Vector2.down, groundDetectDistance, 3))
            tempVelocity.y = jumpHeight;

        if (Stage >= 1)
        {
            if (tempVelocity.y >= 0)
                GetComponent<Rigidbody2D>().gravityScale = gravityScaleBase;
            else if (tempVelocity.y < 0)
                GetComponent<Rigidbody2D>().gravityScale = .25f;
            if (Stage == 1 && !GrowthDone)
            {
                hp = 5;
                maxhp = 5;
                damage = 1;
                GrowthDone = true;
            }
        }

        if (Stage >= 2)
        {
            if (Input.GetKeyDown(KeyCode.Space) && doubleJumpReady && !Physics2D.Raycast(raycastPos, Vector2.down, groundDetectDistance, 3))
            {
                tempVelocity.y = jumpHeight;
                doubleJumpReady = false;
            }
            if (Physics2D.Raycast(raycastPos, Vector2.down, groundDetectDistance, 3))
                doubleJumpReady = true;
            if (Stage == 2 && !GrowthDone)
            {
                hp = 10;
                maxhp = 10;
                damage = 1;
                GrowthDone = true;
            }
        }

        if (Stage == 3 && !GrowthDone)
        {
            hp = 15;
            maxhp = 15;
            damage = 2;
            GrowthDone = true;
        }

        if (Stage == 4 && !GrowthDone)
        {
            hp = 20;
            maxhp = 20;
            damage = 2;
            GrowthDone = true;
        }

        if (canShoot)
        {
            if (Input.GetKey(KeyCode.Mouse0) && Stage >= 3)
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), b.GetComponent<PolygonCollider2D>());
                b.GetComponent<Rigidbody2D>().rotation = angle;
                b.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * bulletSpeed);
                canShoot = false;
                Destroy(b, bulletLifespan);
                //Shoots the Burst
                StartCoroutine(BurstDelay(b.GetComponent<PolygonCollider2D>()));
                
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                GameObject s = Instantiate(slash, transform);
                s.GetComponent<Rigidbody2D>().rotation = angle;
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), s.GetComponent<CapsuleCollider2D>());
                s.transform.position = slashReach.position;
                canShoot = false;
                Destroy(s, slashLifespan);
            }
        }

        else if (!canShoot)
        {
            fireCountdown += Time.deltaTime;
            if (fireCountdown >= fireRate)
            {
                fireCountdown = 0;
                canShoot = true;
            }
        }

        myRB.velocity = tempVelocity;

        if (hp <= 0)
        {
            gameManager.GetComponent<GameManager>().LoadLevel(1);
        }
    }

    IEnumerator BurstDelay(PolygonCollider2D firstBulletCollider)
    {
        if (Stage >= 4)
        {
            for (int i = 0;i < burstSize;i++)
            {
                yield return new WaitForSeconds(burstRate);
                GameObject b2 = Instantiate(bullet, transform.position, Quaternion.identity);
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), b2.GetComponent<PolygonCollider2D>());
                if (firstBulletCollider != null)
                    Physics2D.IgnoreCollision(firstBulletCollider, b2.GetComponent<PolygonCollider2D>());
                b2.GetComponent<Rigidbody2D>().rotation = angle;
                b2.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * bulletSpeed);
                
                Destroy(b2, bulletLifespan);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Aphid
        if ((collision.gameObject.tag == "Enemy1"))
        {
            hp--;
        }

        //Beetle
        if ((collision.gameObject.tag == "Enemy2"))
        {
            hp -= 2;
        }

        //Snail or Slug
        if ((collision.gameObject.tag == "Enemy3"))
        {
            hp -= 4;
        }

        //Snail or Slug
        if ((collision.gameObject.tag == "Enemy4"))
        {
            hp -= 4;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Pollen"))
        {
            Destroy(collision.gameObject);
            Pollen++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "SunLight") && Pollen >= 5)
        {
            Pollen -= 5;
            GrowthDone = false;
            Stage++;
        }
    }
}
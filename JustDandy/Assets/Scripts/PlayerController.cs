using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRB;
    public GameObject bullet;
    public GameObject slash;
    public Transform slashReach;

    public float slashLifespan = .2f;

    public float bulletSpeed = 1000;
    public float fireRate = .5f;
    private float bulletLifespan = 1;
    private float fireCountdown = 0;
    private bool canShoot = true;

    public float moveSpeed = 10;
    public float jumpHeight = 15;
    public float groundDetectDistance = -.3f;

    private float gravityScaleBase = 0;
    private bool glide = true;

    private bool doubleJumpReady = true;

    public float Stage = 1;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        gravityScaleBase = GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //mousePos on screen
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 distance = new Vector2(transform.position.y - mousePos.y, transform.position.x - mousePos.x);
        //rotation towards mouse
        float angle = (Mathf.Atan2(distance.x, distance.y) * Mathf.Rad2Deg) + 180;
        //Right = direction
        myRB.rotation = angle;

        Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y - .51f);
        Vector2 tempVelocity = myRB.velocity;
        tempVelocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && Physics2D.Raycast(raycastPos, Vector2.down, groundDetectDistance, 3))
            tempVelocity.y = jumpHeight;

        if (glide)
        {
            if (tempVelocity.y >= 0)
                GetComponent<Rigidbody2D>().gravityScale = gravityScaleBase;
            else if (tempVelocity.y < 0)
                GetComponent<Rigidbody2D>().gravityScale = .25f;
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
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                GameObject s = Instantiate(slash, slashReach.position, Quaternion.identity);
                s.GetComponent<Rigidbody2D>().rotation = angle;
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), s.GetComponent<CapsuleCollider2D>());
                canShoot = false;
                s.transform.position = slashReach.position;
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
    }
}

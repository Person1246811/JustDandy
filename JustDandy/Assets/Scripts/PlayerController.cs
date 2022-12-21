using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRB;
    public GameObject bullet;
    public GameObject Crosshair;
    public GameObject slash;
    public GameObject gameManager;
    public Animator myAnim;
    public AudioClip damageSound;
    public AudioClip jumpSound;
    public AudioClip pollenSound;
    public AudioClip shootSound;

    public float hp = 5;
    public float maxhp = 5;
    public float damage = 1;
    private float angle = 0;
    private bool enemyAttack = true;
    public float enemyAttackCooldown = .5f;
    private float enemyAttackCountdown = 0;

    public float SlashOffRate = .2f;
    public float slashLifespan = .2f;
    public float bulletSpeed = 1000;
    public float fireRate = .5f;
    public float bulletLifespan = 1;
    private float fireCountdown = 0;
    private bool canShoot = true;
    public float burstRate = .075f;
    public int burstSize = 2;

    public float moveSpeed = 10;
    public float jumpHeight = 12;
    public float groundDetectDistance = -.3f;
    public bool allowedToMove = true;

    private float gravityScaleBase = 0;
    public float glideAmount = .25f;
    private bool doubleJumpReady = true;

    public int Pollen = 0;
    public int Stage = 1;
    private bool GrowthDone = false;
    
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        Crosshair = GameObject.Find("Crosshair");
        slash = GameObject.Find("Slash");
        gameManager = GameObject.Find("GameManager");
        gravityScaleBase = GetComponent<Rigidbody2D>().gravityScale;
        myAnim = GetComponent<Animator>();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerPrefs.SetFloat("Health", 5);
            PlayerPrefs.SetInt("Stage", 1);
            PlayerPrefs.SetInt("Pollen", 0);
        }
        hp = PlayerPrefs.GetFloat("Health", 5);
        Stage = PlayerPrefs.GetInt("Stage", 1);
        Pollen = PlayerPrefs.GetInt("Pollen", 0);
    }
    
    void Update()
    {
        //mousePos on screen
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 distance = new Vector2(transform.position.y - mousePos.y, transform.position.x - mousePos.x);
        //rotation towards mouse
        angle = (Mathf.Atan2(distance.x, distance.y) * Mathf.Rad2Deg) + 180;

        slash.GetComponent<Rigidbody2D>().rotation = angle;

        Crosshair.transform.position = new Vector2(mousePos.x, mousePos.y);

        Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y - .51f);
        Vector2 tempVelocity = myRB.velocity;
        tempVelocity.x = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && Physics2D.Raycast(raycastPos, Vector2.down, groundDetectDistance, 3))
        {
            tempVelocity.y = jumpHeight;
            GetComponent<AudioSource>().PlayOneShot(jumpSound);
        }

        if (Stage >= 1)
        {
            if (tempVelocity.y >= 0)
                GetComponent<Rigidbody2D>().gravityScale = gravityScaleBase;
            else if (tempVelocity.y < 0)
                GetComponent<Rigidbody2D>().gravityScale = glideAmount;
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
                GetComponent<AudioSource>().PlayOneShot(shootSound);
                Destroy(b, bulletLifespan);
                //Shoots the Burst
                StartCoroutine(BurstDelay(b.GetComponent<PolygonCollider2D>()));

            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                slash.GetComponent<SpriteRenderer>().enabled = true;
                slash.GetComponent<CapsuleCollider2D>().enabled = true;
                canShoot = false;
                // Add delay to slash staying active
                StartCoroutine(SlashOff(slash));
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

        if (allowedToMove)
        {
            myRB.velocity = tempVelocity;
            if (myRB.velocity.x > .1)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (myRB.velocity.x < -.1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
            myRB.velocity = new Vector2(0, 0);

        if (!enemyAttack)
        {
            enemyAttackCountdown += Time.deltaTime;
            if (enemyAttackCountdown >= enemyAttackCooldown)
            {
                enemyAttackCountdown = 0;
                enemyAttack = true;
            }
        }

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
                GetComponent<AudioSource>().PlayOneShot(shootSound);
                Destroy(b2, bulletLifespan);
            }
        }
    }

    IEnumerator SlashOff(GameObject slash)
    {
        yield return new WaitForSeconds(SlashOffRate);
        slash.GetComponent<SpriteRenderer>().enabled = false;
        slash.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (enemyAttack)
        {
            //Aphid
            if ((collision.gameObject.tag == "Enemy1"))
            {
                enemyAttack = false;
                hp--;
                GetComponent<AudioSource>().PlayOneShot(damageSound);
            }

            //Beetle
            if ((collision.gameObject.tag == "Enemy2"))
            {
                enemyAttack = false;
                hp -= 2;
                GetComponent<AudioSource>().PlayOneShot(damageSound);
            }

            //Snail or Slug
            if ((collision.gameObject.tag == "Enemy3"))
            {
                enemyAttack = false;
                hp -= 4;
                GetComponent<AudioSource>().PlayOneShot(damageSound);
            }

            //Snail or Slug
            if ((collision.gameObject.tag == "Enemy4Attack"))
            {
                Destroy(collision.gameObject);
                enemyAttack = false;
                hp -= 2;
                GetComponent<AudioSource>().PlayOneShot(damageSound);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Pollen"))
        {
            Destroy(collision.gameObject);
            Pollen++;
            GetComponent<AudioSource>().PlayOneShot(pollenSound);
        }

        if ((collision.gameObject.tag == "Teleport") && (GameObject.FindGameObjectsWithTag("Pollen").Length <= 0))
        {
            PlayerPrefs.SetFloat("Health", hp);
            PlayerPrefs.SetInt("Stage", Stage);
            PlayerPrefs.SetInt("Pollen", Pollen);
            gameManager.GetComponent<GameManager>().LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
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

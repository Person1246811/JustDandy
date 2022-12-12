using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{

    public Rigidbody2D myRb;

    public  GameObject player;

    public float moveSpeed = 10;

    public GameObject bullet;

    

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(bullet);
            Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), bullet.GetComponent<CircleCollider2D>());
        }



    }
}

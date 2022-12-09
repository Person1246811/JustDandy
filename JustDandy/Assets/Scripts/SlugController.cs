using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{

    public Rigidbody2D myRb;

    public  GameObject player;

    public float moveSpeed = 10;

    public float Distance = 0;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        Distance = player.transform.position.x - transform.position.x;
        if (Distance >= 2.5f || Distance <= -2.5f)
            Debug.Log(Distance);
        else
            Debug.Log("It works");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            myRb.AddForce(new Vector2(player.transform.position.x * moveSpeed, 0));
        }
    }
}

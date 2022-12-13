using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public bool isBackground = false;
    public Rigidbody2D background;
    public float backgroundSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //fix bug where the background moves when the player moves against a wall
        if (isBackground && GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            Vector2 tempVelocity = background.velocity;
            tempVelocity.x = Input.GetAxisRaw("Horizontal") * backgroundSpeed;
            background.velocity = tempVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Attack" && collision.gameObject.name != "Slash" && !isBackground)
        {
            Destroy(collision.gameObject);
        }
    }
}

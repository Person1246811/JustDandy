using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidController : MonoBehaviour
{
    public float jumpPower = 10f;
    public float jumpRate = 1000f;
    [SerializeField] private float jumpTimer = 0.15f;
    public Rigidbody2D myRB;
    public bool jumpright = true;
    public float health = 1;

    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > jumpRate)
        {
            Jump();

            if (direction == 1)
                direction = -1;
            else if (direction == -1)
                direction = 1;
        }
    }

    void Jump()
    {
        myRB.AddForce(new Vector2(direction*1, 1) * jumpPower);
        jumpRate = Time.time + jumpTimer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            health --;
        }

    }
}

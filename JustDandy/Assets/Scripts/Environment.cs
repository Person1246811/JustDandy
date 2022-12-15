using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public bool isParallax = false;
    public bool isImpactDestroy = false;
    private float startposX, startposY;
    public GameObject cam;
    public float parallax;

    // Start is called before the first frame update
    void Start()
    {
        if (isParallax)
        {
            startposX = transform.position.x;
            startposY = transform.position.y;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isParallax)
        {
            transform.position = new Vector2(startposX + cam.transform.position.x * parallax, startposY + cam.transform.position.y * parallax);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Attack" && collision.gameObject.name != "Slash" && isImpactDestroy)
        {
            Destroy(collision.gameObject);
        }
    }
}

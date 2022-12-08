using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private bool IsCalled = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy2" && IsCalled)
        {
            collision.gameObject.GetComponent<BeetleController>().health--;
            IsCalled = false;
        }
    }
}

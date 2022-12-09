using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private bool CanAttack = true;
    private float AttackCooldown = .1f;
    private float AttackCountdown = 0;

    private void Update()
    {

        if (!CanAttack)
        {
            AttackCountdown += Time.deltaTime;
            if (AttackCountdown >= AttackCooldown)
            {
                AttackCountdown = 0;
                CanAttack = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy1" && CanAttack)
        {
            collision.gameObject.GetComponent<AphidController>().health--;
            CanAttack = false;
        }
        if (collision.gameObject.tag == "Enemy2" && CanAttack)
        {
            collision.gameObject.GetComponent<BeetleController>().health--;
            CanAttack = false;
        }
    }
}

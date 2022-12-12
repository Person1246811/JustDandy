using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject slug;
    public GameObject target;

    public float speed = 10f;

    private float slugX;
    private float targetX;
    private float dist;
    private float nextX;
    private float baseY;
    private float hieght;

    // Start is called before the first frame update
    void Start()
    {
        slug = GameObject.FindGameObjectWithTag("Enemy3");
        target = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        slugX = slug.transform.position.x;
        targetX = target.transform.position.y;

        dist = targetX - slugX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(slug.transform.position.y, target.transform.position.y, (nextX - slugX) / dist);
        hieght = 20f * (nextX - slugX) * (nextX - targetX) / (-0.25f * dist * dist);

        Vector3 movePosition = new Vector3(nextX, baseY + hieght, transform.position.z);
        transform.rotation = lookAtTarget(movePosition - transform.position);
        transform.position = movePosition;
    }

    public static Quaternion lookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}

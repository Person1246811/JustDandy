using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject aphid;
    public GameObject beetle;
    public GameObject slug;
    public GameObject snail;

    public int spawnSelect = 1;
    public int spawnAmount = 1;

    public float spawnRate = 1;
    private float spawnCountdown = 0;
    public bool spawn = false;
    public bool canSpawn = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAmount <= 0)
            Destroy(gameObject);

        if (spawn && canSpawn)
        {
            if (spawnSelect == 1)
            { 
                GameObject s = Instantiate(aphid, transform.position, Quaternion.identity); 
            }
            else if (spawnSelect == 2)
            { 
                GameObject s = Instantiate(beetle, transform.position, Quaternion.identity); 
            }
            else if (spawnSelect == 3)
            { 
                GameObject s = Instantiate(slug, transform.position, Quaternion.identity);
            }
            else if (spawnSelect == 4)
            { 
                GameObject s = Instantiate(snail, transform.position, Quaternion.identity); 
            }
            spawnAmount--;
            spawn = false;
        }

        else if (!spawn && spawnAmount > 0 && canSpawn)
        {
            spawnCountdown += Time.deltaTime;
            if (spawnCountdown >= spawnRate)
            {
                spawnCountdown = 0;
                spawn = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            canSpawn = true;
    }
}

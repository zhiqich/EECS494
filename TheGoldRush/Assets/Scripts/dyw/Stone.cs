using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int health = 3;
    // public double prob = 0.25;

    public GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Pickaxe") 
        {
            health -= Damage.hammer;
            if (health <= 0) 
            {
                Destroy(this.gameObject);
                int prob = Random.Range(0, 10);
                if (prob > 8) 
                {
                    Instantiate(item, transform.position, Quaternion.identity);
                }
            }   
        }
        if (other.tag == "Arrow") 
        {
            health -= Damage.shooter;
            if (health <= 0) 
            {
                Destroy(this.gameObject);
                int prob = Random.Range(0, 10);
                if (prob > 8) 
                {
                    Instantiate(item, transform.position, Quaternion.identity);
                }
            }   
        }
    }
}

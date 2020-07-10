using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMining : MonoBehaviour
{
    int fatigue = 5;
    public GameObject GoldBrick;

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
            fatigue -= 1;
        }
        if (fatigue == 0)
        {
            fatigue = 5;
            Instantiate(GoldBrick, transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0).normalized, Quaternion.identity);
        }
    }
}

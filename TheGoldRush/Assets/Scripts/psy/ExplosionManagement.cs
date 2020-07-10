using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManagement : MonoBehaviour
{
    float damagedelay = 0.65f;
    public PlayerBrain.Color color = PlayerBrain.Color.Blue;
    public int ownerID = 0;
    float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        StartCoroutine(DamageDelay());
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    // trigger other bombs
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (timer <= 0.1f)
        {
            GameObject other = coll.gameObject;
            if (other.CompareTag("bomb"))
            {
                other.GetComponent<BombManagement>().Explode();
            }
        }
    }

    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(damagedelay);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}

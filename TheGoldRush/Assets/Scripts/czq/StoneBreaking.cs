using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBreaking : MonoBehaviour
{
    public int fatigue = 3;
    public GameObject[] items;

    //public AudioSource broken;

    public AudioClip broken;

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
        if (other.tag == "Pickaxe" || other.tag == "Arrow" || other.tag == "BombExplosion")
        {
            fatigue -= Damage.hammer;
            OnAttacked();
        }
        if (fatigue <= 0)
        {
            AudioSource.PlayClipAtPoint(broken, transform.position);
            Destroy(this.gameObject);
            if (Random.Range(0, 10) >= 8)
            {
                GameObject goldbar = Instantiate(items[Random.Range(0, items.Length)], transform.position, Quaternion.identity);
                goldbar.GetComponent<GoldBrickFly>().target = transform.position;
            }
        }
    }

    private bool scaling = false;
    public float scale = 0.8f;
    private float startTime;
    public float halfPeriod = 0.1f;

    private void OnAttacked()
    {
        if (scaling)
        {
            return;
        }
        scaling = true;
        startTime = Time.time;
        StartCoroutine(Attacked());
    }

    IEnumerator Attacked()
    {
        while (Time.time - startTime < halfPeriod)
        {
            float s = 1 - (1 - scale) * Mathf.Sin((Time.time - startTime) * Mathf.PI / halfPeriod);
            transform.localScale = new Vector3(s, s, 1);
            yield return null;
        }
        transform.localScale = Vector3.one;
        scaling = false;
    }
}

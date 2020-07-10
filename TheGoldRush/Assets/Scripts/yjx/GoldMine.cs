using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : MonoBehaviour
{
    // Start is called before the first frame update

    public int MaxHealth = 5;

    private int health;

    public GameObject goldBar;
    private float originalScale;

    private SpriteRenderer sr;
    void Start()
    {
        health = MaxHealth;
        time = interval;
        originalScale = transform.localScale.x;
        sr = GetComponent<SpriteRenderer>();
    }

    public float interval = 10f;
    private float time;

    public AudioClip eject;

    // Update is called once per frame
    void Update()
    {
        //if (time >= interval)
        //{
        //    time = 0f;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Vector3 target = transform.position + 2.2f * new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0).normalized;
        //        GameObject goldbar = Instantiate(goldBar, transform.position, Quaternion.identity);
        //        goldbar.GetComponent<GoldBrickFly>().target = target;
        //    }
        //}
        //time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Pickaxe") || other.CompareTag("Arrow") || other.CompareTag("BombExplosion"))
        {
            health -= 1;
            OnAttacked();
        }
        if (health <= 0) 
        {
            AudioSource.PlayClipAtPoint(eject, transform.position, 3f);
            Camera.main.GetComponent<Animator>().SetTrigger("shake");
            for (int i = 0; i < 5; i++) 
            {
                Vector3 target = transform.position + 2.2f * new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0).normalized;
                GameObject goldbar = Instantiate(goldBar, transform.position, Quaternion.identity);
                goldbar.GetComponent<GoldBrickFly>().target = target;
            }
            health = MaxHealth;
            sr.color = Color.white;
        }
    }

    private bool scaling = false;
    public float scale = 0.8f;
    private float startTime;
    public float halfPeriod = 0.1f;
    public int ejectionGB = 128;

    private void OnAttacked()
    {
        float gb = (255 - (255 - ejectionGB) * (1 - (float)health / (float)MaxHealth)) / 255f;
        Color c = new Color(1, gb, gb);
        sr.color = c;
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
        //float tempSize = transform.localScale.x;
        while (Time.time - startTime < halfPeriod)
        {
            float s = originalScale * (1 - (1 - scale) * Mathf.Sin((Time.time - startTime) * Mathf.PI / halfPeriod));
            //float s = tempSize * (1 - (1 - scale) * Mathf.Sin((Time.time - startTime) * Mathf.PI / halfPeriod));
            transform.localScale = new Vector3(s, s, 1);
            yield return null;
        }
        transform.localScale = new Vector3(originalScale, originalScale, 1);
        scaling = false;
    }
}

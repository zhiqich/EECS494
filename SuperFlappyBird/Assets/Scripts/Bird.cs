using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public int playerNumber;
    public float upForce = 200f;

    private bool isDead = false;
    private bool isDead2 = false;
    private Rigidbody2D rb2d;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false && Input.GetMouseButtonDown(0) && playerNumber == 1)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(0, upForce));
            anim.SetTrigger("Flap");
        }
        if (isDead2 == false && Input.GetKeyDown(KeyCode.Space) && playerNumber == 2)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(0, upForce));
            anim.SetTrigger("Flap");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead == false && playerNumber == 1)
        {
            rb2d.velocity = Vector2.zero;
            isDead = true;
            anim.SetTrigger("Die");
            GameControl.instance.BirdDied(1);
        }
        if (isDead2 == false && playerNumber == 2)
        {
            rb2d.velocity = Vector2.zero;
            isDead2 = true;
            anim.SetTrigger("Die");
            GameControl.instance.BirdDied(2);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Sprite up;
    public Sprite left;
    public Sprite right;
    public int dir = 0;
    float currentTime;
    float startTime;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        if (currentTime - startTime >= 0.2f)
        {
            Destroy(gameObject);
        }
        switch (dir)
        {
            case 0:
                // GetComponent<SpriteRenderer>().sprite = up;
                animator.SetInteger("dir", dir);
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = up;
                animator.SetInteger("dir", dir);
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = left;
                animator.SetInteger("dir", dir);
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = right;
                animator.SetInteger("dir", dir);
                break;
            default:
                break;
        }
    }
}

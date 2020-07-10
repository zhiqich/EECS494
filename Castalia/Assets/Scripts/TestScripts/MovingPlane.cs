using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlane : MonoBehaviour
{
    public Vector3 target_position;
    public bool is_moving = false;
    public GameObject trigger;

    private Vector3 origin_position;

    Coroutine current_movement;

    // Start is called before the first frame update
    void Start()
    {
        origin_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger.GetComponent<TestTrigger>().is_triggered == true)
        {
            if (current_movement != null)
            {
                StopCoroutine(current_movement);
            }
            current_movement = StartCoroutine(Triggered());
        }
        else
        {
            if (current_movement != null)
            {
                StopCoroutine(current_movement);
            }
            current_movement = StartCoroutine(Released());
        }
    }

    IEnumerator Triggered()
    {
        while (transform.position != target_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_position, 0.1f * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Released()
    {
        while (transform.position != origin_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, origin_position, 0.1f * Time.deltaTime);
            yield return null;
        }
    }
}

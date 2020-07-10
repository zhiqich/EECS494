using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 target_position;
    public float triggered_speed;
    public float released_speed;
    public bool movable = true;
    public bool stoppable = true;
    public bool is_equipped = false;
    public bool triggered = false;
    public bool released = false;
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
        if (movable == true)
        {
            if (trigger.GetComponent<TriggerPlatform>().is_triggered == true)
            {
                if (triggered == false)
                {
                    if (current_movement != null)
                    {
                        StopCoroutine(current_movement);
                    }
                    current_movement = StartCoroutine(Triggered());
                }
            }
            else
            {
                if (released == false)
                {
                    if (current_movement != null)
                    {
                        StopCoroutine(current_movement);
                    }
                    current_movement = StartCoroutine(Released());
                }
            }
        }
    }

    IEnumerator Triggered()
    {
        released = false;
        triggered = true;
        while (transform.position != target_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_position, triggered_speed * Time.deltaTime);
            yield return null;
        }
        is_equipped = true;
        if (stoppable == true)
        {
            movable = false;
        }
    }

    IEnumerator Released()
    {
        triggered = false;
        released = true;
        while (transform.position != origin_position)
        {
            transform.position = Vector3.MoveTowards(transform.position, origin_position, released_speed * Time.deltaTime);
            is_equipped = false;
            yield return null;
        }
    }

    public void StopCurrent()
    {
        if (GetComponent<MovingPlatform>().current_movement != null)
        {
            StopCoroutine(GetComponent<MovingPlatform>().current_movement);
        }
    }
}

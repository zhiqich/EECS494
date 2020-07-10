using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlatform : MonoBehaviour
{
    public bool is_triggered = false;
    // public GameObject[] targets;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Robot")
        {
            is_triggered = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Robot")
        {
            is_triggered = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            is_triggered = true;
        }
        // Debug.Log("Triggered");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            is_triggered = false;
        }
        // Debug.Log("Released");
    }
}

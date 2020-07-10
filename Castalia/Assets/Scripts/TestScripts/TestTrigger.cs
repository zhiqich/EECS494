using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
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

    private void OnTriggerStay(Collider other)
    {
        is_triggered = true;
        // Debug.Log("Triggered");
    }

    private void OnTriggerExit(Collider other)
    {
        is_triggered = false;
        // Debug.Log("Released");
    }
}

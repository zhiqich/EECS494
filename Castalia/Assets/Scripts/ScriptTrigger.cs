using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTrigger : MonoBehaviour
{
    public GameObject t2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot"))
        {
            t2.GetComponent<MoveWithRobot>().enabled = true;
            Destroy(gameObject);
        }
    }
}

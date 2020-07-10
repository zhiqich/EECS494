using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    public bool is_visible = true;

    // Start is called before the first frame update
    void Start()
    {
        if (is_visible == false)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<MovingPlatform>().trigger.GetComponent<TriggerPlatform>().is_triggered == true && is_visible == false)
        {
            is_visible = true;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    Vector3 diff;

    // Start is called before the first frame update
    void Start()
    {
        diff = Camera.main.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position - diff;
    }
}

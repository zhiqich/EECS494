using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTriangle : MonoBehaviour
{
    public GameObject t1;
    public GameObject t2;
    public GameObject t3;

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
        t1.GetComponent<TriangleRotation>().RotationTriangle();
        t2.GetComponent<TriangleRotation>().RotationTriangle();
        t3.GetComponent<TriangleRotation>().RotationTriangle();
    }
}

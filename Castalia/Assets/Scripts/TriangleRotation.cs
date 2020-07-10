using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleRotation : MonoBehaviour
{
    public Transform[] rotation_z;
    public int state = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotationTriangle()
    {
        state = (state + 1) % rotation_z.Length;
        // Debug.Log(rotation_z[state].eulerAngles.z);
        transform.rotation = rotation_z[state].rotation;
        // while (transform.rotation != rotation_z[state].rotation)
        // {
        //     transform.rotation = Quaternion.Lerp(transform.rotation, rotation_z[state].rotation, Time.deltaTime * 1.0f);
        // }
    }
}

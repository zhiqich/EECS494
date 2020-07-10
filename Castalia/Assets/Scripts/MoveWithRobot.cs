using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithRobot : MonoBehaviour
{
    public GameObject robot;
    public float smooth = 12.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (robot.transform.position.z <= 6.0f && robot.transform.position.z >= -6.0f)
        {
            Vector3 smooth_position = Vector3.Lerp(transform.position, new Vector3(0.0f, 0.0f, robot.transform.position.z), smooth * Time.deltaTime);
            transform.position = smooth_position;
        }
    }
}

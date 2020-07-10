using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFollower : MonoBehaviour
{
    public GameObject robot;
    public Vector3 offset_position;

    // Start is called before the first frame update
    void Start()
    {
        // offset_position = transform.position - robot.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        transform.position = robot.transform.position + offset_position;
        transform.LookAt(robot.transform.position);
    }

    public void ManualUpdate()
    {
        transform.position = robot.transform.position + offset_position;
        transform.LookAt(robot.transform.position);
    }
}

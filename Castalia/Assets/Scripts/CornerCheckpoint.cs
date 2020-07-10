using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCheckpoint : MonoBehaviour
{
    public bool rotation_done = false;
    public int target_direction = 1;
    public GameObject robot;

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
        if (other.CompareTag("Robot") && rotation_done == false)
        {
            rotation_done = true;
            robot.GetComponent<RobotDirection>().dir = target_direction;
            if (target_direction != Camera.main.GetComponent<CameraController>().current_follow_state)
            {
                Camera.main.GetComponent<CameraController>().CameraRotation(target_direction);
            }
        }
    }
}

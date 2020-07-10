using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool follow_robot = true;
    public bool whole_view = false;
    public GameObject[] camera_set_points;
    public int level;
    public GameObject robot_follower;
    public float transit_speed = 10.0f;
    public bool is_changing = false;
    public Vector3[] offset_positions = { new Vector3(0.0f, 3.0f, -5.0f), new Vector3(-5.0f, 3.0f, 0.0f), new Vector3(0.0f, 3.0f, 5.0f), new Vector3(5.0f, 3.0f, 0.0f) };
    public int current_follow_state = 0;
    public int prev_dir = 0;
    public bool which_key = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && is_changing == false)
        {
            which_key = true;
            if (follow_robot == true)
            {
                prev_dir = GetComponent<FollowPlayer>().robot.GetComponent<RobotDirection>().dir;
                GetComponent<FollowPlayer>().robot.GetComponent<RobotDirection>().dir = camera_set_points[level].GetComponent<CameraSetPointDir>().dir;
                follow_robot = false;
                StartCoroutine(CameraTransit(camera_set_points[level].transform, true));
            }
            else
            {
                GetComponent<FollowPlayer>().robot.GetComponent<RobotDirection>().dir = prev_dir;
                whole_view = false;
                StartCoroutine(CameraTransit(robot_follower.transform, false));
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && is_changing == false && whole_view == false)
        {
            which_key = false;
            GetComponent<FollowPlayer>().robot.GetComponent<RobotDirection>().dir = (GetComponent<FollowPlayer>().robot.GetComponent<RobotDirection>().dir + 1) % 4;
            current_follow_state = (current_follow_state + 1) % 4;
            follow_robot = false;
            robot_follower.GetComponent<RobotFollower>().offset_position = offset_positions[current_follow_state];
            robot_follower.GetComponent<RobotFollower>().ManualUpdate();
            GetComponent<FollowPlayer>().offset_position = offset_positions[current_follow_state];
            StartCoroutine(CameraTransit(robot_follower.transform, false));
        }
    }

    IEnumerator CameraTransit(Transform target, bool view)
    {
        is_changing = true;
        while (transform.position != target.position || transform.rotation != target.rotation)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * transit_speed);
            // transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * transit_speed);
            if (which_key == true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * transit_speed);
            }
            else
            {
                transform.LookAt(GetComponent<FollowPlayer>().robot.transform.position);
            }
            yield return null;
        }
        if (view == true)
        {
            whole_view = true;
        }
        else
        {
            follow_robot = true;
        }
        is_changing = false;
    }

    public void CameraRotation(int target_direction)
    {
        if (is_changing == false)
        {
            current_follow_state = target_direction;
            follow_robot = false;
            robot_follower.GetComponent<RobotFollower>().offset_position = offset_positions[current_follow_state];
            robot_follower.GetComponent<RobotFollower>().ManualUpdate();
            GetComponent<FollowPlayer>().offset_position = offset_positions[current_follow_state];
            StartCoroutine(CameraTransit(robot_follower.transform, false));
        }
    }
}

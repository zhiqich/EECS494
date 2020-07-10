using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    public float smooth = 25.0f;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.GetComponent<CameraController>().whole_view == false && Camera.main.GetComponent<CameraController>().follow_robot == false)
        {
            return;
        }
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        float temp = 0.0f;
        switch (GetComponent<RobotDirection>().dir)
        {
            case 1:
                temp = moveHorizontal;
                moveHorizontal = moveVertical;
                moveVertical = -temp;
                break;
            case 2:
                moveHorizontal = -moveHorizontal;
                moveVertical = -moveVertical;
                break;
            case 3:
                temp = moveHorizontal;
                moveHorizontal = -moveVertical;
                moveVertical = temp;
                break;
            default:
                break;
        }
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
            // Vector3 dir = Camera.main.transform.TransformDirection(new Vector3(moveHorizontal, 0, moveVertical));
            Vector3 dir = new Vector3(moveHorizontal, 0, moveVertical);
            Quaternion rotation_target = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation_target, Time.deltaTime * smooth);
        }
    }
}

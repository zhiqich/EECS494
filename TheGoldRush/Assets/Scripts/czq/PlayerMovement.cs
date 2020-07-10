using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private PlayerBrain pb;
    private int playerId = 0;

    private Vector2 i_movement;
    private float speed;

    private Animator animator;

    private PlayerInventory pi;

    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBrain>();
        playerId = pb.PlayerID;
        animator = GetComponent<Animator>();
        pi = GetComponent<PlayerInventory>();
        speed = pb.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (pb.freezed)
        {
            return;
        }
        Move();
    }

    void Move()
    {
        if (pb.stunned)
        {
            return;
        }
        speed = pb.speed * (1 - pi.getNumGoldBar() * 0.16f);
        bool changeDir = false;
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            if (pb.dir != PlayerBrain.Direction.Left)
            {
                pb.dir = PlayerBrain.Direction.Left;
                changeDir = true;
            }
            movement = new Vector3(-1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (pb.dir != PlayerBrain.Direction.Right)
            {
                pb.dir = PlayerBrain.Direction.Right;
                changeDir = true;
            }
            movement = new Vector3(1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (pb.dir != PlayerBrain.Direction.Up)
            {
                pb.dir = PlayerBrain.Direction.Up;
                changeDir = true;
            }
            movement = new Vector3(0, 1, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (pb.dir != PlayerBrain.Direction.Down)
            {
                pb.dir = PlayerBrain.Direction.Down;
                changeDir = true;
            }
            movement = new Vector3(0, -1, 0);
        }
        bool moving = false;
        if (changeDir)
        {
            animator.SetInteger("Dir", (int)pb.dir);
        }
        else if (movement != Vector3.zero)
        {
            moving = true;
        }
        animator.SetBool("Moving", moving);
        //Debug.Log("" + i_movement + movement);
        if (moving)
        {
            movement *= speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    Vector3 getMovement()
    {
        Vector3 movement = new Vector3();
        float x = i_movement.x;
        float y = i_movement.y;
        if (x * x + y * y <= 0.2)
        {
            return movement;
        }
        if (x > 0 && y > -x && y <= x)
        {
            movement.x = 1;
        }
        else if (y < 0 && x <= -y && x > y)
        {
            movement.y = -1;
        }
        else if (x < 0 && y < -x && y >= x)
        {
            movement.x = -1;
        }
        else if (y > 0 && x >= -y && x < y)
        {
            movement.y = 1;
        }
        return movement;
    }

    // void OnMove(InputValue move)
    // {
    //     i_movement = move.Get<Vector2>();
    // }
    public void GetInput()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal_input) > 0.0f)
        {
            vertical_input = 0.0f;
        }
        i_movement = new Vector2(horizontal_input, vertical_input);
    }


}

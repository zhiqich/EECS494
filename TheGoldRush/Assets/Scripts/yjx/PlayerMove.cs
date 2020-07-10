using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using ParsecUnity;

public class PlayerMove : MonoBehaviour
{

    private PlayerBrain pb;
    private int playerId = 0;

    private Vector2 i_movement;
    private float speed;

    //private Animator animator;

    private PlayerInventory pi;

    private Rigidbody2D rb2d;

    public ParticleSystem dust;

    // Start is called before the first frame update
    void Start()
    {
        pb = GetComponent<PlayerBrain>();
        playerId = pb.PlayerID;
        pi = GetComponent<PlayerInventory>();
        speed = pb.speed;
        rb2d = GetComponent<Rigidbody2D>();
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
        bool moving = false;
        speed = pb.speed * (1 - pi.getNumGoldBar() * 0.16f);
        Vector3 movement = Vector3.zero;
        if (pb.moving) 
        {
            CreateDust();
            if (ParsecInput.GetKey(pb.PlayerID, KeyCode.A) && pb.dir == PlayerBrain.Direction.Left) 
            {
                movement = new Vector3(-1, 0, 0);
            }
            else if (ParsecInput.GetKey(pb.PlayerID, KeyCode.D) && pb.dir == PlayerBrain.Direction.Right)
            {
                
                movement = new Vector3(1, 0, 0);
            }
            else if (ParsecInput.GetKey(pb.PlayerID, KeyCode.W) && pb.dir == PlayerBrain.Direction.Up)
            {
                     
                movement = new Vector3(0, 1, 0);
            }
            else if (ParsecInput.GetKey(pb.PlayerID, KeyCode.S)  && pb.dir == PlayerBrain.Direction.Down)
            {
                movement = new Vector3(0, -1, 0);
            }
        }
        else
        {
            if (ParsecInput.GetKey(pb.PlayerID, KeyCode.A)) 
            {
                pb.dir = PlayerBrain.Direction.Left;
                CreateDust();
                movement = new Vector3(-1, 0, 0);
            }
            else if (ParsecInput.GetKey(pb.PlayerID, KeyCode.D))
            {
                
                pb.dir = PlayerBrain.Direction.Right;
                CreateDust();
                movement = new Vector3(1, 0, 0);
            }
            else if (ParsecInput.GetKey(pb.PlayerID, KeyCode.W))
            {
                
                pb.dir = PlayerBrain.Direction.Up;
                CreateDust();  
                movement = new Vector3(0, 1, 0);
            }
            else if (ParsecInput.GetKey(pb.PlayerID, KeyCode.S))
            {
                
                pb.dir = PlayerBrain.Direction.Down;
                CreateDust();
                movement = new Vector3(0, -1, 0);
            }
        }

        if (movement != Vector3.zero)
        {
            moving = true;
        }
        //animator.SetBool("Moving", moving);

        pb.SetMoving(moving);

        //Debug.Log("" + i_movement + movement);
        if (moving)
        {
            movement *= speed * Time.deltaTime;
            //transform.Translate(movement);
            rb2d.MovePosition(transform.position + movement);
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
        float horizontal_input = ParsecInput.GetAxisRaw(pb.PlayerID, "Horizontal");
        float vertical_input = ParsecInput.GetAxisRaw(pb.PlayerID, "Vertical");

        if (Mathf.Abs(horizontal_input) > 0.0f)
        {
            vertical_input = 0.0f;
        }
        i_movement = new Vector2(horizontal_input, vertical_input);
    }

    public void CreateDust()
    {
        dust.Play();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RobotMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    private GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        // gm = FindObjectOfType<GameMaster>();
        transform.position = gm.last_checkpoint;
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
        rb.MovePosition(transform.position + new Vector3(moveHorizontal, 0.0f, moveVertical) * speed * Time.deltaTime);
        if (transform.position.y < -10.0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicsII : MonoBehaviour
{
    public GameObject platform1;
    public GameObject platform2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log(other.gameObject.tag);
        // Debug.Log(platform2.transform.position.x);
        // Debug.Log(transform.position.x);
        if (other.gameObject.tag == "Platform" && platform2.GetComponent<MovingPlatform>().released == true && platform2.transform.position.x >= 1.9f && GetComponent<MovingPlatform>().is_equipped == true)
        {
            GetComponent<MovingPlatform>().movable = false;
            platform1.GetComponent<MovingPlatform>().movable = false;
            platform2.GetComponent<MovingPlatform>().movable = false;
            // if (GetComponent<MovingPlatform>().current_movement != null)
            // {
            //     StopCoroutine(GetComponent<MovingPlatform>().current_movement);
            // }
            // if (platform1.GetComponent<MovingPlatform>().current_movement != null)
            // {
            //     StopCoroutine(platform1.GetComponent<MovingPlatform>().current_movement);
            // }
            // if (platform2.GetComponent<MovingPlatform>().current_movement != null)
            // {
            //     StopCoroutine(platform2.GetComponent<MovingPlatform>().current_movement);
            // }
            GetComponent<MovingPlatform>().StopCurrent();
            platform1.GetComponent<MovingPlatform>().StopCurrent();
            platform2.GetComponent<MovingPlatform>().StopCurrent();
            transform.position = new Vector3(0.0f, 0.0f, 22.5f);
            platform1.transform.position = new Vector3(-2.0f, 0.0f, 19.5f);
            platform2.transform.position = new Vector3(2.0f, 0.0f, 25.5f);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            platform1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            platform2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        if (other.gameObject.tag == "Platform" && platform2.GetComponent<MovingPlatform>().triggered == true && platform2.transform.position.x <= -1.0f && GetComponent<MovingPlatform>().is_equipped == true)
        {
            GetComponent<MovingPlatform>().StopCurrent();
            platform1.GetComponent<MovingPlatform>().StopCurrent();
            platform2.GetComponent<MovingPlatform>().StopCurrent();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicsIII : MonoBehaviour
{
    public GameObject platform1;
    public GameObject platform2;
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active == true && platform1.GetComponent<MovingPlatform>().movable == false && platform2.GetComponent<MovingPlatform>().movable == false && GetComponent<MovingPlatform>().is_equipped == true)
        {
            active = false;
            GetComponent<MovingPlatform>().movable = false;
            platform1.GetComponent<TimingTrigger>().StopTiming();
            platform2.GetComponent<TimingTrigger>().StopTiming();
            platform1.GetComponent<TimingTrigger>().active = false;
            platform2.GetComponent<TimingTrigger>().active = false;
            platform1.GetComponent<MovingPlatform>().movable = false;
            platform2.GetComponent<MovingPlatform>().movable = false;
            platform1.GetComponent<TimingTrigger>().StopTiming();
            platform2.GetComponent<TimingTrigger>().StopTiming();
            platform1.transform.position = platform1.GetComponent<MovingPlatform>().target_position;
            platform2.transform.position = platform2.GetComponent<MovingPlatform>().target_position;
            // transform.RotateAround(new Vector3(11.0f, -0.375f, 30.0f), Vector3.up, 30 * Time.deltaTime);
            // platform1.transform.RotateAround(new Vector3(11.0f, -0.375f, 30.0f), Vector3.up, 30 * Time.deltaTime);
            // platform2.transform.RotateAround(new Vector3(11.0f, -0.375f, 30.0f), Vector3.up, 30 * Time.deltaTime);
            // Destroy(platform1);
            // Destroy(platform2);
            StartCoroutine(BridgeRotating());
        }
    }

    IEnumerator BridgeRotating()
    {
        float initial_time = Time.time;
        float progress = Time.time - initial_time;
        while (progress < 1.0f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        Destroy(platform1);
        Destroy(platform2);
        transform.position = new Vector3(11.0f, -0.5f, 30.0f);
        transform.localScale += new Vector3(1.0f, 0.25f, 12.0f);
        while (transform.eulerAngles.y <= 90)
        {
            transform.Rotate(0, 15 * Time.deltaTime, 0);
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 90, 0);
    }
}

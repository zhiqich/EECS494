using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicsIV : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject b1;
    public GameObject b2;
    public GameObject b3;
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            p2.GetComponent<MovingPlatform>().stoppable = !p1.GetComponent<MovingPlatform>().movable;
            p3.GetComponent<MovingPlatform>().stoppable = !p2.GetComponent<MovingPlatform>().movable;
            p4.GetComponent<MovingPlatform>().stoppable = !p3.GetComponent<MovingPlatform>().movable;
            if (p1.GetComponent<MovingPlatform>().movable == false)
            {
                p1.transform.position = p1.GetComponent<MovingPlatform>().target_position;
            }
            if (p2.GetComponent<MovingPlatform>().movable == false)
            {
                p2.transform.position = p2.GetComponent<MovingPlatform>().target_position;
            }
            if (p3.GetComponent<MovingPlatform>().movable == false)
            {
                p3.transform.position = p3.GetComponent<MovingPlatform>().target_position;
            }
            if (p4.GetComponent<MovingPlatform>().movable == false)
            {
                p4.transform.position = p4.GetComponent<MovingPlatform>().target_position;
            }
            if (p3.GetComponent<MovingPlatform>().movable == false && p2.GetComponent<TimingTrigger>().active == true)
            {
                p2.GetComponent<TimingTrigger>().active = false;
                p2.GetComponent<TimingTrigger>().StopTiming();
                p2.GetComponent<MovingPlatform>().movable = false;
                p2.GetComponent<TimingTrigger>().StopTiming();
                p2.transform.position = p2.GetComponent<MovingPlatform>().target_position;
            }
            if (p1.GetComponent<MovingPlatform>().movable == false && p2.GetComponent<MovingPlatform>().movable == false && p3.GetComponent<MovingPlatform>().movable == false && p4.GetComponent<MovingPlatform>().movable == false)
            {
                active = false;
                StartCoroutine(LastPiece());
            }
        }
    }

    IEnumerator LastPiece()
    {
        while (p5.transform.position != new Vector3(29.0f, -0.5f, 30.0f))
        {
            p5.transform.position = Vector3.MoveTowards(p5.transform.position, new Vector3(29.0f, -0.5f, 30.0f), 1.0f * Time.deltaTime);
            yield return null;
        }
        p5.transform.position = new Vector3(29.0f, -0.5f, 30.0f);
        float initial_time = Time.time;
        float progress = Time.time - initial_time;
        while (progress < 1.0f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        b1.SetActive(true);
        b2.SetActive(true);
        b3.SetActive(true);
        Destroy(p1);
        Destroy(p2);
        Destroy(p3);
        Destroy(p5);
        while (b1.transform.eulerAngles.z <= 90)
        {
            b1.transform.Rotate(0, 0, 15 * Time.deltaTime);
            yield return null;
        }
        for (int i = 0; i < 8; i++)
        {
            initial_time = Time.time;
            progress = Time.time - initial_time;
            while (progress < 0.5f)
            {
                progress = Time.time - initial_time;
                yield return null;
            }
            b2.transform.localScale += new Vector3(0.0f, 1.0f, 0.0f);
        }
    }
}

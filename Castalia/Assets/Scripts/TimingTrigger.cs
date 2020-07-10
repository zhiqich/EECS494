using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingTrigger : MonoBehaviour
{
    public bool active = true;
    public bool is_timing = false;
    public float counting = 5.0f;
    public Coroutine timing;
    public GameObject time_light;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active == true && is_timing == false && GetComponent<MovingPlatform>().movable == false)
        {
            is_timing = true;
            timing = StartCoroutine(Timing());
        }
    }

    IEnumerator Timing()
    {
        int flash = 0;
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / counting;
        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / counting;
            if (progress * 10 >= flash)
            {
                flash += 1;
                if (flash % 2 == 0)
                {
                    time_light.GetComponent<Light>().enabled = true;
                }
                else
                {
                    time_light.GetComponent<Light>().enabled = false;
                }
            }
            yield return null;
        }
        GetComponent<MovingPlatform>().movable = true;
        time_light.GetComponent<Light>().enabled = false;
        is_timing = false;
    }

    public void StopTiming()
    {
        if (timing != null)
        {
            time_light.GetComponent<Light>().enabled = false;
            StopCoroutine(timing);
        }
    }
}

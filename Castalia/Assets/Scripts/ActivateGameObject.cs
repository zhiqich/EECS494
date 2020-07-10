using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObject : MonoBehaviour
{
    public GameObject[] deactivates;
    public GameObject[] activates;
    public bool active = true;

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
        if (active == true)
        {
            active = false;
            StartCoroutine(LevelStart());
        }
    }

    IEnumerator LevelStart()
    {
        float initial_time = Time.time;
        float progress = Time.time - initial_time;
        while (progress < 1.0f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            deactivates[i].SetActive(false);
        }
        initial_time = Time.time;
        progress = Time.time - initial_time;
        while (progress < 0.5f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            deactivates[i].SetActive(true);
        }
        initial_time = Time.time;
        progress = Time.time - initial_time;
        while (progress < 0.5f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            deactivates[i].SetActive(false);
        }
        initial_time = Time.time;
        progress = Time.time - initial_time;
        while (progress < 0.5f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            deactivates[i].SetActive(true);
        }
        initial_time = Time.time;
        progress = Time.time - initial_time;
        while (progress < 0.5f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            deactivates[i].SetActive(false);
        }
        initial_time = Time.time;
        progress = Time.time - initial_time;
        while (progress < 0.5f)
        {
            progress = Time.time - initial_time;
            yield return null;
        }
        for (int i = 0; i < 10; i++)
        {
            activates[i].SetActive(true);
        }
    }
}

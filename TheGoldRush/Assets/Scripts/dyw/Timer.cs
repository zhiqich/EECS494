using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public static Timer instance;
    public GameObject text;
    public int TotalTime = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void StartGame()
    {
        instance.StartCoroutine(instance.CountDown());
    }

    IEnumerator CountDown()
    {
        while (TotalTime >=0)
        {
            text.GetComponent<Text>().text = "Remain Time " + TotalTime.ToString() + " s";
            yield return new WaitForSeconds(1);
            TotalTime--;
        }
        EventBus.Publish<EndEvent>(new EndEvent());
    }
}

public class EndEvent
{
    public EndEvent() {}
}

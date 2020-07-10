using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGear : MonoBehaviour
{
    public GameObject gear1;
    public GameObject gear2;
    public GameObject gear3;
    public GameObject gear4;
    public GameObject gear5;
    public GameObject trigger1;
    public GameObject trigger2;
    public GameObject trigger3;
    public GameObject trigger4;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trigger1.GetComponent<TriggerPlatform>().is_triggered == true || trigger2.GetComponent<TriggerPlatform>().is_triggered == true)
        {
            GetComponent<GFMachine>().StartMachine();
            GetComponent<GFMachine>().Reverse = false;
        }
        else if (trigger3.GetComponent<TriggerPlatform>().is_triggered == true || trigger4.GetComponent<TriggerPlatform>().is_triggered == true)
        {
            GetComponent<GFMachine>().StartMachine();
            GetComponent<GFMachine>().Reverse = true;
        }
        else
        {
            GetComponent<GFMachine>().StopMachine();
        }
    }
}

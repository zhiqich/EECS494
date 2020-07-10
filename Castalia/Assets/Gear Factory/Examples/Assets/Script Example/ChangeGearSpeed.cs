using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGearSpeed : MonoBehaviour {
    [Multiline]
    public string whatDoesThisScriptDo = "In play mode:\r\n" +
                                         "Click a gear to set speed to match Gear Speed property.\r\n";
    public float gearSpeed = 10f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GFGear gear = hit.transform.GetComponent<GFGear>();
                if (gear != null)
                {
                    gear.SetSpeed(gearSpeed);
                }
            }
        }
    }
}
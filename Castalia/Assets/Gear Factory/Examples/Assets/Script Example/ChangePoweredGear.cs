using UnityEngine;
using System.Collections;

public class ChangePoweredGear : MonoBehaviour {
    [Multiline]
    public string whatDoesThisScriptDo = "In play mode:\r\n" +
                                         "Press 1-key to power gear in gear1 property.\r\n" +
                                         "Press 2-key to power gear in gear2 property.";

    public GFMachine machine;
    public GFGear gear1;
    public GFGear gear2;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
        {
            machine.PoweredGear = gear1;
            machine.RecalculateGears();
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2))
        {
            machine.PoweredGear = gear2;
            machine.RecalculateGears();
        }

    }
}

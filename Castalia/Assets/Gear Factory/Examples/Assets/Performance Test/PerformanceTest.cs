using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerformanceTest : MonoBehaviour {
	public GFMachine machine;
	public GFGear gearToTest;
	public int numberOfInstances = 1;	
	
	private List<GFGear> instances = new List<GFGear>();
	
	// Use this for initialization
	void Start() {
		AddGears();		
	}
	
	void AddGears()
	{
		if (numberOfInstances < 1)
			numberOfInstances = 1;

		if (numberOfInstances > 1)
		{
			GFGear gear = null;
			
			int seqLen = 0;
			int repeat = 0; 
			int n = 0;
			Vector3 dir;
			for (int i = 0; i < numberOfInstances-1; i+=seqLen)
			{
				if (repeat == 1)
				{
					repeat = 0;
					seqLen++;
				}
				else 
					repeat++;
				
				for (int g=0; g<seqLen; g++)
				{
					if (instances.Count == numberOfInstances-1)
					{
						i = 0;
						break;
					}	
					gear = instances.Count > 0 ? instances[instances.Count-1] : gearToTest;
	
					dir = Quaternion.AngleAxis(90f * n, Vector3.back) * Vector3.right;
					GFGear newGear = gear.Clone(dir).GetComponent<GFGear>();
					instances.Add(newGear);
				}
				n++;
				if (n >= 4) n = 0;
			}

			machine.RecalculateGears();
		}
	}
	
	// Update is called once per frame
	void Update() {
	
	}
/*	
	void OnGUI() {
		// Make a background box
		GUI.Box(new Rect(10,10,100,90), "Instances");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,80,20), "+")) {
			numberOfInstances++;
			AddGear();
		}

		// Make the second button.
/*		
		if(GUI.Button(new Rect(20,70,80,20), "-")) {
			numberOfInstances--;
			if (numberOfInstances <= 0)
				numberOfInstances = 1;
			Reset();
		}* /
	}
*/	
}

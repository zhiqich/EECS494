#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(GFMachine))]
public class GFMachineEditor : Editor {
 	
	private SerializedObject machine;
	private SerializedProperty machineSpeed;
	private GFMachine machineObject;
	
	[MenuItem ("GameObject/Create Other/Gear Factory/Machine")]
	static void Create(){
		GameObject gameObject = new GameObject("Machine");
		gameObject.AddComponent(typeof(GFMachine));
	}
	
	void OnEnable()
	{
		machine = new SerializedObject(this.target);		
		machineSpeed = machine.FindProperty("speed");
		machineObject = (GFMachine)this.target;
	}
	
	void OnSceneGUI()
	{		
		if (!Application.isPlaying)
		{
			if (machineObject != null && machineObject.PoweredGear != null)
			{
				Handles.color = new Color(1.0f, 0.7f, 0.0f, 0.25f);
				Handles.DrawSolidDisc(machineObject.PoweredGear.transform.position, machineObject.PoweredGear.gameObject.CalcYAlignedCenterPlane().normal, machineObject.PoweredGear.radius / 2.0f);
				Handles.Label(machineObject.PoweredGear.transform.position + ((machineObject.PoweredGear.radius*1.30f) * Vector3.up) + (machineObject.PoweredGear.radius * Vector3.left) , "Machine powered (Speed:"+machineObject.speed.ToString()+")" );
			}
		
			foreach (GFGear g in machineObject.GetGears())
			{
				g.DrawPoweredBy();
			}
		}
	}
 
	override public void OnInspectorGUI()
	{
		base.OnInspectorGUI(); 
		
		machine.Update();
		EditorGUILayout.Slider(machineSpeed, 0.0f, machineObject.maxSpeed);
		machine.ApplyModifiedProperties();
		
		int gearcount = machineObject.GetGears().Length;
		EditorGUILayout.HelpBox("Total number of gears in this machine: "+gearcount.ToString()+" gear"+(gearcount == 1 ? "":"s"), MessageType.Info, true);
	}
 	
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// [GameObject editor] Editor for manipulating both the GFGear and GFGearGen.
/// </summary>
[CustomEditor(typeof(GFGear)), CanEditMultipleObjects]
public class GFGearEditor : Editor {
    // Static foldouts to prevent the user from unfolding after every selection
    static public bool alignParametersFoldout;

    /*
	private static bool _isInAddGearMode = false;
	public static bool isInAddGearMode {
		get {return _isInAddGearMode; } 
		set {
			_isInAddGearMode = value;
			
			if (_isInAddGearMode)
			{
				// Lock editor for drawing our stuff
				HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			}
			else
			{
				// Give editor control back to user
				HandleUtility.Repaint();
			}
		}
	}
	*/
    static private bool isInAddGearMode = false;
    static private bool cancelAddGearMode = false;
    

    private void CloneGear(GFGear gear, Vector3 direction)
	{
		GameObject newGearGO = gear.Clone(direction);
						
		// Mark new gear as dirty
		UnityEditor.EditorUtility.SetDirty(newGearGO);
		// Set focus on new gear	
		Selection.activeGameObject = newGearGO;
	}
	
	private Vector3 GetMouseInWorldCoords(Plane plane, Vector2 mousePos)
	{
		if (Camera.current != null)
		{
            Ray r = HandleUtility.GUIPointToWorldRay(mousePos);
			float distance;
			if (plane.Raycast(r, out distance))
			{
				return r.GetPoint(distance);
			}
		}
		return Vector3.zero;
	}
	
	
	private void CloneTowardsMousePos(GFGear gear, Vector2 mousePos)
	{
		Vector3 mousewpos = GetMouseInWorldCoords(MeshUtils.CalcYAlignedCenterPlane(gear.gameObject), mousePos);
		Vector3 direction = mousewpos - gear.gameObject.transform.position;
		direction.Normalize();
        
		CloneGear(gear, direction); 
	}
	
	private void CancelAddGearMode()
	{
		isInAddGearMode = false;
		HandleUtility.Repaint();
         
    }
	
	void OnSceneGUI()
	{
        Rect guiArea = new Rect( 10, 10, 110, 150);

        GFGear gear = (GFGear)this.target;

		if (!Application.isPlaying)
		{ 
            if (UnityEditor.Selection.Contains(gear.gameObject))
			{
                RecalculateGears(gear);

                GFGearGen gearGen = gear.gameObject.GetComponent<GFGearGen>();

                // Show powered by arrow
                if (gear.machine == null || (gear.machine != null && gear.machine.ShowPoweredByIndicator))
					gear.DrawPoweredBy();

                if (gearGen == null && gear.AutoAlign)
                    gear.DrawAlignmentHelpers();

                //GUI.enabled = (gear.DrivenBy != null && (gear.machine == null || (gear.machine != null && gear.machine.ShowPoweredByIndicator)));

                //Handles.color = Color.white;
                //Handles.DrawLine(gear.transform.position, GetMouseInWorldCoords(MeshUtils.CalcYAlignedCenterPlane(gear.gameObject), Event.current.mousePosition));

                #region GUI.enabled then this is visible. 
                // Note: Not drawing this part is not an option as it will destroy your focus on selected element even if it's visible as being focused.
                //if (GUI.enabled && gear.machine != null && gear.machine.ShowBox)
                //	gear.DrivenBy.gameObject.DrawBoundingBox(Color.yellow);

                // Draw unlink button
                Handles.BeginGUI();

                GUILayout.BeginArea(guiArea);

                if (!isInAddGearMode)
                {
                    if (gear.DrivenBy != null)
                    {
                        GUIContent gcontent = new GUIContent("Unlink", "Manually override linkage.\r\n- Sets \"Driven By\" to: null\n- Sets \"Auto Set Driven By\" to: false");

                        if (GUILayout.Button(gcontent))
                        {
                            gear.DrivenBy = null;
                            gear.AutoSetDrivenBy = false;
                            UnityEditor.EditorUtility.SetDirty(gear);
                        }


                        GUI.enabled = true;

                        if (gearGen != null)
                        {
                            #region auto alignment
                            string label = "";
                            string hint = "";
                            bool autoAlign = false;
                            if (gearGen.alignTeethWithParent || gearGen.alignRadiusWithParent)
                            {
                                autoAlign = false;
                                label = "Move freely";
                                hint = "Switch off auto-alignment.\r\n- Sets \"Align Teeth With Parent\" to: false\r\n- Sets \"Align Radius With Parent\" to: false\r\n";
                            }
                            else
                            {
                                autoAlign = true;
                                label = "Snap to parent";
                                hint = "Switches on auto-alignment.\r\n- Sets \"Align Teeth With Parent\" to: true\r\n- Sets \"Align Radius With Parent\" to: true\r\n";
                            }

                            GUIContent gcontent2 = new GUIContent(label, hint);

                            if (GUILayout.Button(gcontent2))
                            {
                                gearGen.alignTeethWithParent = autoAlign;
                                gearGen.alignRadiusWithParent = autoAlign;
                                UnityEditor.EditorUtility.SetDirty(gearGen);
                            }
                            #endregion
                        }
                        else
                        {
                            #region auto alignment
                            if (gear.DrivenBy != null)
                            {
                                string label = "";
                                string hint = "";
                                if (gear.AutoAlign)
                                {
                                    label = "Move freely";
                                    hint = "Switch off auto-alignment.\r\n";
                                }
                                else
                                {
                                    label = "Snap to parent";
                                    hint = "Switches on auto-alignment.\r\n";
                                }

                                GUIContent gcontent3 = new GUIContent(label, hint);

                                if (GUILayout.Button(gcontent3))
                                {
                                    gear.AutoAlign = !gear.AutoAlign;
                                    UnityEditor.EditorUtility.SetDirty(gear);
                                }
                            }
                            #endregion

                        }
                    }

                    if (gearGen != null)
                    {
                        GUIContent gnewgear = new GUIContent("Add single", "Adds a new gear that's linked\r\nto last selected gear.");//GFGearEditor.isInAddGearMode ? "Done" : "Link new gear", "Adds a new gear that's linked to selected gear.");

                        if (GUILayout.Button(gnewgear))
                        {
                            CloneGear(gear, Vector3.right);
                        }

                        GUIContent gnewgearMulti = new GUIContent("Add multiple", "Add multiple gears.\r\nClick in your scene to specify direction.\r\nRight click when finished.");

                        if (GUILayout.Button(gnewgearMulti))
                        {
                          isInAddGearMode = true;
                        }

                        GUIContent grandomgear = new GUIContent("Randomize", "Rebuilds current gear with\r\ndifferent randomized settings.");//GFGearEditor.isInAddGearMode ? "Done" : "Link new gear", "Adds a new gear that's linked to selected gear.");

                        if (GUILayout.Button(grandomgear))
                        {
                            gearGen.Randomize();
                        }
                    }
                }
                else {
                    GUIContent gnewgearMulti = new GUIContent("Done", "Finish adding multiple gears.\r\n");

                    if (GUILayout.Button(gnewgearMulti))
                    {
                        cancelAddGearMode = true;
                    }

                }

                GUILayout.EndArea();

                //GUI.Label(new Rect(Screen.width - 250, Screen.height - 70 - 150, 250, 100), GUI.tooltip);
                Handles.EndGUI();
                #endregion


                if (gear.machine != null && gear.machine.PoweredGear == gear)
				{
					Handles.Label(gear.transform.position + ((gear.radius*1.30f) * Vector3.up) + (gear.radius * Vector3.left) , "Machine powered (Speed:"+gear.machine.speed.ToString()+")" );
				}
			}

            if (isInAddGearMode || cancelAddGearMode)
            {
                if (cancelAddGearMode || Event.current.type != EventType.MouseUp || (Event.current.type == EventType.MouseUp && !guiArea.Contains(Event.current.mousePosition)))
                {
                    if (Event.current.type == EventType.MouseUp || cancelAddGearMode)
                    {
                        if (Event.current.button == 0 && !cancelAddGearMode)
                        {
                            CloneTowardsMousePos(gear, Event.current.mousePosition);
                        }
                        else
                        {
                            cancelAddGearMode = false;

                            CancelAddGearMode();
                        }
                    }

                    int controlID = GUIUtility.GetControlID(FocusType.Passive);
                    if (Event.current.type == EventType.Layout)
                        HandleUtility.AddDefaultControl(controlID);
                }
            }
        }

	}
	
	public void RecalculateGears(GFGear g1)
	{
		GFGear prevDrivenBy = g1.DrivenBy;
		if (g1.AutoSetDrivenBy)
			g1.DrivenBy = null;
		foreach (GFGear otherGear in g1.otherGears)
		{				
			if (!otherGear.gameObject.Equals(g1.gameObject) && ((g1.AutoSetDrivenBy && g1.Intersects(otherGear)) || !g1.AutoSetDrivenBy))
			{
				// Set initial rotation 	
				if (g1.machine == null || (g1.machine != null && g1.machine.PoweredGear != g1))
				{
					if (g1.AutoSetDrivenBy && g1.DrivenBy != otherGear)
					{
						// If redundant / circular reference is occuring: restore previous link and abort
						if (otherGear.DrivenBy == g1)
							g1.DrivenBy = prevDrivenBy;
						else
						{
							g1.DrivenBy = otherGear;
						}
					}
										
					if (g1.DrivenBy != null)
					{
						// If gear is auto generated, we can align the teeth
						GFGearGen gg1 = g1.GetComponent(typeof(GFGearGen)) as GFGearGen;
						GFGearGen gg2 = g1.DrivenBy.GetComponent(typeof(GFGearGen)) as GFGearGen;
                        if (gg1 != null && gg2 != null)
                        {
                            if (gg1.alignTeethWithParent)
                            {
                                gg1.Align(gg2);
                            }
                            else
                            {
                                if (gg1.alignRadiusWithParent)
                                {
                                    gg1.AlignRadius(gg2);
                                }
                            }
                        }
                        else {
                            // If gear is not auto generated, we can align the positions if autoAlign is true.
                            if (gearAutoAlign.boolValue)
                            {
                                g1.Align(g1.DrivenBy);
                            }
                        }
					}
				}
				
				// Tell the unity editor we changed something by code, so it gets saved.
				UnityEditor.EditorUtility.SetDirty(g1);
				// All aligned and set: get the hell out of this loop!
				break;
			}
		}
		
	}
	
	
	private bool hasGFGearGen = false;
	private GFGear gearObject;
	private SerializedObject gear;
	private SerializedProperty gearNumberOfTeeth;
	private SerializedProperty gearRotateX;
	private SerializedProperty gearRotateY;
	private SerializedProperty gearRotateZ;
    private SerializedProperty gearAutoAlign;
    private SerializedProperty gearRadius;
    private SerializedProperty gearTipLength;
    private SerializedProperty gearInnerTeeth;
    private SerializedProperty gearDrivenBy;
    private SerializedProperty gearReverseRotation;
    private SerializedProperty gearReverseRotationPlusSubtree;
    private SerializedProperty gearSyncSpeed;
    private SerializedProperty gearAutoSetDrivenBy;

    void OnEnable()
	{
        
		gearObject = (GFGear)this.target;
		
		gear = new SerializedObject(this.target);		
		gearNumberOfTeeth = gear.FindProperty("numberOfTeeth");
		gearRotateX = gear.FindProperty("rotateX");
		gearRotateY = gear.FindProperty("rotateY");
		gearRotateZ = gear.FindProperty("rotateZ");
        gearAutoAlign = gear.FindProperty("AutoAlign");
        gearRadius = gear.FindProperty("radius");
        gearTipLength = gear.FindProperty("tipLength");
        gearInnerTeeth = gear.FindProperty("innerTeeth");
        
        gearDrivenBy = gear.FindProperty("DrivenBy");
        gearAutoSetDrivenBy = gear.FindProperty("AutoSetDrivenBy");
        gearReverseRotation = gear.FindProperty("ReverseRotation");
        gearReverseRotationPlusSubtree = gear.FindProperty("ReverseRotationPlusSubtree");
        gearSyncSpeed = gear.FindProperty("SyncSpeed");

        GFGearGen c = (GFGearGen)gearObject.gameObject.GetComponent(typeof(GFGearGen));
		hasGFGearGen = c != null;
    
	}
	
	override public void OnInspectorGUI()
	{ 
        gear.Update();

        //base.OnInspectorGUI();
        EditorGUILayout.PropertyField(gearAutoSetDrivenBy);
        EditorGUILayout.PropertyField(gearDrivenBy);

        if (!hasGFGearGen)
		{
            EditorGUILayout.PropertyField(gearNumberOfTeeth);
            EditorGUILayout.PropertyField(gearAutoAlign);

            //if (gearAutoAlign.boolValue)
            //{
            alignParametersFoldout = EditorGUILayout.Foldout(alignParametersFoldout, new GUIContent("Alignment parameters", ""));
            if (alignParametersFoldout)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(gearRadius);
                EditorGUILayout.PropertyField(gearTipLength);
                EditorGUILayout.PropertyField(gearInnerTeeth);

                EditorGUI.indentLevel--;
            }
            //}
        }
               

        EditorGUILayout.BeginHorizontal();//GUILayout.ExpandWidth(true));
		EditorGUILayout.PrefixLabel("Rotate");
		EditorGUILayout.LabelField(new GUIContent("X",""), GUILayout.Width(20));
		EditorGUILayout.PropertyField(gearRotateX, new GUIContent(""), GUILayout.Width(20));
		EditorGUILayout.LabelField(new GUIContent("Y",""), GUILayout.Width(20));
		EditorGUILayout.PropertyField(gearRotateY, new GUIContent(""), GUILayout.Width(20));
		EditorGUILayout.LabelField(new GUIContent("Z",""), GUILayout.Width(20));
		EditorGUILayout.PropertyField(gearRotateZ, new GUIContent(""), GUILayout.Width(20));
		EditorGUILayout.EndHorizontal();


        EditorGUILayout.PropertyField(gearReverseRotation);
        EditorGUILayout.PropertyField(gearReverseRotationPlusSubtree);
        EditorGUILayout.PropertyField(gearSyncSpeed);

        string info;
        info = "Current speed: " + gearObject.currentSpeed + " degrees per second.\r\n";
        info += "Ratio: "+gearObject.speedMultiplier+"\r\n";
        info += "Gear to machine ratio: "+gearObject.machineRatio+"\r\n";
        info += "\r\n";
        EditorGUILayout.HelpBox(info, MessageType.Info, true);


        //base.OnInspectorGUI();
        //return;
        gear.ApplyModifiedProperties();
        
	}

}
#endif 
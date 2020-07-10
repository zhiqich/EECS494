using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// [GameObject editor] In the menu GameObject>Create Other>Gear Factory>Gear. 
/// Editor for the procedural gameobject containing at least the GFGear and GFGearGen components
/// </summary>
[CustomEditor(typeof(GFGearGen)), CanEditMultipleObjects]
public class GFGearGenEditor : Editor
{
    // Static foldouts to prevent the user from unfolding after every selection
    static public bool toothParametersFoldout;
    static public bool uvFoldout;
    static public bool normalsFoldout;

    // Using serialized objects and properties to enable in-editor undo and stuff
    private SerializedObject gearGen;
    private Dictionary<string, SerializedProperty> gearGenProps;
    private SerializedObject gear;
    private Dictionary<string, SerializedProperty> gearProps;
    //	private bool innerTeethInitialState;

    private GFGearGen gearGenObject;
    private static Mesh lastGearGenObjectMesh;

    [MenuItem("GameObject/Create Other/Gear Factory/Gear")]
    static void Create()
    {
        GameObject gameObject = new GameObject("Gear");
        GFGear g = (GFGear)gameObject.AddComponent(typeof(GFGear));
        GFGearGen gg = (GFGearGen)gameObject.AddComponent(typeof(GFGearGen));
        MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
        meshFilter.mesh = new Mesh();

        gg.Rebuild();

        gameObject.GetComponent<Renderer>().material = new Material(Shader.Find("Diffuse"));

        if (g.DrivenBy == null)
        {
            gg.alignTeethWithParent = false;
            gg.alignRadiusWithParent = false;
        }
        
        GFGearGenEditor.Persist(gg);
    }

    private void AddGearGenProperty(string name)
    {
        gearGenProps.Add(name.ToLower(), gearGen.FindProperty(name));
    }
    private void AddGearProperty(string name)
    {
        gearProps.Add(name.ToLower(), gear.FindProperty(name));
    }

    /// <summary>
    /// Get serialized property of GearGen
    /// </summary>
    /// <param name='name'>
    /// Name.
    /// </param>
    private SerializedProperty GearGenP(string name)
    {
        if (gearGenProps.ContainsKey(name.ToLower()))
            return gearGenProps[name.ToLower()];
        else
        {
            Debug.LogError("GearGen property '" + name + "' not in dictionary.");
            return null;
        }
    }
    private SerializedProperty GearP(string name)
    {
        if (gearProps.ContainsKey(name.ToLower()))
            return gearProps[name.ToLower()];
        else
        {
            Debug.LogError("Gear property '" + name + "' not in dictionary.");
            return null;
        }
    }

    void OnEnable()
    {
        gearGenObject = (GFGearGen)this.target;
        lastGearGenObjectMesh = gearGenObject.gameObject.GetComponent<MeshFilter>().sharedMesh;

        gearGen = new SerializedObject(this.target);
        gearGenProps = new Dictionary<string, SerializedProperty>();
        AddGearGenProperty("numberOfTeeth");
        AddGearGenProperty("radius");
        AddGearGenProperty("innerRadius");
        AddGearGenProperty("innerMinVertexDistance");
        AddGearGenProperty("thickness");
        AddGearGenProperty("fillCenter");
        AddGearGenProperty("fillOutside");
        AddGearGenProperty("is3d");
        AddGearGenProperty("alignTeethWithParent");
        AddGearGenProperty("alignRadiusWithParent");
        AddGearGenProperty("twistAngle");
        AddGearGenProperty("twistOutside");
        AddGearGenProperty("innerTeeth");
        AddGearGenProperty("tipLength");
        AddGearGenProperty("tipSize");
        AddGearGenProperty("valleySize");
        AddGearGenProperty("valleyAngleOffset");
        AddGearGenProperty("tipAngleOffset");
        AddGearGenProperty("skew");
        AddGearGenProperty("showNormals");
        AddGearGenProperty("splitVerticesAngle");

        AddGearGenProperty("generateTextureCoordinates");
        AddGearGenProperty("uvTiling");
        AddGearGenProperty("uvOffset");

        GFGear c = (GFGear)gearGenObject.gameObject.GetComponent(typeof(GFGear));
        if (c != null)
        {
            gear = new SerializedObject(c);
            gearProps = new Dictionary<string, SerializedProperty>();
            AddGearProperty("numberOfTeeth");
        }
        else
        {
            gear = null;
        }

        //		innerTeethInitialState = gearGenObject.innerTeeth;
        //Persist();

        Undo.undoRedoPerformed = UndoCallback;
    }

    void UndoCallback()
    {
        gearGenObject.Rebuild();
    }

    private void RenderGUITipValleyProperties()
    {
        if (gearGenObject.innerTeeth)
        {
            EditorGUILayout.PropertyField(GearGenP("valleySize"), new GUIContent("Tip Size", ""));
            EditorGUILayout.PropertyField(GearGenP("valleyAngleOffset"), new GUIContent("Tip Angle Offset", ""));
            EditorGUILayout.PropertyField(GearGenP("tipSize"), new GUIContent("Valley Size", ""));
            EditorGUILayout.PropertyField(GearGenP("tipAngleOffset"), new GUIContent("Valley Angle Offset", ""));
        }
        else
        {
            EditorGUILayout.PropertyField(GearGenP("tipSize"), new GUIContent("Tip Size", ""));
            EditorGUILayout.PropertyField(GearGenP("tipAngleOffset"), new GUIContent("Tip Angle Offset", ""));
            EditorGUILayout.PropertyField(GearGenP("valleySize"), new GUIContent("Valley Size", ""));
            EditorGUILayout.PropertyField(GearGenP("valleyAngleOffset"), new GUIContent("Valley Angle Offset", ""));
        }
    }

    private void RenderGUIRadiusProperties()
    {
        if (gearGenObject.innerTeeth)
        {
            EditorGUILayout.Slider(GearGenP("innerRadius"), 0.01f, 100f, new GUIContent("iRadius", ""));
            EditorGUILayout.Slider(GearGenP("radius"), 0.01f, GearGenP("innerRadius").floatValue, new GUIContent("rInner Radius", ""));
        }
        else
        {
            EditorGUILayout.Slider(GearGenP("radius"), 0.01f, 100f, new GUIContent("rRadius", ""));
            EditorGUILayout.Slider(GearGenP("innerRadius"), 0.0f, GearGenP("Radius").floatValue, new GUIContent("iInner Radius", ""));
        }
    }

    private void SwapProps(string propName1, string propName2)
    {
        float tmp = GearGenP(propName1).floatValue;
        GearGenP(propName1).floatValue = GearGenP(propName2).floatValue;
        GearGenP(propName2).floatValue = tmp;
    }

    private void SwapForInnerTeeth()
    {
        SwapProps("radius", "innerRadius");
        SwapProps("valleySize", "tipSize");
        SwapProps("valleyAngleOffset", "tipAngleOffset");
    }

    override public void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        gearGen.Update();

        //RenderGUIRadiusProperties();			
        EditorGUILayout.PropertyField(GearGenP("radius"));
        EditorGUILayout.PropertyField(GearGenP("innerRadius"));

        if (gearGenObject.innerRadius > 0f)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(GearGenP("innerMinVertexDistance"));
            EditorGUI.indentLevel--;
        }

        GearGenP("innerTeeth").boolValue = GearGenP("innerRadius").floatValue > GearGenP("radius").floatValue;

        EditorGUILayout.PropertyField(GearGenP("alignTeethWithParent"), new GUIContent("Align Teeth With Parent", ""));
        
        if (!gearGenObject.alignTeethWithParent)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.IntSlider(GearGenP("numberOfTeeth"), 2, 100, new GUIContent("Number Of Teeth", ""));
            EditorGUILayout.PropertyField(GearGenP("alignRadiusWithParent"), new GUIContent("Align Radius With Parent", ""));
            EditorGUI.indentLevel--;
        }
        
        toothParametersFoldout = EditorGUILayout.Foldout(toothParametersFoldout, new GUIContent("Tooth parameters", ""));
        if (toothParametersFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(GearGenP("tipLength"), new GUIContent("Tip Length", ""));
            RenderGUITipValleyProperties();
            EditorGUILayout.PropertyField(GearGenP("skew"), new GUIContent("Skew", ""));
            EditorGUI.indentLevel--;
        }

        
        EditorGUILayout.PropertyField(GearGenP("is3d"), new GUIContent("Is 3d", ""));
        EditorGUI.indentLevel++;
        if (gearGenObject.is3d)
        {
            EditorGUILayout.PropertyField(GearGenP("thickness"), new GUIContent("Thickness", ""));
            EditorGUILayout.PropertyField(GearGenP("fillCenter"), new GUIContent("Fill Center", ""));
            EditorGUILayout.PropertyField(GearGenP("fillOutside"), new GUIContent("Fill Outside", ""));
            EditorGUILayout.PropertyField(GearGenP("twistAngle"), new GUIContent("Twist Angle", ""));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(GearGenP("twistOutside"), new GUIContent("Twist Outside", ""));
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
        
        normalsFoldout = EditorGUILayout.Foldout(normalsFoldout, new GUIContent("Normals", ""));
        if (normalsFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(GearGenP("showNormals"), new GUIContent("Show Normals", ""));
            EditorGUILayout.PropertyField(GearGenP("splitVerticesAngle"), new GUIContent("Split Vertices Angle", ""));

            EditorGUI.indentLevel--;
        }
        
        uvFoldout = EditorGUILayout.Foldout(uvFoldout, new GUIContent("UV Coordinates", ""));
        if (uvFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(GearGenP("generateTextureCoordinates"), new GUIContent("Texture Coordinates", ""));
            if (gearGenObject.generateTextureCoordinates != GFGearGenTextureCoordinates.None)
            {
                EditorGUILayout.PropertyField(GearGenP("uvTiling"), new GUIContent("UV Tiling", ""));
                EditorGUILayout.PropertyField(GearGenP("uvOffset"), new GUIContent("UV Offset", ""));
            }
            EditorGUI.indentLevel--;
        }
        
        gearGen.ApplyModifiedProperties();

        if (gearGenObject.GetComponent<MeshFilter>().sharedMesh == null)
        {
            gearGenObject.Init();
        }
        // Reflect changes by regenerating gear
        if (GUI.changed)
        {
            gearGenObject.Rebuild();
        }
        
        string info = "";

        //		"Batching dynamic objects has certain overhead per vertex, so batching is applied only to meshes containing less than 900 vertex attributes in total.
        //If your shader is using Vertex Position, Normal and single UV, then you can batch up to 300 verts; whereas if your shader is using Vertex Position, Normal, UV0, UV1 and Tangent, then only 180 verts.
        //"900 per attribute / 4 attributes (pos + normal + UV + Tangent)"

        if (gearGenObject.numberOfVertices <= 900 / 4)
            info += "[Dynamic batching (where available)]\r\n";
        else
            if (gearGenObject.numberOfVertices <= 900 / 3)
            info += "[Dynamic batching when not using bumpmap (where available)]\r\n";

        info += "Number of vertices: " + gearGenObject.numberOfVertices + " vertices";
        info += "\r\n";
        info += "Number of faces: " + gearGenObject.numberOfFaces + " faces";
        info += "\r\n";
        info += "Removed by optimizer: " + gearGenObject.ggp.verticesRemoved + " vertices";
        info += "\r\n";
        
        EditorGUILayout.HelpBox(info, MessageType.Info, true);
        
        // GameObject has GFGear component ? Auto set the number of teeth
        if (gear != null)
        {
            gear.Update();
            GearP("numberOfTeeth").intValue = GearGenP("numberOfTeeth").intValue;
            gear.ApplyModifiedProperties();
        }
        GUILayout.Space(40);
        //DrawDefaultInspector();

        GFGearGenEditor.Persist(gearGenObject); 
    }
     

    static private void Persist(GFGearGen gearGenObject)
    {
        Mesh newMesh = gearGenObject.GetComponent<MeshFilter>().sharedMesh;
        if (newMesh == null || !AssetDatabase.Contains(newMesh))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Gear Factory/Meshes"))
                AssetDatabase.CreateFolder("Assets/Gear Factory", "Meshes");

            string assetName = AssetDatabase.GenerateUniqueAssetPath("Assets/Gear Factory/Meshes/" + gearGenObject.gameObject.name + ".asset");
            AssetDatabase.CreateAsset(newMesh, assetName);
            AssetDatabase.SaveAssets(); 
            
        }
    }
    /*
    public void OnSceneGUI()
    {         
        if (Event.current.commandName == "SoftDelete")
        {
            if (target != null)
            {
                Mesh theMesh = (target as GFGearGen).gameObject.GetComponent<MeshFilter>().sharedMesh;
                if (theMesh != null && AssetDatabase.Contains(theMesh))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(theMesh));                     
                }
            }
        } 
    }*/

   

    public void OnDestroy()
    {
        if (Application.isEditor && !Application.isPlaying && target == null && lastGearGenObjectMesh != null) { 
            Mesh theMesh = lastGearGenObjectMesh; 
            if (theMesh != null && AssetDatabase.Contains(theMesh))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(theMesh));
            } 
       }

    }

}
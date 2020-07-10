using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Responsible for procedurally creating a visual interpretation of a gear, using its parameters to determine shape and size.
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GFGearGen : GFBase
{
	public readonly GFGearGenProc ggp = new GFGearGenProc();
	
	public GFGearGenTextureCoordinates generateTextureCoordinates = GFGearGenTextureCoordinates.Box;
	
	public Vector3 uvTiling = Vector3.one;
	public Vector3 uvOffset = Vector3.zero;

	
	/// <summary>
	/// The width of the tooth tip.
	/// </summary>
	public float tipSize = 0.0f;
	/// <summary>
	/// The angleoffset of the tip relative to tipLength.
	/// </summary>
	public float tipAngleOffset = 0f;
	/// <summary>
	/// The size of the valley (space between the base of two teeth).
	/// </summary>
	public float valleySize = 0.0f;
	/// <summary>
	/// The angleoffset of the tip valley relative to radius-tipLength.
	/// </summary>
	public float valleyAngleOffset = 0f;
	/// <summary>
	/// The angle-offset for each tip.
	/// Creates a saw tooth effect.
	/// </summary>
	public float skew = 0.0f;
	/// <summary>
	/// The angle-offset between the front and backface of the gear.
	/// Only visible when is3d is checked.
	/// </summary>
	public float twistAngle = 0f;
	/// <summary>
	/// Twist outer edge too. 
	/// May have some visual drawbacks, but maybe useful if twistAngle isn't twisting it far enough for your needs.
	/// You might want to raise the splitVerticesAngle threshold to smoothen the outside.
	/// </summary>
	public bool twistOutside = false;
	/// <summary>
	/// The thickness of the gear.
	/// Only visible when is3d is checked.
	/// </summary>
	public float thickness = 0.1f;
	
	/// <summary>
	/// Generate normals.
	/// This overrides the internal normal calculation.
	/// This will create flat surfaces, except on the small side.
	/// It's a great leverage between keeping a low vertex count and getting acceptable lighting results.
	/// It will use shared vertices and shared normals but normals are forced to keep side surfaces correctly lit.
	/// </summary>
//	public bool generateNormals = true;
	
	/// <summary>
	/// Show normals in editor for debugging purposes.
	/// </summary>
	public bool showNormals = false;
	
	public float splitVerticesAngle = 30f;
	
	/// <summary>
	/// When enabled a 3d gear mesh will be created.
	/// When disabled it will only create vertices and faces for the front side.
	/// If you are using the gear for 2d purposes, disable this option for improved performance.
	/// </summary>
	public bool is3d = true;
	/// <summary>
	/// When used in 3d and inner radius is larger than 0.0, it will fill the inner side of the gear with faces.
	/// No extra vertices will be generated.
	/// Leave this option turned off when you're planning to fill the inner space of the gear with a custom mesh.
	/// </summary>
	public bool fillCenter = true;
	/// <summary>
	/// Fill the outer ring of the gear
	/// </summary>
	public bool fillOutside = true;
	/// <summary>
	/// Inner side will be hollow when inner radius is larger than 0.0
	/// It will generate extra vertices.
	/// </summary>
	public float innerRadius = 0.0f; // 0.0f - (radius-tipLength)
    /// <summary>
    /// When distance drops below this value, vertices in the center ring will be merged. 
    /// </summary>
    public float innerMinVertexDistance = 0.01f;
	/// <summary>
	/// Align number of teeth, based on radius with parent and set orientation to interlock correctly with parent.
	/// </summary> 
	public bool alignTeethWithParent = true;
	/// <summary>
	/// Align radius of gear with parent
	/// </summary>
	public bool alignRadiusWithParent = true;
	 	
	public int numberOfVertices{get {return gameObject.GetComponent<MeshFilter>().sharedMesh == null ? 0 : gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;}}
	public int numberOfFaces{get {return gameObject.GetComponent<MeshFilter>().sharedMesh == null ? 0 : gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;}}
	
	
	public GFGear gear {
		get {
			return this.gameObject.GetComponent<GFGear>();
		}
	}

    
	public void Rebuild(){
        if (this != null && this.gameObject != null)
            ggp.GenerateGear(this);
	}
	
	void Start(){
		isShifted = false;
	}
	
#if UNITY_EDITOR
	void Update(){
		if (!Application.isPlaying)
		{
			if (showNormals)
			{			
				for(int i=0; i<ggp.vertices.Count; i++)
				{
					Vector3 lok = gameObject.transform.TransformPoint(ggp.vertices[i]);
				 	Debug.DrawLine(lok, gameObject.transform.TransformPoint(ggp.vertices[i]+ggp.normals[i]), Color.white);
				}
			}	


		}
	}
#endif
    void Awake()
    {
        Init();
    }

    public void Init(){
		// In editormode ? Iterate through all other GO's and check if our sharedMesh is shared with others, if so: get ourselves a brand new fresh one!
        if (!Application.isPlaying)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent("MeshRenderer") as MeshRenderer;
            MeshFilter meshFilter = gameObject.GetComponent("MeshFilter") as MeshFilter;
            if (meshRenderer != null && meshFilter != null)
            {
	            if (meshFilter.sharedMesh != null)
	            {
	                GFGearGen[] ggs = GameObject.FindObjectsOfType(typeof(GFGearGen)) as GFGearGen[];
	
	                foreach (GFGearGen gg in ggs)
	                {
	                    MeshFilter otherMeshFilter = gg.GetComponent(typeof(MeshFilter)) as MeshFilter;
	                    if (otherMeshFilter != null)
	                    {
	                        if (otherMeshFilter.sharedMesh == meshFilter.sharedMesh && otherMeshFilter != meshFilter)
	                        {
	                            meshFilter.mesh = new Mesh();
								Rebuild();
	                        }
	                    }
	                }
	            }
	
	            if (meshFilter.sharedMesh == null)
	            {
	                meshFilter.sharedMesh = new Mesh();
					Rebuild();
	            }
			}
			
        }
    }
	
	/// <summary>
	/// Align current gear with the gear specified in alignTo.
	/// </summary>
	/// <param name='alignTo'>
	/// Align to this gear.
	/// </param>
	public void Align(GFGearGen alignTo)
	{ 
        int desiredTeethCount = (int)Mathf.Round((alignTo.numberOfTeeth / alignTo.radius) * this.radius);
        if (this.numberOfTeeth != desiredTeethCount)
        {
            this.numberOfTeeth = desiredTeethCount;
            Rebuild();
        }

        base.Align(alignTo);        
	}
	
	/// <summary>
	/// Align radius of current gear with the gear specified in alignTo based on number of teeth.
	/// </summary>
	/// <param name='alignTo'>
	/// Align to this gear.
	/// </param>
	public void AlignRadius(GFGearGen alignTo)
	{
        float desiredRadius = (alignTo.radius / (float)alignTo.numberOfTeeth) * this.numberOfTeeth;
        if (this.radius != desiredRadius)
        {
            // First set radius to the amount of teeth related to it's radius
            this.radius = desiredRadius;
            Rebuild();
        }

		base.Align(alignTo);
	}

    /// <summary>
    /// Aligns the position and rotation to match gear specified in alignTo.
    /// </summary>
    /// <param name='alignTo'>
    /// Align to.
    /// </param>
    public void AlignPositions(GFGearGen alignTo)
    {
        base.Align(alignTo);
    }

    /// <summary>
    /// Randomize this instance.
    /// Alters number of teeth when AlignTeethWithParent is turned off.
    /// Alters radius, tiplength, tipsize, valleysize, skew, innerRadius and thickness (if it's a 3d gear).
    /// </summary>
    public void Randomize()		
	{
		if (!alignTeethWithParent)
		{
			numberOfTeeth = Random.Range(2, 20);
		}
		
		radius = Random.Range(0.1f, 5.0f);
		
		tipLength = Random.Range (0.1f, 1.0f);
		tipSize = Random.Range (0.0f, 1.0f);
		valleySize = Random.Range (0.0f, 1.0f);
		skew = Random.Range (0.0f, 1.0f);
		if (is3d)
			thickness = Random.Range (0.1f, 2.0f);
		if (Random.Range(0.0f, 1.0f) > 0.5f)
			innerRadius = Random.Range (0.0f, radius-tipLength);
		else 
			innerRadius = 0.0f;
		
		Rebuild();
	}
   
}
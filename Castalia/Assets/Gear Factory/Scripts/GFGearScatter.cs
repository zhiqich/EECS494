using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// [Component] Bonus feature. Generate gears facing along the normals of a plane. Can be used to create large surfaces with gears.
/// </summary>
[AddComponentMenu("Gear Factory/Gear Scatter")]
public class GFGearScatter : MonoBehaviour {
	/// <summary>
	/// Gets the distribution object.
	/// </summary>
	/// <value>
	/// Can only be set from within the GFGearScatter class.
	/// </value>
	public GameObject distributionObject{get; private set;}

	/// <summary>
	/// The GFGearGen entity to be scattered.
	/// </summary>
	public GFGearGen gearEntity;
	
	/// <summary>
	/// Orientation of gears follow the face normal vector.
	/// </summary>
	public bool followNormals = true;
	
	/// <summary>
	/// Hide object while playing.
	/// </summary>
	public bool hideEmitter = true;
	
	public float scale = 1.0f;
	
	private List<Vector3> vectors;
	private List<Vector3> normals;
	private List<GFGearGen> gears;
	
	/// <summary>
	/// Combines meshes instantiated by the ditributer to one mesh. 
	/// A mesh renderer is generated at runtime when not present.
	/// If you wish to have more control: feel free to add a meshrenderer to the distributer gameobject yourself.
	/// When doing so, it will automatically stop creating one at runtime and use yours instead.
	/// </summary>
	public bool combineMeshes = false;
	
	// Use this for initialization
	void Start () {
		distributionObject = this.gameObject;
		
		vectors = new List<Vector3>();
		normals = new List<Vector3>();
			
		//normals = new List<Vector3>();
		gears = new List<GFGearGen>();
		if (distributionObject != null && gearEntity != null)
		{
			MeshFilter m = (MeshFilter)distributionObject.GetComponent(typeof(MeshFilter));
			if (m != null)
			{			
				followNormals = followNormals && m.mesh.normals.Length == m.mesh.vertices.Length && m.mesh.normals.Length > 0;
				
				// Walk through faces
				for (int i=0; i< m.mesh.triangles.Length; i+=3)				
				{
					int[] triangle = new int[]{m.mesh.triangles[i], m.mesh.triangles[i+1], m.mesh.triangles[i+2]};
					
					// For every face, get the three vertices
					// Get the longest distance between two vertices
					
					float d01 = Vector3.Distance(m.mesh.vertices[triangle[0]], m.mesh.vertices[triangle[1]]);
					float d12 = Vector3.Distance(m.mesh.vertices[triangle[1]], m.mesh.vertices[triangle[2]]);
					float d20 = Vector3.Distance(m.mesh.vertices[triangle[2]], m.mesh.vertices[triangle[0]]);
					int idxFrom = 2;
					int idxTo = 0;
//					float dist;// = d20;
					
					if (d01 > d12 && d01 > d20)
					{
						idxFrom = 0;
						idxTo = 1;
//						dist = d01;
					}
					else 
					{
						if (d12 > d01 && d12 > d20)
						{
							idxFrom = 1;
							idxTo = 2;
//							dist = d12;
						}
					}
					
					// Face normal
					Vector3 n = Vector3.zero;
					if (followNormals)
					{
						Vector3 n1 = m.mesh.normals[triangle[1]];
						Vector3 n2 = m.mesh.normals[triangle[2]];
						Vector3 n3 = m.mesh.normals[triangle[0]];
						n = (n1+n2+n3) / 3.0f;
						n.Normalize();
					}
						
					
					// Get center of line segment 
					Vector3 center = Vector3.Lerp(m.mesh.vertices[triangle[idxFrom]], m.mesh.vertices[triangle[idxTo]], 0.5f);
					
					// If center isn't already occupied: add vector
					if (!vectors.Contains(center))
					{
						
						vectors.Add(center);
						if (followNormals)
							normals.Add(n);
					//	normals.Add(center.normalized);
					}
				}
				
				// Walk through vectors and instantiate a gear
				
				GFGearGen gg;
				for (int i=0; i<vectors.Count; i++)
				{
					Vector3 v = /*distributionObject.transform.localToWorldMatrix * */ vectors[i];// + m.transform.position;// m.mesh.vertices[i];
					Quaternion lr = Quaternion.LookRotation(followNormals ? normals[i] : v.normalized);
						
					if (i==0)
					{
						//gearEntity.gameObject.active = false;
						gearEntity.transform.position = v;//distributionObject.transform.TransformPoint(v);
						gearEntity.transform.rotation = lr;
						gearEntity.transform.localRotation.eulerAngles.Set(0.0f,0.0f,0.0f);
						gg = gearEntity;
					}
					else {
						gg = (GFGearGen)Instantiate(gearEntity);//, /*distributionObject.transform.TransformPoint(v)*/v, Quaternion.LookRotation(v.normalized));
					
						gg.transform.parent = gearEntity.transform.parent;
						gg.transform.position = v;
						gg.transform.rotation = lr;
							
						if (gg.gear != null)
						{
							GFGear prevgear = gears[i-1].gear;
							if (prevgear != null)
								gg.gear.DrivenBy = prevgear;
						}
					}
					gears.Add(gg);					
				}	
				
				GFGear gear = gearEntity.gear;// (GFGear)gearEntity.gameObject.GetComponent(typeof(GFGear));
				if (gear != null)
				{
					if (gear.machine != null)
						gear.machine.RecalculateGears();
				}

				
				#region Combine meshes to reduce draw calls
				if (combineMeshes)
				{
					//transform.position = distributionObject.transform.position;
					//transform.rotation = distributionObject.transform.rotation;
					
					List<CombineInstance> cmb = new List<CombineInstance>();
					for (int i=0; i<gears.Count; i++)
					{						 
							MeshFilter mf = gears[i].gameObject.GetComponent<MeshFilter>();
							if (mf != null)
							{
								CombineInstance ci = new CombineInstance();
								ci.mesh = mf.sharedMesh;
								ci.transform = mf.transform.localToWorldMatrix;
								
								mf.gameObject.SetActive(false);
								cmb.Add(ci);
							}					 
					}
					MeshFilter currentMf = transform.GetComponent<MeshFilter>();
					currentMf.SetMesh(new Mesh());
        			currentMf.sharedMesh.CombineMeshes(cmb.ToArray());
					
					MeshRenderer currentMr = transform.GetComponent<MeshRenderer>();
					if (currentMr == null)
						currentMr = (MeshRenderer)transform.gameObject.AddComponent(typeof(MeshRenderer));					
					transform.gameObject.SetActive(true);
				}
				#endregion

				m.GetComponent<Renderer>().enabled = !hideEmitter;
			}
		}		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// [Component] Use this script on any GameObject to let it become a gear factory compliant gear.
/// After that you can power your GameObject by a Machine, by placing it inside the Machine object in your Unity hierarchy.
/// It also enables you to integrate your own gear mesh in the scene, without having you to use the procedural gears from the GFGearGen component.
/// </summary>
[ExecuteInEditMode]
[System.Serializable]
[AddComponentMenu("Gear Factory/Gear")]
public class GFGear : GFBase
{
	
	#region Properties
	/// <summary>
	/// Gets the machine.
	/// </summary>
	/// <value>
	/// The machine.
	/// </value>
	public GFMachine machine
	{
		get {
			if (this.gameObject.transform.parent != null)
			{
				GFMachine parent = this.gameObject.transform.parent.gameObject.GetComponent<GFMachine>();
			
				if (parent != null)
					return parent;
			}
			return null;
		}
	}
	
	/// <summary>
	/// Gets the root objects.
	/// </summary>
	/// <value>
	/// The root objects.
	/// </value>
	private GFGear[] rootGearObjects
	{
		get{
			List<GFGear> go = new List<GFGear>();
			Transform[] ts = Object.FindObjectsOfType<Transform>();
			foreach(Transform t in ts)
			{
				if (t.parent == null)
				{
					GFGear foundGear = t.gameObject.GetComponent<GFGear>();
					if (foundGear != null && !foundGear.gameObject.Equals(this.gameObject))
					{
						go.Add(foundGear);
					}
				}
			}
			return go.ToArray();
		}
	}
	
	
	
	/// <summary>
	/// Gets the other gears. When selected gear is part of a machine group, it will only process the gears within the machine.
	/// </summary>
	/// <value>
	/// The other gears.
	/// </value>
	public GFGear[] otherGears
	{
		get {
			if (machine != null)
				return machine.GetGears(this);
			else 
				return rootGearObjects;
		}
	}
	
	/// <summary>
	/// Gets the mesh.
	/// </summary>
	/// <value>
	/// The mesh.
	/// </value>
	public Mesh mesh
	{
		get {
			return this.gameObject.GetSharedMesh();
		}		
	}
	
	/// <summary>
	/// Set radius of the gear based on the largest axis of the bounding box.
	/// </summary>
	public void SyncRadiusToBounds()
	{        
		if (this.mesh != null)
		{
			Bounds aabb = this.mesh.bounds;
				
			radius = Mathf.Max(aabb.size.x, Mathf.Max(aabb.size.y, aabb.size.z)) / 2.0f;//Vector3.Magnitude(this.mesh.bounds.size);
		}
	}

    private float GetRadius()
    {
        SyncRadiusToBounds();
        return radius;
    }

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="GFGear"/> is turning counter clockwise.
	/// </summary>
	/// <value>
	/// <c>true</c> if counter clockwise; otherwise, <c>false</c>.
	/// </value>
	public bool CCW{
		get; set;
	} 
	
	
	public float speedMultiplier{
		get;set;
	}
	
	public List<GFGear> children = new List<GFGear>();
	public GFGear[] childrenAsArray {
		get
		{
            return children.ToArray();
        }
	}
	#endregion 
	

	#region Public fields (shown in inspector)
	/// <summary>
	/// Movement of the current gear is caused by the gear specified by the driven by parameter.
	/// </summary>
	public GFGear DrivenBy;

    /// <summary>
    /// Snap to nearest gear.
    /// </summary>
    //public bool SnapToNearestGear = true;

	
	/// <summary>
	/// Auto set "driven by" when you intersect with another gear.
	/// Turn this option off when you manually want to connect.
	/// (ex. if you have a gear-axle-gear system when gears are connected but not adjacent/nearby) .
	/// </summary>
	public bool AutoSetDrivenBy = true;

    /// <summary>
    /// Auto align this gear. 
	/// Only visible when there's no GFGearGen component present in the current gameobject.
    /// </summary>
    public bool AutoAlign = false;

    /// <summary>
    /// Reverse rotation: Overrides calculated direction
    /// (ex. to create rolers or wheels)
    /// </summary>
    public bool ReverseRotation = false;
	/// <summary>
	/// Reverse rotation: Overrides calculated direction
	/// Recalculates all gears powered by this gear.
	/// Force enables ReverseRotation after machine execution.
	/// </summary>
	public bool ReverseRotationPlusSubtree = false;
	
	/// <summary>
	/// Sync speed with powered gear: Overrides calculated speed
	/// (ex. like train wheels where one is bigger but using a bar to fixate the speed)
	/// </summary>
	public bool SyncSpeed = false;
	
	#region TODO: move these to descendant class for abstraction
	/// <summary>
	/// The size (length) of the tooth: used as offset to shift the gear into place
	/// </summary>
	public float toothSize {get {return 0.25f;}}
	
    /// <summary>
    /// Speed in degrees per second. Updated by GFMachine.
    /// </summary>
    public float currentSpeed { get { return _currentSpeed; } }
    internal float _currentSpeed = 0f;

    /// <summary>
    /// Returns gear ratio. This is in relation to the gear that powers it.
    /// Same as the speed multiplier.
    /// </summary>
    public float ratio
    {
        get
        {
            return speedMultiplier;
        }
    }

    /// <summary>
    /// Ratio of the gear in relation to the machine powered gear.
    /// </summary>
    public float machineRatio
    {
        get
        {
            return _machineRatio;// currentSpeed / machine.speed; <-doesn't work when speed is 0
        }
    }
    internal float _machineRatio = 1f;


    /// <summary>
    /// The total number of teeth: used to calculate the angle.
    /// Only visible when there's no GFGearGen component present in the current gameobject.
    /// </summary>
    //[HideInInspector] 
    //public int numberOfTeeth = 8;

    [HideInInspector]
	public bool rotateX = false;
	[HideInInspector]
	public bool rotateY = false;
	[HideInInspector]
	public bool rotateZ = true;

	/// <summary>
	/// Convenience getter. Gets the gearGen if available, else it returns null.
	/// </summary>
	/// <value>
	/// The gearGen component.
	/// </value>
	public GFGearGen gearGen {get {return this.gameObject.GetComponent<GFGearGen>();}}
	
	// Following is for avoiding garbage collector
	private Quaternion rotationQuaternion;
	private Vector3 rotationAngles;
	#endregion
	
	#endregion
	
	#region Private fields
	private Vector3 rotationVector; 
	private Quaternion tempRotation;
	private float totalAngle;
	public float angle {get; private set;}
	
	private Quaternion startPos;
	private float finalAngle;
//	private Quaternion endPos;
	#endregion 
	
	#region Basic standard events: Start / Update
	// Use this for initialization
	void Start () {
		//tempRotation = new Quaternion();
		//rotation = this.gameObject.transform.localRotation;
		//totalAngle = 0.0f;
		startPos = this.gameObject.transform.localRotation;
//		endPos = startPos;
		
		rotationQuaternion = Quaternion.identity;
		rotationAngles = Vector3.zero;
	}
	/*
	// Update is called once per frame
	void Update () {
	}*/
	 
	#endregion
	 
	
	#region Custom methods 
 
	public void SetRotation(float angle)
	{ 
		this.angle = angle;
		finalAngle = (CCW ? -1.0f : 1.0f) * angle;
		
		if (rotateX)
			rotationAngles.x = finalAngle;
		if (rotateY)
			rotationAngles.y = finalAngle;
		if (rotateZ)
			rotationAngles.z = finalAngle;
		rotationQuaternion.eulerAngles = rotationAngles;
		this.gameObject.transform.localRotation = startPos * rotationQuaternion;
	}
	
	public bool Intersects(GFGear otherGear)
	{
		MeshUtils.IntersectType it = MeshUtils.Intersect(MeshUtils.CalcOrientedBoundingBox(this.gameObject), MeshUtils.CalcOrientedBoundingBox(otherGear.gameObject));
		return it == MeshUtils.IntersectType.Intersect || it == MeshUtils.IntersectType.Inside;
	}
	
	#endregion
	
    /// <summary>
    /// Clone a gear and place the duplicate version aligned.
    /// </summary>
    /// <param name="direction">Direction at which to align it to</param>
    /// <returns>The cloned gear as gameobject</returns>
	public GameObject Clone(Vector3 direction)
	{
		GFGear gear = this;
		GFGearGen gearGen = (GFGearGen)this.gameObject.GetComponent(typeof(GFGearGen));
		float d;
		if (gearGen != null)
			d = gearGen.radius + (gearGen.radius - gearGen.tipLength);
		else 
			d = (gear.GetRadius() * 2.1f) - gear.toothSize;
			
		GameObject newGearGO = (GameObject)Instantiate(gear.gameObject);
		newGearGO.transform.parent = gear.gameObject.transform.parent;
		GFGear newGear = newGearGO.GetComponent(typeof(GFGear)) as GFGear;		
		newGear.DrivenBy = gear;
		newGear.AutoSetDrivenBy = false;
		
		GFGearGen newGearGen = newGear.GetComponent<GFGearGen>();
		if (newGearGen != null)
		{
			newGearGen.alignTeethWithParent = true;
			newGearGen.alignRadiusWithParent = true;
		}
		
		newGearGO.transform.position = gear.transform.position + (direction.normalized * d);
		
		// Get number that is not followed by any other number
		string re = @"(\d+)(?!.*\d)";
		Match m = Regex.Match(gear.gameObject.name, re);
		int nmbr = 1;
		// Add one to the last number (if exists)
		if (m != null && m.Value != null && m.Value != "" && gear.DrivenBy != null)
			nmbr = int.Parse(m.Value)+1;
		// Rename object to sequentially numbered object
		newGearGO.name = Regex.Replace(gear.gameObject.name, re, "").Trim() + " " + nmbr.ToString();
		
		newGearGen.Align(gearGen);

		return newGearGO;
	}


    /// <summary>
    /// Set machine speed to match a gear speed.
    /// </summary>
    /// <param name="gearSpeed">Desired new speed of the given gear in degrees per second.</param>
    public void SetSpeed(float gearSpeed)
    {
        machine.SetSpeedByGear(this, gearSpeed);
    }

}

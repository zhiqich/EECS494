/*! \mainpage Gear Factory by Atomic Crew
 *
 * \section intro_sec Introduction
 *
 * Gear Factory is a <A HREF="http://www.unity3d.com">Unity3D</A> component package for:
 * - procedural generation of gears
 * - rotation based on gear ratio 
 * 
 * If you want to know more, check out our <A HREF="http://www.atomiccrew.com/gearfactory/">product page</A> first.
 *
 * \section install_sec Installation
 *
 * \subsection step1 Purchased from Unity asset store
 * - Install through the asset store.
 * 
 * \subsection step2_sec Purchased from our site or a reseller
 * - Download the package through the provided link.
 * - If the package is compressed, first extract it into a temporary directory.
 * - Start Unity.
 * - Open your project in which you would like to use Gear Factory or start a new one.
 * - In the main menu go to: Assets > Import package > Custom package ... 
 * - Select the extracted .unitypackage
 * - Done!
 * 
 * \section started_sec Getting started
 * 
 * - From the main menu, choose GameObject > Create Other > Gear Factory > Gear.
 * - You should see a procedurally created gear without any materials on it in your scene.
 * - From the main menu, choose GameObject > Create Other > Gear Factory > Machine. 
 * - In your hierarchy select the Machine.
 * - From your hierarchy drag the Gear we just created into the "Powered Gear" property of the Machine inspector.
 * - Click Play and watch your first Gear doing a 360 ! 
 * 
 * \section more_sec Final note
 * 
 * All sourcecode is included. 
 * The sourcecode is only available in C# (for now) to keep the maintenance update frequency up to speed.
 * Licensing is on a per seat basis. This means you are only licensed to use the package for a single developer only. 
 *  
 * The possibilities are endless. 
 * Keep in mind: all automation can be disabled. 
 * The separation of the core engine from the mesh generator makes it possible to create any mesh you like, and integrate it into a machine powered contraption.
 *  
 * Creative freedom is where you need it!
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// [GameObject] Powers one gear and calculates all turn directions and speeds based on their ratio.
/// </summary>
[ExecuteInEditMode]
public class GFMachine : MonoBehaviour {
	
	#region Public global fields (shown in inspector)
	/// <summary>
	/// Show arrow on gear which points to parent that's driving the gear
	/// </summary>
	public bool ShowPoweredByIndicator = true;
	#endregion 

	public GFGear PoweredGear = null;
	[HideInInspector]
	public float maxSpeed = 1000.0f;	
	[HideInInspector]
	public float speed = 20.0f;
	//public float stepRevResInDegrees = 360.0f; // Revolution resolution: Number of steps per full rotation: higher number == smoother
	[HideInInspector]
	public float step = 0.0f;
	
	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="GFMachine"/> is started.
	/// </summary>
	/// <value>
	/// <c>true</c> if is started; otherwise, <c>false</c>.
	/// </value>
	public bool isStarted {get;	private set;}
		
	/// <summary>
	/// Turn directions of all gears are reversed.
	/// </summary>
	public bool Reverse = false; 
	/// <summary>
 	/// Reverse turn direction of gears when normals, of the front facing plane, are in opposite direction to eachother. 
	/// </summary>
	public bool AutoReverseOnReversedOrientation = true;
	
	private	Queue<GFGear> gearUpdateQueue = new Queue<GFGear>();

	/// <summary>
	/// Used as temporary buffer to store history when determining turn directions.
	/// </summary>
	private List<GFGear> history = new List<GFGear>();   
	
	/// <summary>
	/// Gets the gears driven by currentGear.
	/// </summary>
	/// <returns>
	/// The gears.
	/// </returns>
	/// <param name='currentGear'>
	/// Current gear.
	/// </param>
	public GFGear[] GetGears(GFGear currentGear)
	{
		List<GFGear> go = new List<GFGear>();
		for (int i=0; i<this.transform.childCount; i++)
		{
			GFGear foundGear = this.transform.GetChild(i).gameObject.GetComponent<GFGear>();
			if (foundGear != null && (currentGear == null || !foundGear.gameObject.Equals(currentGear.gameObject)))
			{
				go.Add(foundGear);
			}
		}
		return go.ToArray();
	}
	
	/// <summary>
	/// Gets the gears.
	/// </summary>
	/// <returns>
	/// The gears.
	/// </returns>
	public GFGear[] GetGears()
	{
		return GetGears(null);
	}	
	
	private GFGear[] gears;  
	
		
	void Start () {
		CalculateGears(); 
		StartMachine();
	}

	// Update is called once per frame
	void Update () {
		if (PoweredGear != null)
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				// Make sure the poweredgear is only powered by this machine
				if (PoweredGear.DrivenBy != null)
				{
					PoweredGear.DrivenBy = null;
					UnityEditor.EditorUtility.SetDirty(PoweredGear);
				}
			}
			else
			{
#endif
				if (isStarted && speed > 0.0f)
					Step(speed);
#if UNITY_EDITOR
			}
#endif
		}
	}
	
	/// <summary>
	/// Starts the machine.
	/// </summary>
	public void StartMachine()	
	{
		isStarted = true;
	}
	
	/// <summary>
	/// Stops the machine.
	/// </summary>
	public void StopMachine()
	{
		isStarted = false;
	}
	
	/// <summary>
	/// Resets the machine.
	/// Sets rotation to zero, sets speed to zero.
	/// </summary>
	public void ResetMachine()
	{
		StopMachine();
		step = 0.0f;		
		Step(0.0f);
	}
	
	/// <summary>
	/// Step the machine.
	/// Set machine speed to zero and use this method for interactive use.
	/// It's framerate independent.
	/// </summary>
	/// <param name='speed'>
	/// Speed in degrees per second.
	/// </param>
	public void Step(float speed)
	{ 
		step += Time.deltaTime * speed * (Reverse ? -1.0f : 1.0f);
		//FIXME: individual gear rotated > 360 reset at 0: not through step: if powered gear is smaller than other gear in same machine: larger gear gets out of sync as it has not completed its turn yet.
		PoweredGear.SetRotation(step);
        PoweredGear._currentSpeed = speed;

        // Avoiding recursion through use of a queue instead of the internal stack which is slow and limited.
        //gearUpdateQueue.Clear();
        gearUpdateQueue.Enqueue(PoweredGear);
		while (gearUpdateQueue.Count > 0){
			GFGear parent = gearUpdateQueue.Dequeue();
			if (parent != null){
				for (int i=0; i<parent.childrenAsArray.Length; i++)
				{ 
					GFGear child = parent.childrenAsArray[i];
                    child._currentSpeed = parent.currentSpeed * child.speedMultiplier;
                    child.SetRotation(parent.angle * child.speedMultiplier);
					gearUpdateQueue.Enqueue(child);
				}
			}
		}
	}
	
	/// <summary>
	/// This method is only called once on start.
	/// It precalculates all turn directions and speeds.
	/// </summary>
	private void CalculateGears()
	{
		gears = GetGears();
		history.Clear();
		ResetHierarchy();

        RebuildHierarchy();

		if (PoweredGear != null)
		{
			RebuildTurnDirections(PoweredGear, PoweredGear.ReverseRotation, 1.0f);
		}
	}

	/// <summary>
	/// Call this method only when the structure of the machine changes.
	/// Ex. if you add or remove gears, or change their properties or positions at runtime.
	/// </summary>
	public void RecalculateGears()
	{
		CalculateGears();
	}
	
	/// <summary>
	/// Rotates g1 and g2 with point of collission of front facing center planes as pivot point.
	/// Calculates scalar products to determine if normals are in opposite direction of eachother and returns false if that's the case.
	/// </summary>
	/// <param name='g1'>
	/// Gear 1
	/// </param>
	/// <param name='g2'>
	/// Gear 2
	/// </param>
	/// <returns>True if normals align after rotation around point of collision of planes.</returns>
	private bool Opposing(GFGear g1, GFGear g2)
	{
		if (AutoReverseOnReversedOrientation)
		{
			Vector3 c1 = g1.transform.position;
			Vector3 c2 = g2.transform.position;
			

			// Calculate planes through center
			Plane p1 = g1.gameObject.CalcYAlignedCenterPlane(Space.World);
			Plane p2 = g2.gameObject.CalcYAlignedCenterPlane(Space.World);
			
			// Calculate g1.center --> point of nearest collision of the two planes (c1) --> g2.center
			//Vector3 point = Vector3.zero;
			//Vector3 direction = Vector3.zero;
			//MathUtils.PlanePlaneIntersection(out point, out direction, p1, p2, c1, c2);

			// If parallel planes ? skip pivot rotation
//			if (direction != Vector3.zero)
//			{
				// Calculate triangle: Center (c1) -> Center translated by normal -> Plane intersection point (point)
				Plane t1 = new Plane(c1, c1+p1.normal, c2);
				// Calculate triangle: Center (c2) -> Center translated by normal -> Plane intersection point (point)
				Plane t2 = new Plane(c2, c2+p2.normal, c1);
 /*
Debug.DrawLine(c1, c1+p1.normal, Color.green, 3, false);
Debug.DrawLine(c1+p1.normal, c2, Color.green, 3, false);
Debug.DrawLine(c2, c1, Color.green, 3, false);
Debug.DrawLine(c1, c1+t1.normal, Color.blue, 3, false);

Debug.DrawLine(c2, c2+p2.normal, Color.magenta, 3, false);
Debug.DrawLine(c2+p2.normal, c1, Color.magenta, 3, false);
Debug.DrawLine(c1, c2, Color.magenta, 3, false);
Debug.DrawLine(c2, c2+t2.normal, Color.cyan, 3, false);
 */
				// Calculate normals of those triangles: directions must be opposite
				return t1.normal == t2.normal;
			/*
			}
			else
			{
				// Parallel planes but facing the same sides: return true 
				return p1.normal == p2.normal;					
			}*/
		}
		
		return false;
	}
	
	private float GetGearRatio(GFGear gear1, GFGear gear2)
	{
		return (float)gear1.numberOfTeeth / (float)gear2.numberOfTeeth;	//gear1.radius / gear2.radius;
	}
	
	/// <summary>
	/// Clears all children from all gears.
	/// </summary>
	private void ResetHierarchy()
	{
		foreach(GFGear g in gears)
		{
			g.children.Clear();
		}		
	}

    /// <summary>
    /// Inverse the parent hierarchy to become children of the powered gear.
    /// </summary>
    private void RebuildHierarchy()
    {
        if (PoweredGear != null)
        {
            List<GFGear> gearsToInverse = new List<GFGear>();
            GFGear parentToInv = PoweredGear;
            while (parentToInv != null && !gearsToInverse.Contains(parentToInv))
            {
                gearsToInverse.Add(parentToInv);
                parentToInv = parentToInv.DrivenBy;
            }

            for (int i = gearsToInverse.Count - 1; i > 0; i--)
            {
                gearsToInverse[i].AutoSetDrivenBy = false;
                gearsToInverse[i].DrivenBy = gearsToInverse[i - 1];
            }

            PoweredGear.AutoSetDrivenBy = false;
            PoweredGear.DrivenBy = null;
        }
    }

    private void RebuildTurnDirections(GFGear poweredBy, bool ccw, float speedMultiplier, float globalMultiplier = 1f)
	{ 
		if (!history.Contains(poweredBy))
		{ 

			history.Add(poweredBy);
			poweredBy.CCW = ccw;
			poweredBy.speedMultiplier = speedMultiplier;
            poweredBy._machineRatio = globalMultiplier;

            //if (poweredBy == null)
            //    poweredBy._machineRatio = 1f;

            foreach (GFGear g in gears)
			{
				// Iterate through gears and clean up empty children
				int i = 0;
				while (i < g.children.Count)
				{
					if (g.children[i] == null)
						g.children.RemoveAt(i);
					else 
						i++;
				}
				
				// Find our gear in the gears collection
				if (g.DrivenBy == poweredBy)
				{
					bool newccw = ccw;
					
					if (!poweredBy.children.Contains(g))
						poweredBy.children.Add(g);
					
					// If g is to the left of the gear: turn the otherside around so we don't have to flip it manually
					// we construct a plane through the center of the gear and determine if the center position of the other gear is on the backside of it
					if (Opposing(g, poweredBy) || (poweredBy.gearGen != null && poweredBy.gearGen.innerTeeth) || (g.gearGen != null && g.gearGen.innerTeeth))
						newccw = !ccw;
						
					//calculate new speedMultiplier: compare g with poweredBy number of teeth (much more accurate then radius):  
					if (!g.SyncSpeed)
					{
					    speedMultiplier = GetGearRatio(poweredBy, g);
                      //_machineRatio = poweredBy._machineRatio * g.speedMultiplier;
                    }
					else
					{
						speedMultiplier = 1.0f; 
					}
					
					if (poweredBy.ReverseRotationPlusSubtree)
					{
						g.ReverseRotation = true;
						newccw = !newccw;
						#if UNITY_EDITOR
						// Tell the unity editor we changed something by code, so it gets saved.
						UnityEditor.EditorUtility.SetDirty(g);
						#endif
					}
					
					RebuildTurnDirections(g, !newccw, speedMultiplier, globalMultiplier * speedMultiplier);

					if (g.ReverseRotation && !g.ReverseRotationPlusSubtree)
					{
						g.CCW = !g.CCW;
					}
				}	
			}
		}
		
		/*
		gearUpdateQueue.Clear();

		poweredBy.CCW = ccw;
		poweredBy.speedMultiplier = speedMultiplier;
		gearUpdateQueue.Enqueue(poweredBy);
		history.Clear();
		while (gearUpdateQueue.Count > 0)
		{
			GFGear parent = gearUpdateQueue.Dequeue();	
			if (parent != null){
				if (!history.Contains(parent))
				{
					history.Add(parent);
					
					foreach(GFGear g in gears)
					{
						if (g.DrivenBy == parent)
						{
							bool newccw = ccw;
							
							if (!parent.Children.Contains(g))
								parent.Children.Add(g);
							
							// If g is to the left of the gear: turn the otherside around so we don't have to flip it manually
							// we construct a plane through the center of the gear and determine if the center position of the other gear is on the backside of it
							if (Opposing(g, parent))
								newccw = !ccw;
								
							//calculate new speedMultiplier: compare g with poweredBy number of teeth (much more accurate then radius):  
							if (!g.SyncSpeed)
							{
							  speedMultiplier = GetGearRatio(parent, g);  
							}
							
							if (g.ReverseRotationPlusSubtree)
							{
								newccw = !newccw;
								if (!g.ReverseRotation)
								{
									g.ReverseRotation = true;
									// Tell the unity editor we changed something by code, so it gets saved.
									UnityEditor.EditorUtility.SetDirty(g);
								}
							}
												
//							RebuildTurnDirections(g, !newccw, speedMultiplier);
							g.CCW = !newccw;
							g.speedMultiplier = speedMultiplier;
							
							gearUpdateQueue.Enqueue(g);
						}
					}
				}
			}
		}
		
		foreach (GFGear g in history){
			if (g.ReverseRotation && !g.ReverseRotationPlusSubtree)
			{
				g.CCW = !g.CCW;
			}
		}*/
	}

    /// <summary>
    /// Set machine speed to match a gear speed.
    /// </summary>
    /// <param name="gear">Adapt speed to this gear.</param>
    /// <param name="gearSpeed">Desired new speed of the given gear in degrees per second.</param>
    public void SetSpeedByGear(GFGear gear, float gearSpeed)
    {
        speed = gearSpeed / gear.machineRatio;
    }
}

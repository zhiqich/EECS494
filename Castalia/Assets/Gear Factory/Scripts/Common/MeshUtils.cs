using UnityEngine;
using System.Collections.Generic;

public static class MeshUtils {
	#region 3d Math functions (by Atomiccrew (Dennis van Haazel))
	#region Comments
	/* Bounding box (local)                                                       
	                                                                            
	                                                                            
	                                                                            
	                                                                            
	                      P5                                     P8             
	                     ****************************************               
	                    **                                     **               
	                   * *                                    * *               
	                P6*  *                                 P7*  *               
	                 ****************************************   *               
	                 *   *                                  *   *               
	                 *   *                                  *   *               
	                 *   *                                  *   *               
	                 *   *                                  *   *               
	                 *   *                                  *   *               
	                 *   *                                  *   *               
	                 *   *                                  *   *               
	     Y           *   *                                  *   *               
	     *           *   *                                  *   *               
	     *           *   *                                  *   *               
	     *    Z      *   *                                  *   *               
	     *   *       *   ****************************************               
	     *  *        *  * P1                                *  * P4             
	     * *         * *                                    * *                 
	     **          **                                     **                  
	     ********X   ****************************************                   
	    0            P2                                     P3                  
	                                                                            */
		
	/* Axis aligned planes (local)                                                
	                                                                            
	                                                                            
	                                     *                                      
	                                    **                                      
	                                   * *                                      
	                                  *  *                                      
	                     *********************************                      
	               **********************************   *                       
	               *   *           *   .            *  *                        
	               *  *           *.  .             * *                         
	               * *           * . .              **                          
	               **           *  ..               *                           
	               * ......... *...................**                           
	              *           *   ..0             * *                           
	             *            *  . .             *  *                           
	            *             * .  .            *   *                           
	           *              *.   .           ****** <--- Y-aligned            
	          *               *    .          *                                 
	         ********************************* <--- X-aligned                   
	                          *   *                                             
	                          *  *                                              
	                          * *                                               
	                          **                                                
	                          * <--- Z-aligned                                  
	                                                                            
	                                                                            
	                                                                            
	                                                                            
	 Normals follow positive directions                                         */
	#endregion
	
	public enum PlaneSide {Back, Front};
	public enum IntersectType {Outside, Inside, Intersect};

	static public PlaneSide TestPoint(Plane p, Vector3 point)
	{		
		return p.GetSide(point) ? PlaneSide.Front : PlaneSide.Back; 
	}
	
	static public IntersectType Intersect(Vector3[] obb1, Vector3[] obb2)
	{
		if (obb1 != null && obb2 != null)
		{
			int len = obb2.Length;
			int vin = 0;
			int vout = 0;
			Plane[] p = {	new Plane(obb1[2-1], obb1[1-1], obb1[3-1]),
			  				new Plane(obb1[3-1], obb1[4-1], obb1[7-1]),
							new Plane(obb1[7-1], obb1[8-1], obb1[6-1]),
							new Plane(obb1[6-1], obb1[5-1], obb1[1-1]),
							new Plane(obb1[1-1], obb1[8-1], obb1[4-1]),
							new Plane(obb1[2-1], obb1[3-1], obb1[6-1])
			};
			
			for (int i = 0; i < p.Length; i++) 
			{
				vout = 0;
	
				for (int j = 0; j < len; j++)
				{
					if (TestPoint(p[i], obb2[j]) == PlaneSide.Back) 
						vout++; 
					else 
						vin++;
				}
				
				// All corners of mesh outside planes: mesh is outside 
				if (vout == len)
				{
					return IntersectType.Outside;
				}
			}
	
			if (vin == 8 * p.Length)
				return IntersectType.Inside;
			else
				return IntersectType.Intersect; 	
		}
		else 
			return IntersectType.Outside;
	}
	
	static public Vector3[] CalcOrientedBoundingBox(this GameObject g1)
	{
		// Calc bounding volume
		Mesh m = GetSharedMesh(g1);
		if (m != null)
		{
			Vector3 mi = m.bounds.min;
			Vector3 ma = m.bounds.max;
			
			// If z remains 0 depth, it's probably 2d: force it to 1.0f
			if (m.bounds.extents.z == 0){
				ma.z = mi.z + 1.0f;
			}
				
			Vector3 p1 = new Vector3(mi.x, mi.y, ma.z);
			Vector3 p2 = new Vector3(mi.x, mi.y, mi.z);
			Vector3 p3 = new Vector3(ma.x, mi.y, mi.z);
			Vector3 p4 = new Vector3(ma.x, mi.y, ma.z);
			Vector3 p5 = new Vector3(mi.x, ma.y, ma.z);
			Vector3 p6 = new Vector3(mi.x, ma.y, mi.z);
			Vector3 p7 = new Vector3(ma.x, ma.y, mi.z);
			Vector3 p8 = new Vector3(ma.x, ma.y, ma.z);
			
			p1 = g1.transform.TransformPoint(p1);
			p2 = g1.transform.TransformPoint(p2);
			p3 = g1.transform.TransformPoint(p3);
			p4 = g1.transform.TransformPoint(p4);
			p5 = g1.transform.TransformPoint(p5);
			p6 = g1.transform.TransformPoint(p6);
			p7 = g1.transform.TransformPoint(p7);
			p8 = g1.transform.TransformPoint(p8);
			
			return new Vector3[]{p1,p2,p3,p4,p5,p6,p7,p8};
		}
		else 
			return null;
	}

	/// <summary>
	/// Calculates the X aligned center plane.
	/// </summary>
	/// <returns>
	/// The X aligned center plane.
	/// </returns>
	/// <param name='g1'>
	/// Game object
	/// </param>
	/// <param name='space'>
	/// If space is set to World (default): the plane will be transformed to worldcoordinates.
	/// </param>
	static public Plane CalcXAlignedCenterPlane(this GameObject g1, Space space)
	{		
		return new Plane(
			space == Space.World ? g1.transform.TransformDirection(Vector3.up) : Vector3.up, 
			space == Space.World ? g1.transform.TransformPoint(Vector3.zero) : Vector3.zero
		);
		/*
		Mesh m = GetMesh(g1);
		if (m != null)
		{
			Vector3 mi = m.bounds.min;
			Vector3 ma = m.bounds.max;
			Vector3 p6 = new Vector3(mi.x, 0, mi.z);
			Vector3 p7 = new Vector3(ma.x, 0, mi.z);
			Vector3 p8 = new Vector3(ma.x, 0, ma.z);
			
			if (space == Space.World)
			{
				p6 = g1.transform.TransformPoint(p6);
				p7 = g1.transform.TransformPoint(p7);
				p8 = g1.transform.TransformPoint(p8);
			}
			return new Plane(p7,p8,p6);
		} */
	}
	static public Plane CalcXAlignedCenterPlane(this GameObject g1){return CalcXAlignedCenterPlane(g1, Space.World);}

	/// <summary>
	/// Calculates the Y aligned center plane.
	/// Positive Z is frontside.
	/// </summary>
	/// <returns>
	/// The Y aligned center plane.
	/// </returns>
	/// <param name='g1'>
	/// Game object
	/// </param>
	/// <param name='space'>
	/// If space is set to World (default): the plane will be transformed to worldcoordinates.
	/// </param>
	static public Plane CalcYAlignedCenterPlane(this GameObject g1, Space space)
	{
		return new Plane(
			space == Space.World ? g1.transform.TransformDirection(Vector3.forward) : Vector3.forward, 
			space == Space.World ? g1.transform.TransformPoint(Vector3.zero) : Vector3.zero
		);
	}
	static public Plane CalcYAlignedCenterPlane(this GameObject g1){return CalcYAlignedCenterPlane(g1, Space.World);}

	/// <summary>
	/// Calculates the Z aligned center plane.
	/// Positive Y is frontside.
	/// </summary>
	/// <returns>
	/// The Z aligned center plane.
	/// </returns>
	/// <param name='g1'>
	/// Game object
	/// </param>
	/// <param name='space'>
	/// If space is set to World (default): the plane will be transformed to worldcoordinates.
	/// </param>
	static public Plane CalcZAlignedCenterPlane(this GameObject g1, Space space)
	{
		return new Plane(
			space == Space.World ? g1.transform.TransformDirection(Vector3.right) : Vector3.right, 
			space == Space.World ? g1.transform.TransformPoint(Vector3.zero) : Vector3.zero
		);
	}
	static public Plane CalcZAlignedCenterPlane(this GameObject g1){return CalcZAlignedCenterPlane(g1, Space.World);}
	
	/// <summary>
	/// Gets the center.
	/// </summary>
	/// <returns>
	/// The center.
	/// </returns>
	/// <param name='p1'>
	/// P1.
	/// </param>
	static public Vector3 GetCenter(this Plane p1)
	{
		return p1.normal.SetVectorLength(p1.distance);
	} 
	#endregion


	
	/// <summary>
	/// Gets the mesh from a gameobject.
	/// If no mesh is assigned to the mesh filter a new mesh will be created and assigned.
	/// If the mesh assigned to the mesh filter is shared, it will be automatically duplicated and the instantiated mesh will be returned.
	/// By using mesh property you can modify the mesh for a single object only. The other objects that used the same mesh will not be modified.
	/// [url]http://docs.unity3d.com/Documentation/ScriptReference/MeshFilter-mesh.html[/url]
	/// </summary>
	/// <returns>
	/// The mesh or null when no meshfilter available.
	/// Returns the instantiated Mesh assigned to the mesh filter.
	/// </returns>
	/// <param name='g1'>
	/// The game object
	/// </param>
	static public Mesh GetMesh(this GameObject g1)
	{
		MeshFilter mf = g1.GetComponent<MeshFilter>();
		return mf.GetMesh();
	}

	/// <summary>
	/// Gets the sharedmesh from a gameobject.
	/// It is recommended to use this function only for reading mesh data and not for writing, since you might modify imported assets and 
	/// all objects that use this mesh will be affected. Also, be aware that is not possible to undo the changes done to this mesh.
	/// [url]http://docs.unity3d.com/Documentation/ScriptReference/MeshFilter-sharedMesh.html[/url]
	/// </summary>
	/// <returns>
	/// The sharedmesh or null when no meshfilter available.
	/// </returns>
	/// <param name='g1'>
	/// The game object
	/// </param>
	static public Mesh GetSharedMesh(this GameObject g1)
	{
		MeshFilter mf = g1.GetComponent<MeshFilter>();
		return mf.GetSharedMesh();
	}
	
	/// <summary>
	/// Gets the mesh from a MeshFilter.
	/// If no mesh is assigned to the mesh filter a new mesh will be created and assigned.
	/// If the mesh assigned to the mesh filter is shared, it will be automatically duplicated and the instantiated mesh will be returned.
	/// By using mesh property you can modify the mesh for a single object only. The other objects that used the same mesh will not be modified.
	/// [url]http://docs.unity3d.com/Documentation/ScriptReference/MeshFilter-mesh.html[/url]
	/// </summary>
	/// <returns>
	/// The mesh or null when no meshfilter available.
	/// Returns the instantiated Mesh assigned to the mesh filter.
	/// </returns>
	/// <param name='meshFilter'>
	/// The mesh filter 
	/// </param>
	static public Mesh GetMesh(this MeshFilter meshFilter)
	{
		if (meshFilter != null)
		{
			return meshFilter.mesh;
		}
		return null;
	}

	/// <summary>
	/// Gets the sharedmesh from a MeshFilter.
	/// It is recommended to use this function only for reading mesh data and not for writing, since you might modify imported assets and 
	/// all objects that use this mesh will be affected. Also, be aware that is not possible to undo the changes done to this mesh.
	/// [url]http://docs.unity3d.com/Documentation/ScriptReference/MeshFilter-sharedMesh.html[/url]
	/// </summary>
	/// <returns>
	/// The sharedmesh or null when no meshfilter available.
	/// </returns>
	/// <param name='meshFilter'>
	/// The mesh filter 
	/// </param>
	static public Mesh GetSharedMesh(this MeshFilter meshFilter)
	{
		if (meshFilter != null)
		{
			return meshFilter.sharedMesh;
		}
		return null;
	}
	
	/// <summary>
	/// Sets the sharedmesh of a mesh filter and destroys the old one preventing "leaked mesh" warnings.
	/// </summary>
	/// <param name='meshFilter'>
	/// Meshfilter
	/// </param>
	/// <param name='mesh'>
	/// Mesh to assign
	/// </param>
	static public void SetSharedMesh(this MeshFilter meshFilter, Mesh mesh)
	{
		Mesh oldMesh = meshFilter.sharedMesh;
	
		meshFilter.sharedMesh = mesh;
		if (oldMesh != null)
		{
			if (Application.isEditor)
				Object.DestroyImmediate(oldMesh);
			else 
				Object.Destroy(oldMesh);
		}
	}
	
	/// <summary>
	/// Sets the sharedmesh of the first mesh filter found in a given game object.
	/// If no mesh filter was found, it adds one to the game object.
	/// </summary>
	/// <param name='g1'>
	/// Game object 
	/// </param>
	/// <param name='mesh'>
	/// Mesh to assign
	/// </param>
	static public void SetSharedMesh(this GameObject g1, Mesh mesh)
	{
		MeshFilter mf = g1.GetComponent<MeshFilter>();
		if (mf == null)
			mf = (MeshFilter)g1.AddComponent(typeof(MeshFilter));
		
		if (mf != null)
		{
			SetSharedMesh(mf, mesh);		
		}		
	}	

	/// <summary>
	/// Sets the mesh of a mesh filter and destroys the old one preventing "leaked mesh" warnings.
	/// </summary>
	/// <param name='meshFilter'>
	/// Meshfilter
	/// </param>
	/// <param name='mesh'>
	/// Mesh to assign
	/// </param>
	static public void SetMesh(this MeshFilter meshFilter, Mesh mesh)
	{
		Mesh oldMesh = meshFilter.mesh;
		meshFilter.mesh = mesh;
		if (oldMesh != null)
		{
			if (Application.isEditor)
				Object.DestroyImmediate(oldMesh);
			else 
				Object.Destroy(oldMesh);
		}
	}
	
	/// <summary>
	/// Sets the mesh of the first mesh filter found in a given game object.
	/// If no mesh filter was found, it adds one to the game object.
	/// </summary>
	/// <param name='g1'>
	/// Game object 
	/// </param>
	/// <param name='mesh'>
	/// Mesh to assign
	/// </param>
	static public void SetMesh(this GameObject g1, Mesh mesh)
	{
		MeshFilter mf = g1.GetComponent<MeshFilter>();
		if (mf == null)
			mf = (MeshFilter)g1.AddComponent(typeof(MeshFilter));
		
		if (mf != null)
		{
			SetMesh(mf, mesh);		
		}		
	}	
	
	
	
	/*
	 	Calculate tangents (the vector that's perpendicular to the normal in direction of the positive U coordinates).
	 	
		Derived from
		[url]http://forum.unity3d.com/threads/38984-How-to-Calculate-Mesh-Tangents[/url]
		Lengyel, Eric. “Computing Tangent Space Basis Vectors for an Arbitrary Mesh”. Terathon Software 3D Graphics Library, 2001.
		[url]http://www.terathon.com/code/tangent.html[/url]
	*/
    static public void TangentSolver(Mesh mesh)
    {
		if (mesh.uv.Length != mesh.vertexCount)
		{
			// No UV's found, so no tangents can be calculated
			return;
		}
		
		//speed up math by copying the mesh arrays
    	int[] triangles = mesh.triangles;
    	Vector3[] vertices = mesh.vertices;
    	Vector2[] uv = mesh.uv;
    	Vector3[] normals = mesh.normals;

	    //variable definitions
	    int triangleCount = triangles.Length;
	    int vertexCount = vertices.Length;
	
	    Vector3[] tan1 = new Vector3[vertexCount];
	    Vector3[] tan2 = new Vector3[vertexCount];
	
	    Vector4[] tangents = new Vector4[vertexCount];
	
	    for (long a = 0; a < triangleCount; a += 3)
	    {
	        long i1 = triangles[a + 0];
	        long i2 = triangles[a + 1];
	        long i3 = triangles[a + 2];
	
	        Vector3 v1 = vertices[i1];
	        Vector3 v2 = vertices[i2];
	        Vector3 v3 = vertices[i3];
	
	        Vector2 w1 = uv[i1];
	        Vector2 w2 = uv[i2];
	        Vector2 w3 = uv[i3];
	
	        float x1 = v2.x - v1.x;
	        float x2 = v3.x - v1.x;
	        float y1 = v2.y - v1.y;
	        float y2 = v3.y - v1.y;
	        float z1 = v2.z - v1.z;
	        float z2 = v3.z - v1.z;
	
	        float s1 = w2.x - w1.x;
	        float s2 = w3.x - w1.x;
	        float t1 = w2.y - w1.y;
	        float t2 = w3.y - w1.y;
				
			float div = s1 * t2 - s2 * t1;
	        float r = div == 0.0f ? 0.0f : 1.0f / div;
	
	        Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
	        Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
	
	        tan1[i1] += sdir;
	        tan1[i2] += sdir;
	        tan1[i3] += sdir;
	
	        tan2[i1] += tdir;
	        tan2[i2] += tdir;
	        tan2[i3] += tdir;
				
				if (float.IsNaN(tan1[1].x))
					Debug.Log("IsNaN");
	    }


	    for (long a = 0; a < vertexCount; ++a)
	    {
	        Vector3 n = normals[a];
	        Vector3 t = tan1[a];
	
	        //Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
	        //tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
	        Vector3.OrthoNormalize(ref n, ref t);
	        tangents[a].x = t.x;
	        tangents[a].y = t.y;
	        tangents[a].z = t.z;
	
	        tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
	    }
	
	    mesh.tangents = tangents;
  	}
	static public Vector3 Normal(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		Vector3 direction = Vector3.Cross(v2-v1, v3-v1);
		return direction.normalized;
	}
	/*
	// Checks overlapping vertices and averages normals from startIndex to endIndex (including)
	// weldVertices ? Find overlapping vertices, set triangles to first vertex and eliminate the other vertices from vertices and normals
	static public void SmoothNormals(float sharpEdgeAngleThreshold, List<Vector3> vertices, List<Vector3> normals)//, bool weldVertices, List<int> faces)
	{
		// Find all overlapping vertices
		Vector3[] vs = vertices.ToArray();
		List<int> doubles = new List<int>();
		for (int i=0; i<vs.Length-1; i++)
		{			
			// First find all overlapping vertices
			doubles.Clear();
			for(int ii=i+1; ii<vs.Length;ii++)
			{
				// Check if vertices overlap and normals differ (if normals overlap, we already processed this one)
				if (vs[ii] == vs[i]) //Vector3.Distance(vs[ii], vs[i]) < 0.1f)// && normals[i] != normals[ii])	
				{
					// Smooth edges if angle < sharpEdgeAngleThreshold
					if (Mathf.Abs(Vector3.Angle(normals[i], normals[ii])) < sharpEdgeAngleThreshold)
					{
						doubles.Add(ii);
					}
				}
			}
			if (doubles.Count > 0)
			{
				// Second average first vertex
				int[] dv = doubles.ToArray();
				for (int di=0; di<dv.Length; di++)
					normals[i] = (normals[i] + normals[dv[di]]) / 2f;
				// Third: set all other vertex normals to first vertex normal
				for (int di=0; di<dv.Length; di++)
					normals[dv[di]] = normals[i];
			}
		}
	}*/
		// Checks overlapping vertices and averages normals from startIndex to endIndex (including)
	// weldVertices ? Find overlapping vertices, set triangles to first vertex and eliminate the other vertices from vertices and normals
	
	static public List<int[]> SmoothNormals(float sharpEdgeAngleThreshold, List<Vector3> vertices, List<Vector3> normals)//, bool weldVertices, List<int> faces)
	{
		// Find all overlapping vertices
		Vector3[] vs = vertices.ToArray();
		List<int> doubles = new List<int>();
		List<int[]> result = new List<int[]>();
		for (int i=0; i<vs.Length-1; i++)
		{			
			// First find all overlapping vertices
			doubles.Clear();
			for(int ii=i+1; ii<vs.Length;ii++)
			{
				// Check if vertices overlap and normals differ (if normals overlap, we already processed this one)
				if (vs[ii] == vs[i]) //Vector3.Distance(vs[ii], vs[i]) < 0.1f)// && normals[i] != normals[ii])	
				{
					// Smooth edges if angle < sharpEdgeAngleThreshold
					if (Mathf.Abs(Vector3.Angle(normals[i], normals[ii])) < sharpEdgeAngleThreshold)
					{
						doubles.Add(ii);
					}
				}
			}
			if (doubles.Count > 0)
			{
				// Second average first vertex
				int[] dv = doubles.ToArray();
				for (int di=0; di<dv.Length; di++)
					normals[i] = normals[i] + normals[dv[di]];
				normals[i] /= (float)dv.Length+1f;
				
				// Third: set all other vertex normals to first vertex normal
				for (int di=0; di<dv.Length; di++)
					normals[dv[di]] = normals[i];
				
				// Now insert the original as the first item for the optimizer to use
				doubles.Insert(0, i);
				dv = doubles.ToArray();
				result.Add(dv);
			}
		}
		return result;
	}

    /// <summary>
    /// Merges double vertices to first vertex when within given distance.
    /// </summary>
    /// <param name="distanceToMerge">Maximum distance from vertex to merge other vertices</param>
    /// <param name="vertices">List with vertices</param>
    /// <param name="normals">List with normals</param>
    /// <param name="faces">List with vertex face indices</param>
    /// <returns></returns>
    static public int RemoveDoubles(float distanceToMerge, List<Vector3> vertices, List<Vector3> normals, List<int> faces, List<int> centersA = null, List<int> centersB = null, List<int> facesA = null, List<int> facesB = null)
    {
        int cnt = 0;
        List<Vector3> vs = vertices;
        List<int[]> result = new List<int[]>();

        for (int i = 0; i < vs.Count-1; i++)
        {
            // First find all overlapping vertices
            for (int ii = vs.Count-1; ii > i; ii--)
            {

                // Check if vertices are within distance
                if (Vector3.Distance(vs[ii], vs[i]) <= distanceToMerge && normals[i] == normals[ii] && ((centersA == null && centersB == null)  || (centersA.Contains(i) || centersB.Contains(i))))
                {
                    bool hasIndexMask = (centersA != null || centersB != null);
                    bool proceed = true;

                    // Check if there's an indexmask (used to optimize center only)
                    if (hasIndexMask)
                        proceed = (centersA.Contains(i) || centersB.Contains(i)) && (centersA.Contains(ii) || centersB.Contains(ii));

                    if (proceed)
                    { 
                        // Merge normals
                        //normals[i] = (normals[ii] + normals[i]) / 2f;

                        // Merge faces: Set vertex index to first vertex
                        for (int fi = 0; fi < faces.Count; fi++)
                        {
                            if (faces[fi] == ii)
                                faces[fi] = i;
                            if (faces[fi] > ii)
                                faces[fi]--;
                        }

                        if (facesA != null)
                        {
                            for (int fi = 0; fi < facesA.Count; fi++)
                            {
                                if (facesA[fi] == ii)
                                    facesA[fi] = i;
                                if (facesA[fi] > ii)
                                    facesA[fi]--;
                            }
                        }

                        if (facesB != null)
                        {
                            for (int fi = 0; fi < facesB.Count; fi++)
                            {
                                if (facesB[fi] == ii)
                                    facesB[fi] = i;
                                if (facesB[fi] > ii)
                                    facesB[fi]--;
                            }
                        }

                        if (centersA != null)
                        {
                            for (int im = centersA.Count - 1; im >= 0; im--)
                            {
                                if (centersA[im] == ii)
                                    centersA.RemoveAt(im);
                                else
                                    if (centersA[im] > ii)
                                    centersA[im]--;
                            }
                        }

                        if (centersB != null)
                        {
                            for (int im = centersB.Count - 1; im >= 0; im--)
                            {
                                if (centersB[im] == ii)
                                    centersB.RemoveAt(im);
                                else
                                    if (centersB[im] > ii)
                                    centersB[im]--;
                            }
                        }

                        vs.RemoveAt(ii);
                        normals.RemoveAt(ii);
                        cnt++;
                    }
                }
            }
        }
        return cnt;
    }
}
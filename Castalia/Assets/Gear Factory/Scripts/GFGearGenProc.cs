using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GFGearGenTextureCoordinates {None, Plane, Box} 
 
/// <summary>
/// Core engine used by GFGearGen to generate a mesh.
/// </summary>
public class GFGearGenProc  
{
	public Mesh mesh;
	
	private GFGearGen gearGen; 
	
	public List<Vector3> vertices;
	public List<int> faces;
	public List<Vector3> normals;
	private List<Vector3> normalsA;
	private List<Vector3> normalsB;
	private	List<Vector3> verticesSideA;
	private List<int> facesSideA;
	private	List<Vector3> verticesSideB;
	private List<int> facesSideB;
	private List<int> centersA;
	private List<int> centersB;
	
	public int numberOfVertices{get; private set;} 
	public int numberOfFaces{get;private set;} 

    public int verticesRemoved { get; private set; }


    public GFGearGenProc()
	{
		vertices = new List<Vector3>();
		faces = new List<int>();
		normals = new List<Vector3>();
		normalsA = new List<Vector3>();
		normalsB = new List<Vector3>();
		
		verticesSideA = new List<Vector3>();
		facesSideA = new List<int>();
		verticesSideB = new List<Vector3>();
		facesSideB = new List<int>();
		centersA = new List<int>();
		centersB = new List<int>();

	}
	
	/*
	private Vector2 WrapUV(Vector2 uv)
	{
		if (uv.x < 0.0f)
			uv.x = 1.0f - Mathf.Abs(uv.x);
		if (uv.y < 0.0f)
			uv.y = 1.0f - Mathf.Abs(uv.y);
		//if (uv.y < 0.0f)
		//	uv.y = uv.y + 1.0f;
		/*
		while (uv.x < 0.0f)
			uv.x = 1.0f + uv.x;
		while (uv.y < 0.0f)
			uv.y = 1.0f + uv.y;
		while (uv.x > 1.0f)
			uv.x = 1.0f - uv.x;
		while (uv.y > 1.0f)
			uv.y = 1.0f - uv.y;
		* /
		//uv.x = Mathf.Clamp01(uv.x);
		//uv.y = Mathf.Clamp01(uv.y);
		return uv;
	}
	*/
	
	private Vector2[] GetUVs(Vector3[] vertices, GFGearGenTextureCoordinates textureCoordinates)
	{
		List<Vector2> uvs = new List<Vector2>();
		
		#region Get highest and lowest x, y, z
		Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		
		foreach (Vector3 vertex in vertices)
		{
			max.x = Mathf.Max(vertex.x, max.x); 	
			max.y = Mathf.Max(vertex.y, max.y); 	
			max.z = Mathf.Max(vertex.z, max.z); 	
			min.x = Mathf.Min(vertex.x, min.x); 	
			min.y = Mathf.Min(vertex.y, min.y); 	
			min.z = Mathf.Min(vertex.z, min.z); 	
		}
		#endregion
		
		// Shift coordinates to positive domain
		max.x += -min.x; 
		max.y += -min.y; 
		max.z += -min.z; 
		
		Vector2 uv;
		for (int i=0; i<vertices.Length; i+=1)
		{	
			Vector3 vertex = vertices[i];  
			
			if (gearGen.generateTextureCoordinates == GFGearGenTextureCoordinates.Plane || gearGen.generateTextureCoordinates == GFGearGenTextureCoordinates.Box)
			{
				// Get U and V component by mapping them based on position
				Vector2 v;
				const float angleThreshold = 44f / Mathf.Rad2Deg;
				// Normal of vertex < 45 degrees angle to xy plane, take xz plane instead
				if (gearGen.generateTextureCoordinates == GFGearGenTextureCoordinates.Box)
				{
					// Top or bottom UV = XZ
					if (Mathf.Abs(normals[i].AngleVectorPlane(Vector3.up)) >= angleThreshold)// || Mathf.Abs(normals[i].AngleVectorPlane(Vector3.down) * Mathf.Rad2Deg) <= 45f)
					{
						v = new Vector2(vertex.x, vertex.z);
						v.x += -min.x;				// Shift coordinates to positive domain
						v.y += -min.z;
						uv = new Vector2(gearGen.uvOffset.x + ((v.x / max.x) * gearGen.uvTiling.x), gearGen.uvOffset.z + ((v.y / max.z) * gearGen.uvTiling.z));
					} 
					else
					{
						// Left or right side UV = ZY
						if (Mathf.Abs(normals[i].AngleVectorPlane(Vector3.left)) >= angleThreshold)// || Mathf.Abs(normals[i].AngleVectorPlane(Vector3.right) * Mathf.Rad2Deg) <= 45f)
						{
							v = new Vector2(vertex.z, vertex.y);
							v.x += -min.z;
							v.y += -min.y;
							uv = new Vector2(gearGen.uvOffset.z + ((v.x / max.z) * gearGen.uvTiling.z), gearGen.uvOffset.y + ((v.y / max.y) * gearGen.uvTiling.y));
						}
						else
						{
							// Front or back side UV = XY
							v = new Vector2(vertex.x, vertex.y);
							v.x += -min.x;
							v.y += -min.y;
							uv = new Vector2(gearGen.uvOffset.x + ((v.x / max.x) * gearGen.uvTiling.x), gearGen.uvOffset.y + ((v.y / max.y) * gearGen.uvTiling.y));
						}
					}
				}
				else 
				{
					v = new Vector2(vertex.x, vertex.y);
					v.x += -min.x;
					v.y += -min.y;
					uv = new Vector2(gearGen.uvOffset.x + ((v.x / max.x) * gearGen.uvTiling.x), gearGen.uvOffset.y + ((v.y / max.y) * gearGen.uvTiling.y));
				}
				
				//if (uv.x > 1.0f || uv.x < 0.0f || uv.y > 1.0f || uv.y < 0.0f)
				//	Debug.Log("uv out of bounds:"+uv.ToString()+" in "+this.gearGen.gameObject.name);
					
				uvs.Add(uv);
				
			}
			else
				return null;
		} 
		return uvs.ToArray();
	}
	
	private void AddVertex(ref List<Vector3> vertices, ref List<Vector3> normals, Vector3 vertex, Vector3 normal)
	{
		vertices.Add(vertex);
		
		//if (gearGen.generateNormals)
			normals.Add(normal);	
	}
	
	private void CreateSide(ref List<Vector3> vertices, ref List<int> faces, ref List<Vector3> normals, ref List<int> centerIndices, bool isBack, int startFacesIndexAt)
	{
		Vector3 normal;	
		//if (gearGen.innerTeeth)
			//normal = isBack ? Vector3.back : Vector3.forward;
		//else
		 	normal = isBack ? Vector3.forward : Vector3.back;
		
		float tipAngleOffset = gearGen.is3d && !isBack ? gearGen.tipAngleOffset : 0f;
		float valleyAngleOffset = gearGen.is3d && !isBack ? gearGen.valleyAngleOffset : 0f;
		
		float z = gearGen.is3d ? (isBack ? 1.0f : -1.0f) * (gearGen.thickness / 2.0f) : 0.0f;
		Vector3 center = new Vector3(0.0f, 0.0f, z);
		float numberOfSlices = gearGen.numberOfTeeth * 4.0f;
		float angle = (-360.0f / numberOfSlices);
		float angleOffset = (angle+gearGen.tipSize) / 2.0f; // Make sure the middle of the first tip is on 0 deg for easy alignment
		//Debug.Log("angleOffset"+angleOffset);
		// Add center point
		//List<int> centerIndices = new List<int>();
		
		// when we have an open center, we must create multiple center points
		if (gearGen.innerRadius > 0.0f)
		{
			//AddVertex(ref vertices, ref normals, center, normal);
			//centerIndices.Add(0 + startFacesIndexAt);
			
			Vector3 centerPoint = new Vector3(center.x, center.y, center.z);
			centerPoint.x += gearGen.innerRadius; 
			for (int sliceNr = 0; sliceNr < numberOfSlices; sliceNr++)
			{ 				
				float newAngle = (angle * sliceNr) - angleOffset;

				
				
				if (isBack && gearGen.twistOutside)
				{
					newAngle += gearGen.twistAngle;
				}
				
				Vector3 rotated = Quaternion.Euler(0f, 0f, newAngle) * centerPoint;

			
				AddVertex(ref vertices, ref normals, rotated, normal);
				centerIndices.Add(sliceNr + startFacesIndexAt);
				 
			}
			//DebugUtils.Draw(Color.red, false, this.gameObject.transform.localToWorldMatrix, test.ToArray());
		}
		else
		{
			AddVertex(ref vertices, ref normals, center, normal);
			centerIndices.Add(0 + startFacesIndexAt);
		}
		
		//normal = Quaternion.Euler(-45.0f,0.0f,0.0f) * normal;
		//normal.Normalize();
		
		int firstVertexIndex = centerIndices.Count + startFacesIndexAt;
		
		int currentIdx = firstVertexIndex;
		bool isTip = true;
		int tipCnt = 0; 
		for (int sliceNr = 0; sliceNr < numberOfSlices; sliceNr++)
		{
			if (tipCnt == 2)
			{
				isTip = !isTip;
				tipCnt = 0;
			}
			
			// Calculate new vertex: angle based on sliceNr
			Vector3 newVector = new Vector3(isTip ? gearGen.radius - tipAngleOffset : gearGen.radius - gearGen.tipLength - valleyAngleOffset, 0.0f, z);
			float newAngle = (angle * sliceNr) - angleOffset;
			
			// Deform angle
			if (isTip)
			{
				if (tipCnt == 0)
					newAngle += gearGen.tipSize;
				if (tipCnt == 1)
					newAngle -= gearGen.tipSize;				

				newAngle += gearGen.skew;
			}
			else
			{
				if (tipCnt == 0)
					newAngle -= gearGen.valleySize;				
				if (tipCnt == 1)
					newAngle += gearGen.valleySize;

				newAngle -= gearGen.skew;
			}
			
			if (isBack)
			{
				newAngle += gearGen.twistAngle;
			}
			
			Vector3 rotated = Quaternion.Euler(0f, 0f, newAngle) * newVector;
			
			#region Normals
			// Calculate normal based on distance to center
			//float normalAngle = (isTip ? 1.0f : ((gearGen.radius - gearGen.tipLength) / gearGen.radius)) * 45.0f; 
			//Vector3 calculatedNormal = Quaternion.Euler(0f, 0f, newAngle) * Quaternion.Euler(0.0f, (isBack ? 1.0f : -1.0f) * normalAngle, 0.0f) * normal;
			Vector3 calculatedNormal;
		//	if (gearGen.innerTeeth)
//				calculatedNormal = isBack ? Vector3.back : Vector3.forward;
			//else
				calculatedNormal = isBack ? Vector3.forward : Vector3.back;
			calculatedNormal.Normalize();
			#endregion
			
			// Add vertex
			AddVertex(ref vertices, ref normals, rotated, calculatedNormal);

			// Draw triangle from center, current, next or vice versa when back facing (better to do this like this instead of flipping the sign of the angle, because this way all faces align for stitching later on)
			if ((isBack && !gearGen.innerTeeth) || (!isBack && gearGen.innerTeeth))
			{
				// Last vertex ? connect back to first vertex at index 1 (0 == center, 1 == first)
				faces.Add(sliceNr == numberOfSlices-1 ? firstVertexIndex : currentIdx+1); 
				faces.Add(currentIdx); 
				faces.Add(centerIndices[(gearGen.innerRadius <= 0.0f || sliceNr == numberOfSlices-1) ? 0 : sliceNr]);
				 
				if (gearGen.innerRadius > 0.0f)
				{
					faces.Add(centerIndices[sliceNr]);
					faces.Add(centerIndices[sliceNr == numberOfSlices-1 ? 0 : sliceNr+1]);
					faces.Add(sliceNr == numberOfSlices-1 ? currentIdx : currentIdx+1); 
				}
			}
			else 
			{
				faces.Add(centerIndices[(gearGen.innerRadius <= 0.0f || sliceNr == numberOfSlices-1) ? 0 : sliceNr]);
				faces.Add(currentIdx);
				// Last vertex ? connect back to first vertex at index 1 (0 == center, 1 == first)
				faces.Add(sliceNr == numberOfSlices-1 ? firstVertexIndex : currentIdx+1); 
				
				if (gearGen.innerRadius > 0.0f)
				{
					faces.Add(sliceNr == numberOfSlices-1 ? currentIdx : currentIdx+1); 
					faces.Add(centerIndices[sliceNr == numberOfSlices-1 ? 0 : sliceNr+1]);
					faces.Add(centerIndices[sliceNr]);
				}
				
			}
			
			currentIdx++;
			tipCnt++; 		
			 

		}
//		DebugUtils.Draw(Color.yellow, false, this.transform.localToWorldMatrix, vertices[faces[0]], vertices[faces[1]]);//,vertices[faces[2]]);
	 

	}
	
	/// <summary>
	/// Creates faces between facesA and facesB.
	/// </summary>
	/// <returns>
	/// The glue.
	/// </returns>
	/// <param name='facesA'>
	/// Faces a.
	/// </param>
	/// <param name='facesB'>
	/// Faces b.
	/// </param>
	private void CreateGlue(List<int> facesA, List<int> centerIndicesA, List<int> facesB, List<int> centerIndicesB, ref List<Vector3> vertices, ref List<int> faces, ref List<Vector3> normals)
	{   
		if (facesA.Count == facesB.Count)
		{
			bool isLast;
			bool isFirst;
			if ((!gearGen.innerTeeth && gearGen.fillOutside) || (gearGen.innerTeeth && gearGen.fillCenter))
			{
				for (int i=0; i<facesA.Count;i++)//facesA.Count; i++)
				{
					isLast = i == facesA.Count-1;
					isFirst = i == 0;
					
					if (!centerIndicesA.Contains(facesA[i]) && !centerIndicesB.Contains(facesB[i]))
					{
						// triangle: 2x A -> 1x B
						// Copy vertices
						// With innerteeth, inside becomes outside
						if (gearGen.innerTeeth)
						{
							vertices.Add(vertices[facesB[i]]);
							vertices.Add(vertices[facesA[i]]);
							vertices.Add(vertices[facesB[isLast ? 1: i+1]]);
						}
						else
						{
							vertices.Add(vertices[facesA[i]]);
							vertices.Add(vertices[facesB[i]]);
							vertices.Add(vertices[facesA[isLast ? 1: i+1]]);
						}
						
						// Add new face
						faces.Add(vertices.Count-3);
						faces.Add(vertices.Count-2);
						faces.Add(vertices.Count-1);
						
						// Add normals
						Vector3 normal =  MeshUtils.Normal(vertices[vertices.Count-3], vertices[vertices.Count-2], vertices[vertices.Count-1]);
						normals.Add(normal);
						normals.Add(normal);
						normals.Add(normal);
							
						// triangle: 2x B -> 1x A
						// Copy vertices
						if (gearGen.innerTeeth)
						{
							vertices.Add(vertices[facesA[i]]);
							vertices.Add(vertices[facesA[isFirst ? facesA.Count-1 : i-1]]);
							vertices.Add(vertices[facesB[isLast ? 1: i+1]]);
						}
						else
						{
							vertices.Add(vertices[facesB[i]]);
							vertices.Add(vertices[facesB[isFirst ? facesA.Count-1 : i-1]]);
							vertices.Add(vertices[facesA[isLast ? 1: i+1]]);
						}
						// Add new face
						faces.Add(vertices.Count-3);
						faces.Add(vertices.Count-2);
						faces.Add(vertices.Count-1);
						// Add normals
						normal =  MeshUtils.Normal(vertices[vertices.Count-3], vertices[vertices.Count-2], vertices[vertices.Count-1]);
						normals.Add(normal);
						normals.Add(normal);
						normals.Add(normal);
						
					} 
				}
			}

			if ((!gearGen.innerTeeth && gearGen.fillCenter && gearGen.innerRadius > 0.0f) || 
				(gearGen.innerTeeth && gearGen.fillOutside))
			{
				for (int i=0; i<centerIndicesA.Count;i++)//facesA.Count; i++)
				{
					isLast = i == centerIndicesA.Count-1;
					// triangle: 2x A -> 1x B
					/*
					faces.Add(centerIndicesA[i]);
					faces.Add(centerIndicesA[isLast ? 0: i+1]);
					faces.Add(centerIndicesB[i]);
					*/
					// Copy vertices
					if (gearGen.innerTeeth)
					{
						vertices.Add(vertices[centerIndicesB[i]]);
						vertices.Add(vertices[centerIndicesB[isLast ? 0: i+1]]);
						vertices.Add(vertices[centerIndicesA[i]]);
					}
					else
					{
						vertices.Add(vertices[centerIndicesA[i]]);
						vertices.Add(vertices[centerIndicesA[isLast ? 0: i+1]]);
						vertices.Add(vertices[centerIndicesB[i]]);
					}
					
					// Add new face
					faces.Add(vertices.Count-3);
					faces.Add(vertices.Count-2);
					faces.Add(vertices.Count-1);
                     
                    // Add normals
                    Vector3 normal =  MeshUtils.Normal(vertices[vertices.Count-3], vertices[vertices.Count-2], vertices[vertices.Count-1]);
					normals.Add(normal);
					normals.Add(normal);
					normals.Add(normal);

					
					// triangle: 2x B -> 1x A
					/*
					faces.Add(centerIndicesB[isLast ? 0: i+1]);
					faces.Add(centerIndicesB[i]);
					faces.Add(centerIndicesA[isLast ? 0: i+1]);
					*/
					// Copy vertices
					if (gearGen.innerTeeth)
					{
						vertices.Add(vertices[centerIndicesA[isLast ? 0: i+1]]);
						vertices.Add(vertices[centerIndicesA[i]]);
						vertices.Add(vertices[centerIndicesB[isLast ? 0: i+1]]);
					}
					else
					{
						vertices.Add(vertices[centerIndicesB[isLast ? 0: i+1]]);
						vertices.Add(vertices[centerIndicesB[i]]);
						vertices.Add(vertices[centerIndicesA[isLast ? 0: i+1]]);
					}
					// Add new face
					faces.Add(vertices.Count-3);
					faces.Add(vertices.Count-2);
					faces.Add(vertices.Count-1);
                     
                    // Add normals
                    normal =  MeshUtils.Normal(vertices[vertices.Count-3], vertices[vertices.Count-2], vertices[vertices.Count-1]);
					normals.Add(normal);
					normals.Add(normal);
					normals.Add(normal);
					
				}
			}
		} 	
	}
	
	
	public void GenerateGear(GFGearGen gearGen){
		this.gearGen = gearGen;
		GameObject gameObject = gearGen.gameObject;
		
		mesh = gameObject.GetSharedMesh();

		if (mesh != null)
		{
			mesh.Clear();
			/*
			//speed up math by copying the mesh arrays
	    	meshTriangles = mesh.triangles;
	    	meshVertices = mesh.vertices;
	    	meshUv = mesh.uv;
	    	meshNormals = mesh.normals;
			*/
			// Clean up (storing these temp lists globally and just clearing them localy prevents the GC to be all nasty on our performance.
			verticesSideA.Clear();
			verticesSideB.Clear();
			facesSideA.Clear();
			facesSideB.Clear();
			vertices.Clear();
			faces.Clear();
			normals.Clear();
			normalsA.Clear();
			normalsB.Clear();
			centersA.Clear();
			centersB.Clear();
			
			// Create vertices
			CreateSide(ref verticesSideA, ref facesSideA, ref normalsA, ref centersA, false, 0);
			if (gearGen.is3d)
				CreateSide(ref verticesSideB, ref facesSideB, ref normalsB, ref centersB, true, verticesSideA.Count);
			
			vertices.AddRange(verticesSideA);
			normals.AddRange(normalsA);
			
			if (gearGen.is3d)
			{
				vertices.AddRange(verticesSideB);
				normals.AddRange(normalsB);
			}

            // Create faces
            faces.AddRange(facesSideA);

            if (gearGen.is3d)
                faces.AddRange(facesSideB);

            // Optimize center vertices when inner radius > threshold
            if (gearGen.innerRadius > 0.0f)
            {
                verticesRemoved = MeshUtils.RemoveDoubles(gearGen.innerMinVertexDistance, vertices, normals, faces, centersA, centersB, facesSideA, facesSideB);
            }
            else
                verticesRemoved = 0;

            if (gearGen.is3d)
                CreateGlue(facesSideA, centersA, facesSideB, centersB, ref vertices, ref faces, ref normals);

            MeshUtils.SmoothNormals(gearGen.splitVerticesAngle, vertices, normals);

            verticesRemoved += MeshUtils.RemoveDoubles(0.001f, vertices, normals, faces);

            // Fill the mesh
            mesh.vertices = vertices.ToArray();
	        mesh.uv = GetUVs(mesh.vertices, gearGen.generateTextureCoordinates);
	        mesh.triangles = faces.ToArray(); 
			// Calculate normals
			/*
			if (!gearGen.generateNormals)
			{
				mesh.RecalculateNormals(); 
				normals.AddRange(mesh.normals);				
			}
			else 
			{ */
//				normals.AddRange(normalsA);
//				normals.AddRange(normalsB);
				mesh.normals = normals.ToArray();
			//}
			
			 		
			// Refresh internal parameters
			MeshUtils.TangentSolver(mesh);
			mesh.RecalculateBounds();
			 
			
			numberOfVertices = mesh.vertexCount;
			numberOfFaces = mesh.triangles.Length / 3;
            
		}
        

	}
	
}

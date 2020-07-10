using UnityEngine;
using System.Collections;

public static class MathUtils {

	
	#region 3d Math functions: optimized. Based on elecman's (http://forum.unity3d.com/threads/126575-3d-Math-functions)
	/// <summary>
	/// Increase or decrease the length of vector by size
	/// </summary>
	/// <returns>
	/// The vector.
	/// </returns>
	/// <param name='vector'>
	/// Vector.
	/// </param>
	/// <param name='size'>
	/// Size.
	/// </param>
	static public Vector3 AddVectorLength(this Vector3 vector, float size){
		
		//get the vector length
		float magnitude = Vector3.Magnitude(vector);
		
		//change the length
		magnitude += size;
		
		//normalize the vector
		Vector3 vectorNormalized = Vector3.Normalize(vector);
		
		//scale the vector
		return Vector3.Scale(vectorNormalized, new Vector3(magnitude, magnitude, magnitude));		
	}
	
	
	/// <summary>
	/// Create a vector of direction "vector" with length "size"
	/// </summary>
	/// <returns>
	/// The vector length.
	/// </returns>
	/// <param name='vector'>
	/// Vector.
	/// </param>
	/// <param name='size'>
	/// Size.
	/// </param>
	static public Vector3 SetVectorLength(this Vector3 vector, float size){
		
		//normalize the vector
		Vector3 vectorNormalized = Vector3.Normalize(vector);
		
		//scale the vector
	//	return Vector3.Scale(vectorNormalized, new Vector3(size, size, size));	
		return vectorNormalized *= size;
	}	
	
	/// <summary>
	/// Calculate the rotational difference from A to B
	/// </summary>
	/// <returns>
	/// The rotation.
	/// </returns>
	/// <param name='B'>
	/// B.
	/// </param>
	/// <param name='A'>
	/// A.
	/// </param>
	static public Quaternion SubtractRotation(this Quaternion B, Quaternion A){		
		Quaternion C = Quaternion.Inverse(A) * B;		
		return C;
	}

	/// <summary>
	/// Find the line of intersection between two planes.
	/// The outputs are a point on the line and a vector which indicates it's direction.
	/// </summary>
	/// <param name='linePoint'>
	/// Line point.
	/// </param>
	/// <param name='lineVec'>
	/// Line vec.
	/// </param>
	/// <param name='p1'>
	/// P1.
	/// </param>
	/// <param name='p2'>
	/// P2.
	/// </param>
	/// <param name='plane1Position'>
	/// Plane1 position.
	/// </param>
	/// <param name='plane2Position'>
	/// Plane2 position.
	/// </param>
	static public void PlanePlaneIntersection(out Vector3 linePoint, out Vector3 lineVec, Plane p1, Plane p2, Vector3 plane1Position, Vector3 plane2Position){
		Vector3 plane1Normal = p1.normal;
		Vector3 plane2Normal = p2.normal;
			
		linePoint = Vector3.zero;
		lineVec = Vector3.zero;
		
		//We can get the direction of the line of intersection of the two planes by calculating the 
		//cross product of the normals of the two planes. Note that this is just a direction and the line
		//is not fixed in space yet. We need a point for that to go with the line vector.
		lineVec = Vector3.Cross(plane1Normal, plane2Normal);
		
		//Next is to calculate a point on the line to fix it's position in space. This is done by finding a vector from
		//the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
		//errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
		//the cross product of the normal of plane2 and the lineDirection.		
		Vector3 ldir = Vector3.Cross(plane2Normal, lineVec);		
		
		float denominator = Vector3.Dot(plane1Normal, ldir);
		
		//Prevent divide by zero and rounding errors by requiring about 5 degrees angle between the planes.
		if(Mathf.Abs(denominator) > 0.006f){
			
			Vector3 plane1ToPlane2 = plane1Position - plane2Position;
			float t = Vector3.Dot(plane1Normal, plane1ToPlane2) / denominator;
			linePoint = plane2Position + t * ldir;
		}
	}	

	//Get the coordinates (world space) of the intersection between a line and a plane
	static public Vector3 LinePlaneIntersection(Vector3 linePoint, Vector3 lineVec, Vector3 planeNormal, Vector3 planePoint){
		
		float length;
		float dotNumerator;
		float dotDenominator;
		Vector3 vector;
		
		//calculate the distance between the linePoint and the line-plane intersection point
		dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
		dotDenominator = Vector3.Dot(lineVec, planeNormal);
		
		//line and plane are not parallel
		if(dotDenominator != 0.0f){
			length =  dotNumerator / dotDenominator;
			
			//create a vector from the linePoint to the intersection point
			vector = lineVec.SetVectorLength(length);
			
			//get the coordinates of the line-plane intersection point
			return linePoint + vector;	
		}
		
		else{
			return Vector3.zero;
		}
		
	}

	//Two non-parallel lines which may or may not touch each other have a point on each line which lays closest
	//to each other. This function finds those two points.
	static public void ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
	
		closestPointLine1 = Vector3.zero;
		closestPointLine2 = Vector3.zero;
		
		float a = Vector3.Dot(lineVec1, lineVec1);
		float b = Vector3.Dot(lineVec1, lineVec2);
		float e = Vector3.Dot(lineVec2, lineVec2);
		
		float d = a*e - b*b;
		
		//lines are not parallel
		if(d != 0.0f){
			
			Vector3 r = linePoint1 - linePoint2;
			float c = Vector3.Dot(lineVec1, r);
			float f = Vector3.Dot(lineVec2, r);
			
			float s = (b*f - c*e) / d;
			float t = (a*f - c*b) / d;
			
			closestPointLine1 = linePoint1 + lineVec1 * s;
			closestPointLine2 = linePoint2 + lineVec2 * t;
		}
		
		else{
			closestPointLine1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			closestPointLine2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		}
	}	

	//This function returns a 3d point in space which is a projection from point "point" to a line, consisting
	//of a vector (lineVec) and a point on that line (linePoint).
	static public Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point){		
		
		//get vector from point on line to point in space
		Vector3 linePointToPoint = point - linePoint;
	
		float t = Vector3.Dot(linePointToPoint, lineVec);
		
		return linePoint + lineVec * t;
	}	
	
	static public Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point){
		
		float distance;
		Vector3 translationVector;
		
		//First calculate the distance from the point to the plane:
		distance = SignedDistancePlanePoint(planeNormal, planePoint, point);
		
		//Reverse the sign of the distance
		distance *= -1;
		
		//Get a translation vector
		translationVector = planeNormal.SetVectorLength(distance);
		
		//Translate the point to form a projection
		return point + translationVector;
	}

	//output is not normalized
	static public Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector){
		
		return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
	}	
	
	//Get the shortest distance between a point and a plane
	static public float SignedDistancePlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 point){
		
		return Vector3.Dot(planeNormal, (point - planePoint));
	}	

	//This function calculates a signed (+ or - sign instead of being ambiguous) dot product. It is basically used
	//to figure out whether a vector is positioned to the left or right of another vector. The way this is done is
	//by calculating a vector perpendicular to one of the vectors and using that as a reference. This is because
	//the result of a dot product only has signed information when an angle is transitioning between more or less
	//then 90 degrees.
	static public float SignedDotProduct(Vector3 vectorA, Vector3 vectorB, Vector3 normal){
		Vector3 perpVector;
		float dot;
		
		//Use the geometry object normal and one of the input vectors to calculate the perpendicular vector
		perpVector = Vector3.Cross(normal, vectorA);
		
		//Now calculate the dot product between the perpendicular vector (perpVector) and the other input vector
		dot = Vector3.Dot(perpVector, vectorB);
		
		return dot;
	}

	//Calculate the angle between a vector and a plane. The plane is made by a normal vector.
	//Output is in radians.
	static public float AngleVectorPlane(this Vector3 vector, Vector3 normal){
		
		float dot;
		float angle;
		
		//calculate the the dot product between the two input vectors. This gives the cosine between the two vectors
		dot = Vector3.Dot(vector, normal);
		
		//this is in radians
		angle = (float)Mathf.Acos(dot);
		
		return 1.570796326794897f - angle; //90
	}
	
	//Calculate the dot product as an angle
	static public float DotProductAngle(this Vector3 vec1, Vector3 vec2){
		
		float dot;
		float angle;
		
		//get the dot product
		dot = Vector3.Dot(vec1, vec2);
		
		//Clamp to prevent NaN error. Shouldn't need this in the first place, but there could be a rounding error issue.
		if(dot < -1.0f){
			dot = -1.0f;
		}							
		if(dot > 1.0f){
			dot =1.0f;
		}
		
		//Calculate the angle. The output is in radians
		//This step can be skipped for optimization...
		angle = Mathf.Acos(dot);
	
		return (float)angle;
	}
	#endregion 
}

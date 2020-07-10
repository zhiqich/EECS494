using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class DebugUtils {
	static public int numberOfSeconds = 3;
	
	static public void Draw(Color c1, params Vector3[] vectors)
	{
		Draw(c1, false, vectors);	
	}
	
	static public void Draw(Color c1, bool depthTest, params Vector3[] vectors)
	{
		Draw(c1, depthTest, new Matrix4x4(), vectors);	
	}
	
	static public void Draw(Color c1, bool depthTest, Matrix4x4 matrix, params Vector3[] vectors)
	{
		if (vectors.Length == 1)
		{
			Vector3 vxFrom = matrix.MultiplyPoint(new Vector3(vectors[0].x, vectors[0].y, vectors[0].z));
			Vector3 vxTo = matrix.MultiplyPoint(new Vector3(vectors[0].x+1.0f, vectors[0].y, vectors[0].z));
			Vector3 vyFrom = matrix.MultiplyPoint(new Vector3(vectors[0].x, vectors[0].y, vectors[0].z));
			Vector3 vyTo = matrix.MultiplyPoint(new Vector3(vectors[0].x, vectors[0].y+1.0f, vectors[0].z));
			Vector3 vzFrom = matrix.MultiplyPoint(new Vector3(vectors[0].x, vectors[0].y, vectors[0].z));
			Vector3 vzTo = matrix.MultiplyPoint(new Vector3(vectors[0].x, vectors[0].y, vectors[0].z+1.0f));
			Draw(Color.red, depthTest, new Vector3[]{vxFrom, vxTo});
			Draw(Color.green, depthTest, new Vector3[]{vyFrom, vyTo});
			Draw(Color.blue, depthTest, new Vector3[]{vzFrom, vzTo});
		}
		else{
			for (int i=0; i<vectors.Length-1; i++)
			{
				#if UNITY_EDITOR
				try 
				{
					Handles.color = c1;
					Handles.DrawLine(matrix.MultiplyPoint(vectors[i]), matrix.MultiplyPoint(vectors[i+1]));
				}
				catch
				{
					Debug.DrawLine(matrix.MultiplyPoint(vectors[i]), matrix.MultiplyPoint(vectors[i+1]), c1, numberOfSeconds, depthTest);
				}
				#else
					Debug.DrawLine(matrix.MultiplyPoint(vectors[i]), matrix.MultiplyPoint(vectors[i+1]), c1, numberOfSeconds, depthTest);
				#endif
			}
		}
	}
	
	
	static public void DrawBoundingBox(this GameObject g1, Color c1)
	{
		Vector3[] v = g1.CalcOrientedBoundingBox();
		Debug.DrawLine(v[0], v[1], c1, numberOfSeconds, false);
		Debug.DrawLine(v[1], v[2], c1, numberOfSeconds, false);
		Debug.DrawLine(v[2], v[3], c1, numberOfSeconds, false);
		Debug.DrawLine(v[3], v[0], c1, numberOfSeconds, false);
									   
		Debug.DrawLine(v[4], v[5], c1, numberOfSeconds, false);
		Debug.DrawLine(v[5], v[6], c1, numberOfSeconds, false);
		Debug.DrawLine(v[6], v[7], c1, numberOfSeconds, false);
		Debug.DrawLine(v[7], v[4], c1, numberOfSeconds, false);
									   
		Debug.DrawLine(v[0], v[4], c1, numberOfSeconds, false);
		Debug.DrawLine(v[1], v[5], c1, numberOfSeconds, false);
		Debug.DrawLine(v[2], v[6], c1, numberOfSeconds, false);
		Debug.DrawLine(v[3], v[7], c1, numberOfSeconds, false);
	}
	
	static public void DrawDisc(this GameObject gameObject, Color c1, string label)
	{
			#if UNITY_EDITOR
				Handles.color = c1;
				Handles.DrawSolidDisc(gameObject.transform.position, gameObject.CalcYAlignedCenterPlane().normal, 1.0f);
				Handles.Label(gameObject.transform.position + Vector3.up + Vector3.left, label);
			#endif		
	}
	static public void DrawPoweredBy(this GFGear gear1)
	{
		if (gear1.DrivenBy != null)
		{
			#if UNITY_EDITOR
				Handles.color = Color.yellow;
				Quaternion q = new Quaternion();
				Vector3 v = gear1.DrivenBy.transform.position - gear1.transform.position;
				if (v != Vector3.zero)
				{
					q = Quaternion.LookRotation(v);
					Handles.DrawLine(gear1.transform.position, gear1.DrivenBy.transform.position);
					Vector3 conePos = gear1.transform.position + ((gear1.DrivenBy.transform.position - gear1.transform.position) / 2.0f);
					Handles.ConeHandleCap(0, conePos, q, HandleUtility.GetHandleSize(conePos) / 4.0f, EventType.Repaint);
				}
			#else 
				Debug.DrawLine(gear1.transform.position, gear1.DrivenBy.transform.position, Color.yellow, 3, false);
			#endif		
		}
	}

    static public void DrawAlignmentHelpers(this GFGear gear1)
    {
        if (gear1.AutoAlign)
        {
#if UNITY_EDITOR
            Color c = Color.green;
            c.a = 0.2f;
            Handles.color = c;
            Handles.DrawSolidDisc(gear1.transform.position, gear1.gameObject.CalcYAlignedCenterPlane().normal, gear1.radius - (gear1.innerTeeth ? 0f : gear1.tipLength));
            c.a = 1f;
            Handles.color = c;
            Vector3 startP = Vector3.zero;
            Vector3 endP = Vector3.zero;
            for (int i = 0; i < gear1.numberOfTeeth; i++)
            {
                float angleRad = ((2 * Mathf.PI) / gear1.numberOfTeeth) * i;
                endP.x = Mathf.Cos(angleRad);
                endP.y = Mathf.Sin(angleRad);
                endP.z = 0f;

                startP = endP * (gear1.radius - gear1.tipLength); 
                endP = endP * gear1.radius;

                startP = gear1.transform.TransformPoint(startP);
                endP = gear1.transform.TransformPoint(endP);

                Handles.DrawAAPolyLine(10f, new Vector3[] { startP, endP });
            }

            /*
            Quaternion q = new Quaternion();
            Vector3 v = gear1.DrivenBy.transform.position - gear1.transform.position;
            if (v != Vector3.zero)
            {
                q = Quaternion.LookRotation(v);
                Handles.DrawLine(gear1.transform.position, gear1.DrivenBy.transform.position);
                Vector3 conePos = gear1.transform.position + ((gear1.DrivenBy.transform.position - gear1.transform.position) / 2.0f);
                Handles.ConeCap(0, conePos, q, HandleUtility.GetHandleSize(conePos) / 4.0f);
            }
            */
#endif
        }
    }

}

using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	public GameObject leftWheel;
	public GameObject rightWheel;
	public GFMachine differentialGeartrain;
	public GFMachine mainEngine;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float lockForce = 0f;
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       		RaycastHit hit;
 
       		if (Physics.Raycast(ray, out hit, 999))
       		{
            	if (hit.transform.gameObject == leftWheel)
					lockForce = 1f;
				else 
            		if (hit.transform.gameObject == rightWheel)
						lockForce = -1f;
        	}
		}
		
		LockDif(lockForce);
	}
	
	private void LockDif(float normalizedForce)
	{
		differentialGeartrain.Reverse = normalizedForce < 0f;
		differentialGeartrain.speed = mainEngine.speed * Mathf.Abs(normalizedForce);
	}
}

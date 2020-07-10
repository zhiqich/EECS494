using UnityEngine;
using System.Collections;

public class Marble : MonoBehaviour {
	
	private Vector3 startpos;
	// Use this for initialization
	void Start () {
		startpos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y < -33.0f)
			this.transform.position = startpos;
	}
}

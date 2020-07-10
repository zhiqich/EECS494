using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleCam : MonoBehaviour {
    public Transform target;
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = target.position;
        pos.z = transform.position.z;
        transform.position = pos;

    }
}

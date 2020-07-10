using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public GameObject owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (brandishing)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, brandishAngle), brandishSpeed * Time.deltaTime);
            if (transform.localRotation == Quaternion.Euler(0, 0, brandishAngle))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 180f - brandishAngle);
                brandishing = false;
                gameObject.SetActive(false);
            }
        }
    }

    public float brandishAngle = 110f;
    public float brandishSpeed = 10f;

    public bool brandishing = false;

    public void Brandish()
    {
        if (brandishing)
        {
            return;
        }
        brandishing = true;
    }

}

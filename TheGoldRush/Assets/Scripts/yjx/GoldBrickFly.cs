using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBrickFly : MonoBehaviour
{

    public Vector3 target = Vector3.zero;
    //private Vector2 target2d;
    private float startTime;
    //public float speed = 10f;
    public float halfPeriod = 1f;
    private Vector3 initialPos;

    private bool arrived = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == Vector3.zero || arrived)
        {
            return;
        }
        Fly();
    }

    void Fly()
    {
        if (Mathf.Sqrt((transform.position.x - target.x) * (transform.position.x - target.x) + (transform.position.y - target.y) * (transform.position.y - target.y)) <= 0.1f)
        {
            arrived = true;
            transform.position = target;
            GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<TrailRenderer>().enabled = false;
        }
        float z = -3f * Mathf.Sin((Time.time - startTime) * Mathf.PI / halfPeriod);
        Vector3 nextPos = Vector2.Lerp(initialPos, target, (Time.time - startTime) / halfPeriod);
        nextPos.z = z;
        transform.position = nextPos;
    }
}

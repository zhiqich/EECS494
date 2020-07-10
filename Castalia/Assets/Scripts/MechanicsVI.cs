using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicsVI : MonoBehaviour
{
    public GameObject t1;
    public GameObject t2;
    public GameObject t3;
    public GameObject t4;
    public GameObject t5;
    public GameObject t6;
    public GameObject trigger1;
    public GameObject trigger2;
    public GameObject trigger3;
    public GameObject trigger4;
    public GameObject tp1;
    public GameObject tp2;
    public GameObject pl;
    public Vector3 h1;
    public Vector3 h2;
    public Vector3 h3;
    public Vector3 h4;
    public Vector3 h5;
    public Vector3 h6;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;
    public Vector3 p4;
    public Vector3 p5;
    public Vector3 p6;
    public Vector3 d1;
    public Vector3 d2;
    public Vector3 d3;
    public Vector3 d4;
    public Vector3 d5;
    public Vector3 d6;
    public Vector3 tpd;
    public Vector3 tph;
    public Vector3 tpl;
    public Vector3 tpf;
    public float speed = 1.0f;
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active == true && t1.GetComponent<TriangleRotation>().state == 0 && t2.GetComponent<TriangleRotation>().state == 0 && t3.GetComponent<TriangleRotation>().state == 0 && t4.GetComponent<TriangleRotation>().state == 0 && t5.GetComponent<TriangleRotation>().state == 0 && t6.GetComponent<TriangleRotation>().state == 0)
        {
            active = false;
            trigger1.SetActive(false);
            trigger2.SetActive(false);
            trigger3.SetActive(false);
            StartCoroutine(ShapeForming());
        }
    }

    IEnumerator ShapeForming()
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time);
        while (progress < 0.5f)
        {
            progress = (Time.time - initial_time);
            yield return null;
        }
        while (t1.transform.position != h1 || t2.transform.position != h2 || t3.transform.position != h3 || t4.transform.position != h4 || t5.transform.position != h5 || t6.transform.position != h6)
        {
            t1.transform.position = Vector3.MoveTowards(t1.transform.position, h1, speed * Time.deltaTime);
            t2.transform.position = Vector3.MoveTowards(t2.transform.position, h2, speed * Time.deltaTime);
            t3.transform.position = Vector3.MoveTowards(t3.transform.position, h3, speed * Time.deltaTime);
            t4.transform.position = Vector3.MoveTowards(t4.transform.position, h4, speed * Time.deltaTime);
            t5.transform.position = Vector3.MoveTowards(t5.transform.position, h5, speed * Time.deltaTime);
            t6.transform.position = Vector3.MoveTowards(t6.transform.position, h6, speed * Time.deltaTime);
            yield return null;
        }
        initial_time = Time.time;
        progress = (Time.time - initial_time);
        while (progress < 0.5f)
        {
            progress = (Time.time - initial_time);
            yield return null;
        }
        while (t1.transform.position != p1 || t2.transform.position != p2 || t3.transform.position != p3 || t4.transform.position != p4 || t5.transform.position != p5 || t6.transform.position != p6)
        {
            t1.transform.position = Vector3.MoveTowards(t1.transform.position, p1, speed * Time.deltaTime);
            t2.transform.position = Vector3.MoveTowards(t2.transform.position, p2, speed * Time.deltaTime);
            t3.transform.position = Vector3.MoveTowards(t3.transform.position, p3, speed * Time.deltaTime);
            t4.transform.position = Vector3.MoveTowards(t4.transform.position, p4, speed * Time.deltaTime);
            t5.transform.position = Vector3.MoveTowards(t5.transform.position, p5, speed * Time.deltaTime);
            t6.transform.position = Vector3.MoveTowards(t6.transform.position, p6, speed * Time.deltaTime);
            yield return null;
        }
        initial_time = Time.time;
        progress = (Time.time - initial_time);
        while (progress < 0.5f)
        {
            progress = (Time.time - initial_time);
            yield return null;
        }
        while (t1.transform.position != d1 || t2.transform.position != d2 || t3.transform.position != d3 || t4.transform.position != d4 || t5.transform.position != d5 || t6.transform.position != d6)
        {
            t1.transform.position = Vector3.MoveTowards(t1.transform.position, d1, speed * Time.deltaTime);
            t2.transform.position = Vector3.MoveTowards(t2.transform.position, d2, speed * Time.deltaTime);
            t3.transform.position = Vector3.MoveTowards(t3.transform.position, d3, speed * Time.deltaTime);
            t4.transform.position = Vector3.MoveTowards(t4.transform.position, d4, speed * Time.deltaTime);
            t5.transform.position = Vector3.MoveTowards(t5.transform.position, d5, speed * Time.deltaTime);
            t6.transform.position = Vector3.MoveTowards(t6.transform.position, d6, speed * Time.deltaTime);
            yield return null;
        }
        initial_time = Time.time;
        progress = (Time.time - initial_time);
        while (progress < 0.5f)
        {
            progress = (Time.time - initial_time);
            yield return null;
        }
        while (tp1.transform.position != tph || tp2.transform.position != tpd || pl.transform.position != tpl)
        {
            tp1.transform.position = Vector3.MoveTowards(tp1.transform.position, tph, speed * Time.deltaTime);
            tp2.transform.position = Vector3.MoveTowards(tp2.transform.position, tpd, speed * Time.deltaTime);
            pl.transform.position = Vector3.MoveTowards(pl.transform.position, tpl, speed * Time.deltaTime);
            yield return null;
        }
        initial_time = Time.time;
        progress = (Time.time - initial_time);
        while (progress < 0.5f)
        {
            progress = (Time.time - initial_time);
            yield return null;
        }
        while (transform.position != tpf)
        {
            transform.position = Vector3.MoveTowards(transform.position, tpf, speed * Time.deltaTime);
            yield return null;
        }
        trigger4.SetActive(true);
    }
}

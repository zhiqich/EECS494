using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guidance : MonoBehaviour
{
    public Text Castalia;
    public Text Arrow;
    public Text R;
    public Text V;
    public int next = 0;
    public bool trigger = true;
    public float castalia_speed = 3.0f;
    public float arrow_speed = 1.5f;
    public float r_speed = 10.0f;
    public float v_speed = 10.0f;
    public Transform t1;
    public Transform t2;
    public Transform t3;

    private GameMaster gm;


    private void Awake()
    {
        Camera.main.GetComponent<CameraController>().follow_robot = false;
        Camera.main.GetComponent<CameraController>().is_changing = true;
        Castalia.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
        Arrow.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
        R.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
        V.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        if (gm.start_game == false)
        {
            Camera.main.GetComponent<CameraController>().follow_robot = true;
            Camera.main.GetComponent<CameraController>().is_changing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (next <= 3 && gm.start_game == true)
        {
            if (next == 0 && trigger == true)
            {
                trigger = false;
                StartCoroutine(CastaliaDisplay());
            }
            if (next == 1 && trigger == true)
            {
                trigger = false;
                StartCoroutine(VDisplay());
            }
            if (next == 2 && trigger == true)
            {
                trigger = false;
                StartCoroutine(RDisplay());
            }
            if (next == 3 && trigger == true)
            {
                trigger = false;
                StartCoroutine(EndGuidance());
            }
        }
    }

    IEnumerator CastaliaDisplay()
    {
        while (next != 0)
        {
            yield return null;
        }
        float init_time = Time.time;
        float progress = (Time.time - init_time) / castalia_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed;
            Castalia.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, progress);
            yield return null;
        }
        Castalia.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        while (Camera.main.transform.position != t2.position)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, t2.position, Time.deltaTime * castalia_speed);
            Camera.main.transform.LookAt(Camera.main.GetComponent<FollowPlayer>().robot.transform.position);
            yield return null;
        }
        next = 1;
        trigger = true;
    }

    IEnumerator VDisplay()
    {
        while (next != 1)
        {
            yield return null;
        }
        float init_time = Time.time;
        float progress = (Time.time - init_time);
        while (progress < 1.5f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        StartCoroutine(ShowV());
        while (Camera.main.transform.position != t3.position)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, t3.position, Time.deltaTime * v_speed);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time);
        while (progress < 1.5f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        StartCoroutine(ShowV());
        while (Camera.main.transform.position != t2.position)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, t2.position, Time.deltaTime * v_speed);
            yield return null;
        }
        next = 2;
        trigger = true;
    }

    IEnumerator RDisplay()
    {
        while (next != 2)
        {
            yield return null;
        }
        float init_time = Time.time;
        float progress = (Time.time - init_time);
        while (progress < 1.5f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        StartCoroutine(ShowR());
        while (Camera.main.transform.position != t1.position)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, t1.position, Time.deltaTime * r_speed);
            Camera.main.transform.LookAt(Camera.main.GetComponent<FollowPlayer>().robot.transform.position);
            yield return null;
        }
        next = 3;
        trigger = true;
    }

    IEnumerator ShowV()
    {
        float init_time = Time.time;
        float progress = (Time.time - init_time) / castalia_speed * 3.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed * 3.0f;
            V.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, progress);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / castalia_speed * 3.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed * 3.0f;
            V.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f - progress);
            yield return null;
        }
    }

    IEnumerator ShowR()
    {
        float init_time = Time.time;
        float progress = (Time.time - init_time) / castalia_speed * 3.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed * 3.0f;
            R.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, progress);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / castalia_speed * 3.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed * 3.0f;
            R.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f - progress);
            yield return null;
        }
    }

    IEnumerator ShowArrow()
    {
        float init_time = Time.time;
        float progress = (Time.time - init_time);
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / castalia_speed * 3.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed * 3.0f;
            Arrow.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, progress);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / castalia_speed * 3.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed * 3.0f;
            Arrow.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f - progress);
            yield return null;
        }
    }

    IEnumerator EndGuidance()
    {
        while (next != 3)
        {
            yield return null;
        }
        StartCoroutine(ShowArrow());
        float init_time = Time.time;
        float progress = (Time.time - init_time) / castalia_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / castalia_speed;
            Castalia.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f - progress);
            yield return null;
        }
        Castalia.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
        next = 3;
        Camera.main.GetComponent<CameraController>().follow_robot = true;
        Camera.main.GetComponent<CameraController>().is_changing = false;
        gm.start_game = false;
    }
}

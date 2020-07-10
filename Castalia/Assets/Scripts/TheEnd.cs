using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    public Text Hermann;
    public Text End;
    public float end_speed = 3.0f;
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (active == true)
        {
            active = false;
            Camera.main.GetComponent<CameraController>().follow_robot = false;
            Camera.main.GetComponent<CameraController>().is_changing = true;
            StartCoroutine(EndDisplay());
        }
    }

    IEnumerator EndDisplay()
    {
        float init_time = Time.time;
        float progress = (Time.time - init_time);
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / end_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / end_speed;
            Hermann.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, progress);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / end_speed;
        while (progress < 3.0f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / end_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / end_speed;
            Hermann.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f - progress);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / end_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / end_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / end_speed;
            End.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, progress);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / end_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time);
            yield return null;
        }
        init_time = Time.time;
        progress = (Time.time - init_time) / end_speed;
        while (progress < 1.0f)
        {
            progress = (Time.time - init_time) / end_speed;
            End.GetComponent<Text>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f - progress);
            yield return null;
        }
        Camera.main.GetComponent<CameraController>().follow_robot = true;
        Camera.main.GetComponent<CameraController>().is_changing = false;
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().last_checkpoint = Vector3.zero;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerShiftPanel : MonoBehaviour
{

    public GameObject leftPanel;
    public GameObject characterInfoPanel;

    private RectTransform lt;
    private RectTransform ct;

    // Start is called before the first frame update
    void Start()
    {
        lt = leftPanel.GetComponent<RectTransform>();
        ct = characterInfoPanel.GetComponent<RectTransform>();
        //GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        Camera mc = GameObject.Find("Main Camera").GetComponent<Camera>();
        mc.orthographic = true;
        GetComponent<Canvas>().worldCamera = mc;
    }

    public float hideX = -400;
    public float showX = 0;

    // Update is called once per frame
    void Update()
    {
        if (shift)
        {
            if (toLeft.anchoredPosition3D.x > hideX)
            {
                float x = Mathf.Lerp(showX, hideX, (timel += Time.deltaTime) / length);
                toLeft.anchoredPosition3D = new Vector3(x, toLeft.anchoredPosition3D.y, toLeft.anchoredPosition3D.z);
            }
            else if (toRight.anchoredPosition3D.x < showX)
            {
                float x = Mathf.Lerp(hideX, showX, (timer += Time.deltaTime) / length);
                toRight.anchoredPosition3D = new Vector3(x, toRight.anchoredPosition3D.y, toRight.anchoredPosition3D.z);
            }
            else
            {
                shift = false;
                timel = 0f;
                timer = 0f;
            }
        }
    }

    private bool shift = false;

    private RectTransform toLeft;
    private RectTransform toRight;

    public void OtherSettings()
    {
        if (shift)
        {
            return;
        }
        toLeft = ct;
        toRight = lt;
        shift = true;
        Debug.Log(ct.anchoredPosition3D);
        Debug.Log(lt.anchoredPosition3D);
    }

    public void CharacterInfo()
    {
        if (shift)
        {
            return;
        }
        toLeft = lt;
        toRight = ct;
        shift = true;
        Debug.Log(ct.anchoredPosition3D);
        Debug.Log(lt.anchoredPosition3D);
    }

    public float length = 1f;
    private float timel = 0f;
    private float timer = 0f;

    //public float factor = 0.1f;

    //IEnumerator MoveLeft(RectTransform toLeft)
    //{
    //    while (toLeft.anchoredPosition3D.x > hideX)
    //    {
    //        float x = Mathf.Lerp(toLeft.anchoredPosition3D.x, hideX, factor);
    //        toLeft.anchoredPosition3D = new Vector3(x, toLeft.anchoredPosition3D.y, toLeft.anchoredPosition3D.z);
    //        yield return null;
    //    }
    //    //yield return StartCoroutine(MoveRight(toRight));
    //}

    //IEnumerator MoveRight(RectTransform toRight)
    //{
    //    Debug.Log("asdf");
    //    while (toRight.anchoredPosition3D.x < showX)
    //    {
    //        float x = Mathf.Lerp(toRight.anchoredPosition3D.x, showX, factor);
    //        toRight.anchoredPosition3D = new Vector3(x, toRight.anchoredPosition3D.y, toRight.anchoredPosition3D.z);
    //        yield return null;
    //    }
    //    yield return null;
    //}

}

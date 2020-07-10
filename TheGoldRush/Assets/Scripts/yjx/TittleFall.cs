using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

public class TittleFall : MonoBehaviour
{
    public GameObject tittle;
    private RectTransform rt;
    private bool tittleFalling = true;

    public float startY = 600f;
    public float finalY = 100f;

    public float howLong = 3f;

    private float time = 0.0f;

    private AudioSource _audio;

    public AudioClip hong;
    public AudioClip theGoldRush;

    public Dropdown numPlayerDropdown;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(tittle.GetComponent<RectTransform>().anchoredPosition3D);
        rt = tittle.GetComponent<RectTransform>();
        //rt.anchoredPosition3D = new Vector3(rt.anchoredPosition3D.x, startY, rt.anchoredPosition3D.y);
        SetY(rt, startY);
        _audio = GetComponent<AudioSource>();
        numPlayerDropdown.value = GameManager.NumPlayer() / 2;
        //bprt = buttonsPanel.GetComponent<RectTransform>();
        //SetY(bprt, startYPanel);
    }

    public void SetY(RectTransform rt, float y)
    {
        rt.anchoredPosition3D = new Vector3(rt.anchoredPosition3D.x, y, rt.anchoredPosition3D.y);
    }

    private float factor = 0;

    // Update is called once per frame
    void Update()
    {
        if (tittleFalling)
        {
            time += Time.deltaTime * factor;
            factor += 0.1f;
            SetY(rt, Mathf.Lerp(startY, finalY, time / howLong));
            if (rt.anchoredPosition3D.y <= finalY)
            {
                tittleFalling = false;
                //AudioSource.PlayClipAtPoint(hong, Vector3.zero, 5f);
                _audio.PlayOneShot(hong, 2f);
                StartCoroutine(PlayTheGoldRush(1));
                Camera.main.GetComponent<Animator>().SetTrigger("shake");
            }
        }

        //PanelFloat();
    }



    IEnumerator PlayTheGoldRush(float sec)
    {
        yield return new WaitForSeconds(sec);
        _audio.PlayOneShot(theGoldRush, 10f);
    }

    public void SetNumPlayer(int n)
    {
        int[] nums = new int[3] { 1, 2, 4 };
        GameManager.instance.SetNumPlayer(nums[n]);
    }

    //public GameObject buttonsPanel;

    ////private bool panelFloating = true;
    //public float howLongPanel = 1f;
    //private float timePanel = 0f;
    //public float startYPanel = -400f;
    //public float finalYPanel = 0f;

    //private RectTransform bprt;

    //private void PanelFloat()
    //{
    //    if (bprt.anchoredPosition3D.y < finalYPanel)
    //    {
    //        SetY(bprt, Mathf.Lerp(startYPanel, finalYPanel, (time += Time.deltaTime) / howLongPanel));
    //    }
    //}

}

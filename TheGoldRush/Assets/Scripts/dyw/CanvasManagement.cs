using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManagement : MonoBehaviour
{
    // Start is called before the first frame update

    public float duration = 1f;
    void Start()
    {
        return;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        GameObject canvas = FindInActiveObjectByTag("Canvas");
        yield return new WaitForSeconds(duration);
        if (canvas)
        {
            Debug.Log("find canvas");
            canvas.SetActive(true);
            Canvas c = canvas.GetComponent<Canvas>();
            if (c.renderMode == RenderMode.ScreenSpaceCamera && c.worldCamera == null)
            {
                c.worldCamera = Camera.main;
            }
        }
    }
    // Update is called once per frame
    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}

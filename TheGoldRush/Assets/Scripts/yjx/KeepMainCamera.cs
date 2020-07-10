using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepMainCamera : MonoBehaviour
{

    static private KeepMainCamera instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-0.5f, 0.2f, -13f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void DestroySelf()
    {
        Destroy(instance.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmManager : MonoBehaviour
{

    private AudioSource bgm;
    private bool intense;

    public static bgmManager instance;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        bgm = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void intenseBGM(bool increase){
        if (increase)
        {
            Increase();
        }
        else
        {
            Restore();
        }
    }

    void Restore()
    {
        if (intense)
        {
            bgm.volume -= 0.03f;
            bgm.pitch -= 0.7f;
            intense = false;
        }
    }

    void Increase()
    {
        if (!intense)
        {
            bgm.volume += 0.03f;
            bgm.pitch += 0.7f;
            intense = true;
        }
    }

    public AudioClip win;

    static public void Win()
    {
        //bgm.Stop();
        //bgm.PlayOneShot(win);
        instance.Restore();
        instance.bgm.Stop();
        instance.bgm.PlayOneShot(instance.win);
    }

    static public void Play()
    {
        instance.bgm.Play();
    }
}

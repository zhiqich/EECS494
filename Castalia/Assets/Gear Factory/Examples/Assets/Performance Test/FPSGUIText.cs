using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSGUIText : MonoBehaviour 
{
    public  float updateInterval = 0.5F;
 
    private float accum   = 0; 
    private int   frames  = 0; 
    private float timeleft; 

    private Text txt;

    void Start()
    {
        txt = GetComponent<Text>();
        if (txt == null)
        {
            Debug.Log("UtilityFramesPerSecond needs a UI Text component!");
            enabled = false;
            return;
        }
        timeleft = updateInterval;  
		
    }
 
    void Update()
    { 
        if (txt != null)
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            frames++;

            if (timeleft <= 0.0)
            {
                float fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);
                txt.text = format;

                if (fps < 30)
                    txt.color = Color.yellow;
                else
                    if (fps < 10)
                txt.color = Color.red;
                else
                txt.color = Color.green;

                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
    }
}
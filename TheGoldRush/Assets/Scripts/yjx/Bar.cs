using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{

    public Text text;
    public Image fill;
    //public int Max;
    private Slider s;

    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<Slider>();
    }

    public void Set(int m)
    {
        s.value = 0;
        s.maxValue = m;
        text.text = s.value.ToString();
        s = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewValue(int value)
    {
        text.text = value.ToString();
        //fill.fillAmount = (float)value / (float)Max;
        s.value = value;
    }
}

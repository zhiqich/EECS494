﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicateWB : MonoBehaviour
{
    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (weapon.which_weapon == 1)
        {
            GetComponent<Text>().color = new Color(255, 0, 0, 255);
        }
        else
        {
            GetComponent<Text>().color = new Color(255, 255, 255, 255);
        }
    }
}
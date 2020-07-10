﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionOnClick : MonoBehaviour {

    public string destination_scene_name;
    public AnimationCurve easing_curve;
    public Texture fade_shape = null;

	// Use this for initialization
	void Start () 
    {
        GetComponent<Button>().onClick.AddListener(_OnClick);
	}
	
    void _OnClick()
    {
        // GameObject.Find("Canvas").SetActive(false);
        SceneTransitionController.RequestSceneTransition(destination_scene_name, 2.0f, _SceneTransitionCallback, fade_shape, easing_curve);
    }

    void _SceneTransitionCallback(SceneTransitionState transition_state, string scene_name)
    {
        Debug.Log(transition_state);
    }
}
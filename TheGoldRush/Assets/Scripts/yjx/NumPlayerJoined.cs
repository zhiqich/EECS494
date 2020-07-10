using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumPlayerJoined : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        OnNumChanged(PlayersManager.instance.numPlayerJoined);
        PlayersManager.instance.onNumPlayerjoinedChanged += OnNumChanged;
    }

    private void OnDestroy()
    {
        PlayersManager.instance.onNumPlayerjoinedChanged -= OnNumChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNumChanged(int num)
    {
        text.text = num.ToString() + "/" + GameManager.NumPlayer().ToString() + " Joined";
    }
}

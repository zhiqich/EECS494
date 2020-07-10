using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayer2Battleground : MonoBehaviour
{

    public GameObject play;

    // Start is called before the first frame update
    void Start()
    {
        play.SetActive(false);
        GameManager.instance.onBeingAbleToPlay += OnBeingAbleToPlay;
    }

    private void OnDestroy()
    {
        GameManager.instance.onBeingAbleToPlay -= OnBeingAbleToPlay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBeingAbleToPlay()
    {
        play.SetActive(true);
    }
}

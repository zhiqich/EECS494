using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    //GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        onClickSound = GetComponent<AudioSource>();
    }

    private AudioSource onClickSound;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int scene)
    {
        onClickSound.Play();
        GameManager.instance.LoadSceneAsync(scene, null);
    }

    public void Exit()
    {
        onClickSound.Play();
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        // Only Call this in GetPlayer
        onClickSound.Play();
        ParsecManager.DestroySelf();
        KeepMainCamera.DestroySelf();
        PlayersManager.DestroySelf();
        GameManager.instance.LoadSceneAsync(0, null);
    }

    public void BackToGetPlayer()
    {
        // only call this in battleground
        onClickSound.Play();
        GameManager.instance.LoadSceneAsync(1, GameManager.Restart);
    }

    public void OnClickSound()
    {
        onClickSound.Play();
    }

    public void GetPlayer2Battleground()
    {
        GameManager.instance.StartGame();
    }
}

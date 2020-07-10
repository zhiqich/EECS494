using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Score : MonoBehaviour
{
    public AudioClip gold;
    private int score = 0;
    PlayerBrain.Color c;

    private Animator anim;
    // private int[] playerScores;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log((GameManager.NumPlayer() + 1) / 2);
        // playerScores = new int[(GameManager.NumPlayer() + 1) / 2];
        c = GetComponentInChildren<BaseTrigger>().color;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if ((Input.GetKeyDown(KeyCode.Alpha1) && c == PlayerBrain.Color.Red) ||
        //    (Input.GetKeyDown(KeyCode.Alpha2) && c == PlayerBrain.Color.Blue))
        //{
        //    score++;
        //    AudioSource.PlayClipAtPoint(gold, gameObject.transform.position, 1f);
        //    EventBus.Publish<ScoreEvent>(new ScoreEvent(this.score, c, playerID, 1));
        //}
    }

    public void PlayerScore(int playerID, int numGoldBar)
    {
        // playerID --;
        // playerID = playerID / 2;
        switch (numGoldBar)
        {
            case -1:
                anim.SetTrigger("M1");
                break;
            case 1:
                anim.SetTrigger("P1");
                break;
            case 2:
                anim.SetTrigger("P2");
                break;
            case 3:
                anim.SetTrigger("P3");
                break;
            default:
                Debug.LogAssertion("NumGoldBug: " + numGoldBar);
                break;
        }
        score += numGoldBar;
        AudioSource.PlayClipAtPoint(gold, gameObject.transform.position, 1f);
        // if (numGoldBar > 0){
        //     playerScores[playerID] += numGoldBar;
        // }
        // EventBus.Publish<ScoreEvent>(new ScoreEvent(this.score, c, this.getHigherPlayerID(), playerScores.Max()));
        EventBus.Publish<ScoreEvent>(new ScoreEvent(this.score, c, playerID, numGoldBar));
    }

    public int GetScore()
    {
        return score;
    }

    // public int getHigherPlayerID(){
    //     return Array.IndexOf(playerScores, playerScores.Max());
    // }
}

public class ScoreEvent
{
    public int score = 0;

    public PlayerBrain.Color color;
    // public int higherPlayer;
    // public int higherScore;
    public int playerID;
    public int playerScore;
    public ScoreEvent(int s, PlayerBrain.Color c, int hp, int hs)
    {
        score = s;
        color = c;
        playerID = hp;
        playerScore = hs;
        // higherPlayer = hp * 2;
        // higherPlayer ++;
        // if (c == PlayerBrain.Color.Red){
        //     higherPlayer ++;
        // }
        // higherScore = hs;

    }
    public override string ToString()
    {
        return score.ToString();
    }
}

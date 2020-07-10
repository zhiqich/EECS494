using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour
{

    public PlayerBrain.Color color = PlayerBrain.Color.Blue;

    //public GameObject note;

    public GameObject indicator;
    public GameObject baseObject;

    //private int numPlayerNearby = 0;
    // Start is called before the first frame update
    void Start()
    {
        Color c = color == PlayerBrain.Color.Blue ? Color.blue : Color.red;
        indicator.GetComponent<SpriteRenderer>().color = c;
    }

    // Update is called once per frame
    void Update()
    {
        //if (numPlayerNearby > 0)
        //{
        //    note.SetActive(true);
        //}
        //else
        //{
        //    note.SetActive(false);
        //}
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        PlayerBrain pb = collision.GetComponent<PlayerBrain>();
    //        if (pb.color == color)
    //        {
    //            numPlayerNearby++;
    //        }
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        PlayerBrain pb = collision.GetComponent<PlayerBrain>();
    //        if (pb.color == color)
    //        {
    //            numPlayerNearby--;
    //        }
    //    }
    //}
}

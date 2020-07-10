using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using ParsecGaming;
using ParsecUnity;

public class ColorSelector : MonoBehaviour
{
    public int playerId;
    public Text px;

    private PlayersManager pm;


    // Start is called before the first frame update
    void Start()
    {
        px.text = "P" + playerId.ToString();
        pm = GetComponentInParent<PlayersManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerId == 1)
        //{
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        pm.SelectColor(playerId, PlayerBrain.Color.Red);
        //    }
        //    else if (Input.GetKey(KeyCode.D))
        //    {
        //        pm.SelectColor(playerId, PlayerBrain.Color.Blue);
        //    }
        //}
        //else
        //{
        //    if (Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        pm.SelectColor(playerId, PlayerBrain.Color.Red);
        //    }
        //    else if (Input.GetKey(KeyCode.RightArrow))
        //    {
        //        pm.SelectColor(playerId, PlayerBrain.Color.Blue);
        //    }
        //}

        if (ParsecInput.GetKey(playerId, KeyCode.A))
        {
            pm.SelectColor(playerId, PlayerBrain.Color.Red);
        }
        else if (ParsecInput.GetKey(playerId, KeyCode.D))
        {
            pm.SelectColor(playerId, PlayerBrain.Color.Blue);
        }
    }
}

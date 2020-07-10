using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParsecUnity;
using ParsecGaming;

public class Selector : MonoBehaviour
{

    public int playerId;
    public Text px;

    public Vector2 currentLocalPos;

    static private Vector2[] positions = new Vector2[] { new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1) };

    private TeamCharacterSelector tcs;

    // Start is called before the first frame update
    void Start()
    {
        tcs = GetComponentInParent<TeamCharacterSelector>();
        px.text = "P" + playerId.ToString();
        //currentLocalPos = new Vector2(-1, 1);
        transform.localPosition = currentLocalPos;
        transform.localScale = new Vector3(0.5f, 0.6f, 1);
    }

    private bool confirmed = false;
    public GameObject ready;

    void Select()
    {
        if (confirmed)
        {
            return;
        }
        //if (playerId == 1)
        //{
        //    if (Input.GetKey(KeyCode.W) && currentLocalPos.y == -1 && tcs.CanMoveTo(playerId, new Vector2(currentLocalPos.x, 1)))
        //    {
        //        currentLocalPos = new Vector2(currentLocalPos.x, 1);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.A) && currentLocalPos.x == 1 && tcs.CanMoveTo(playerId, new Vector2(-1, currentLocalPos.y)))
        //    {
        //        currentLocalPos = new Vector2(-1, currentLocalPos.y);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.S) && currentLocalPos.y == 1 && tcs.CanMoveTo(playerId, new Vector2(currentLocalPos.x, -1)))
        //    {
        //        currentLocalPos = new Vector2(currentLocalPos.x, -1);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.D) && currentLocalPos.x == -1 && tcs.CanMoveTo(playerId, new Vector2(1, currentLocalPos.y)))
        //    {
        //        currentLocalPos = new Vector2(1, currentLocalPos.y);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.Return))
        //    {
        //        confirmed = true;
        //        ready.SetActive(true);
        //        tcs.Ready(playerId);
        //    }
        //}
        //else
        //{
        //    if (Input.GetKey(KeyCode.UpArrow) && currentLocalPos.y == -1 && tcs.CanMoveTo(playerId, new Vector2(currentLocalPos.x, 1)))
        //    {
        //        currentLocalPos = new Vector2(currentLocalPos.x, 1);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.LeftArrow) && currentLocalPos.x == 1 && tcs.CanMoveTo(playerId, new Vector2(-1, currentLocalPos.y)))
        //    {
        //        currentLocalPos = new Vector2(-1, currentLocalPos.y);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.DownArrow) && currentLocalPos.y == 1 && tcs.CanMoveTo(playerId, new Vector2(currentLocalPos.x, -1)))
        //    {
        //        currentLocalPos = new Vector2(currentLocalPos.x, -1);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.RightArrow) && currentLocalPos.x == -1 && tcs.CanMoveTo(playerId, new Vector2(1, currentLocalPos.y)))
        //    {
        //        currentLocalPos = new Vector2(1, currentLocalPos.y);
        //        transform.localPosition = currentLocalPos * 0.94f;
        //    }
        //    else if (Input.GetKey(KeyCode.RightShift))
        //    {
        //        confirmed = true;
        //        ready.SetActive(true);
        //        tcs.Ready(playerId);
        //    }
        //}

        if (ParsecInput.GetKey(playerId, KeyCode.W) && currentLocalPos.y == -1 && tcs.CanMoveTo(playerId, new Vector2(currentLocalPos.x, 1)))
        {
            currentLocalPos = new Vector2(currentLocalPos.x, 1);
            transform.localPosition = currentLocalPos;
        }
        else if (ParsecInput.GetKey(playerId, KeyCode.A) && currentLocalPos.x == 1 && tcs.CanMoveTo(playerId, new Vector2(-1, currentLocalPos.y)))
        {
            currentLocalPos = new Vector2(-1, currentLocalPos.y);
            transform.localPosition = currentLocalPos;
        }
        else if (ParsecInput.GetKey(playerId, KeyCode.S) && currentLocalPos.y == 1 && tcs.CanMoveTo(playerId, new Vector2(currentLocalPos.x, -1)))
        {
            currentLocalPos = new Vector2(currentLocalPos.x, -1);
            transform.localPosition = currentLocalPos;
        }
        else if (ParsecInput.GetKey(playerId, KeyCode.D) && currentLocalPos.x == -1 && tcs.CanMoveTo(playerId, new Vector2(1, currentLocalPos.y)))
        {
            currentLocalPos = new Vector2(1, currentLocalPos.y);
            transform.localPosition = currentLocalPos;
        }
        else if (ParsecInput.GetKey(playerId, KeyCode.Return))
        {
            confirmed = true;
            AudioSource.PlayClipAtPoint(readyMusic, transform.position, 1f);
            ready.SetActive(true);
            tcs.Ready(playerId);
        }
        else if (ParsecInput.GetKey(playerId, KeyCode.Q) && !confirmed)
        {
            tcs.PlayerLeaveTeam(playerId);
        }
    }


    // Update is called once per frame
    void Update()
    {
        Select();
    }

    public AudioClip readyMusic;
}

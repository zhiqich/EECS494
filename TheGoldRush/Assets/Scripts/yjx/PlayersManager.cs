using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParsecGaming;
using ParsecUnity;

public class PlayersManager : MonoBehaviour
{

    private class PlayerInfo
    {
        public GameObject colorSelector;
        public Parsec.ParsecGuest guest;
        public int id;
    }

    public GameObject colorSelectorPrefab;

    private PlayerInfo[] players;

    public delegate void OnNumPlayerJoinedChanged(int n);
    public OnNumPlayerJoinedChanged onNumPlayerjoinedChanged;

    public int numPlayerJoined = 0;

    private Vector3[] pos = new Vector3[4] { new Vector3(0, 2.8f, 0), new Vector3(0, 0.9f, 0), new Vector3(0, -0.9f, 0), new Vector3(0, -2.8f, 0) };

    static public PlayersManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        players = new PlayerInfo[GameManager.NumPlayer()];
        //Debug.Log(players[0]);
    }


    // Start is called before the first frame update
    void Start()
    {
        //selectors = new GameObject[GameManager.NumPlayer()];
    }

    //private int id = 1;
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    SpawnPlayer(new Parsec.ParsecGuest());
        //}
    }

    public void SpawnPlayer(Parsec.ParsecGuest guest)
    {
        if (numPlayerJoined >= GameManager.NumPlayer())
        {
            return;
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                players[i] = new PlayerInfo();
                players[i].colorSelector = Instantiate(colorSelectorPrefab);
                players[i].colorSelector.transform.parent = transform;
                players[i].colorSelector.transform.localPosition = pos[i];
                numPlayerJoined++;
                this.onNumPlayerjoinedChanged?.Invoke(numPlayerJoined);
                players[i].id = i + 1;
                players[i].colorSelector.GetComponent<ColorSelector>().playerId = players[i].id;
                players[i].guest = guest;
                ParsecInput.AssignGuestToPlayer(guest, players[i].id);
                return;
            }
        }
    }



    public void SelectColor(int id, PlayerBrain.Color color)
    {
        TeamCharacterSelector team;
        if (color == PlayerBrain.Color.Blue)
        {
            team = GameObject.Find("BlueTeamSelector").GetComponent<TeamCharacterSelector>();
        }
        else
        {
            team = GameObject.Find("RedTeamSelector").GetComponent<TeamCharacterSelector>();
        }
        if (team.SpawnCharacterSelector(id))
        {
            players[id - 1].colorSelector.SetActive(false);
        }
    }

    public void PlayerLeave(Parsec.ParsecGuest guest)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && players[i].guest.id == guest.id)
            {
                GameObject bt = GameObject.Find("BlueTeamSelector");
                GameObject rt = GameObject.Find("RedTeamSelector");
                if (bt != null && rt != null)
                {
                    bt.GetComponent<TeamCharacterSelector>().PlayerLeave(players[i].id);
                    rt.GetComponent<TeamCharacterSelector>().PlayerLeave(players[i].id);
                }
                //GameManager.PlayerLeave(players[i].id);
                //ParsecInput.UnassignGuest(players[i].guest);
                Destroy(players[i].colorSelector);
                players[i] = null;
                numPlayerJoined--;
                this.onNumPlayerjoinedChanged?.Invoke(numPlayerJoined);
                return;
            }
        }
    }

    public void LeaveAllPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                GameObject bt = GameObject.Find("BlueTeamSelector");
                GameObject rt = GameObject.Find("RedTeamSelector");
                if (bt != null && rt != null)
                {
                    bt.GetComponent<TeamCharacterSelector>().PlayerLeave(players[i].id);
                    rt.GetComponent<TeamCharacterSelector>().PlayerLeave(players[i].id);
                }
                //GameManager.PlayerLeave(players[i].id);
                ParsecInput.UnassignGuest(players[i].guest);
                Destroy(players[i].colorSelector);
                players[i] = null;
                numPlayerJoined--;
                this.onNumPlayerjoinedChanged?.Invoke(numPlayerJoined);
            }
        }
        Debug.Assert(numPlayerJoined == 0);
    }

    public void Restart()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                players[i].colorSelector.SetActive(true);
            }
        }
    }

    static public void DestroySelf()
    {
        Destroy(instance.gameObject);
    }

    public void PlayerLeaveTeam(int playerId)
    {
        if (players[playerId - 1] != null)
        {
            players[playerId - 1].colorSelector.SetActive(true);
        }
    }
}

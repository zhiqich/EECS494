using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamCharacterSelector : MonoBehaviour
{

    public PlayerBrain.Color color;

    private int numPlayerInTeam = 0;
    //private int numReady = 0;

    public GameObject selectorPrefab;

    private Dictionary<int, Selector> id2selector;
    private Dictionary<int, GameObject> id2selectorObj;
    private Dictionary<int, int> peers;
    private Dictionary<int, Vector3> id2InitialPos;

    private Dictionary<Vector2, Character> pos2character;

    public ParticleSystem[] pss;
    public SpriteRenderer[] srs;
    public Text[] names;

    public Vector3[] initialPos;

    private Queue<Vector3> freePosQ;

    //private Dictionary<Vector3, bool> initialPos2id;

    // Start is called before the first frame update
    void Start()
    {
        id2selector = new Dictionary<int, Selector>();
        peers = new Dictionary<int, int>();
        id2selectorObj = new Dictionary<int, GameObject>();
        //GetComponentsInChildren<SpriteRenderer>()[0].color = PlayerBrain.GetColor(color);
        pos2character = new Dictionary<Vector2, Character>
        {
            { new Vector2(-1, 1), Character.PickaxeMan },
            { new Vector2(1, 1), Character.Shooter },
            { new Vector2(1, -1), Character.BomberMan },
            { new Vector2(-1, -1), Character.Thief }
        };

        id2InitialPos = new Dictionary<int, Vector3>();
        freePosQ = new Queue<Vector3>();
        foreach (var p in initialPos)
        {
            freePosQ.Enqueue(p);
        }
    }

    //public delegate void OnSelectorMoved(Vector2 origin, Vector2 target);

    //public OnSelectorMoved onSelectorMoved;

    //private int id = 1;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SpawnCharacterSelector(id);
        //    id++;
        //}
    }

    private Color showColor = new Color(1, 1, 1, 1);
    private Color hideColor = new Color(1, 1, 1, 130f / 255f);

    private Color textShowColor = new Color(0, 0, 0, 1);
    private Color texthideColor = new Color(0, 0, 0, 130f / 255f);

    public bool CanMoveTo(int playerId, Vector2 target)
    {
        if (numPlayerInTeam > 1 && id2selector[peers[playerId]].currentLocalPos == target)
        {
            return false;
        }
        else
        {
            Vector2 origin = id2selector[playerId].currentLocalPos;
            pss[(int)pos2character[origin]].Stop();
            srs[(int)pos2character[origin]].color = hideColor;
            names[(int)pos2character[origin]].color = texthideColor;

            pss[(int)pos2character[target]].Play();
            srs[(int)pos2character[target]].color = showColor;
            names[(int)pos2character[target]].color = textShowColor;
            return true;
        }
    }

    //private void MoveSelectorVisuals(Vector2 o, Vector2 t)
    //{

    //}

    private int firstId = 0;

    public bool SpawnCharacterSelector(int playerId)
    {
        if (numPlayerInTeam >= 2)
        {
            return false;
        }
        numPlayerInTeam++;
        GameObject selector = GameObject.Instantiate(selectorPrefab);
        id2selectorObj.Add(playerId, selector);
        selector.transform.parent = this.gameObject.transform;
        //selector.GetComponent<SpriteRenderer>().color = PlayerBrain.GetColor(color);
        if (color == PlayerBrain.Color.Blue)
        {
            selector.GetComponent<SpriteRenderer>().color = ScoreText.ColorHex(0x82ff);
        }
        else
        {
            selector.GetComponent<SpriteRenderer>().color = Color.red;
        }
        Selector s = selector.GetComponent<Selector>();
        s.playerId = playerId;
        id2selector.Add(playerId, s);
        if (firstId == 0)
        {
            // the first player
            s.currentLocalPos = new Vector2(-1, 1);
            firstId = playerId;
        }
        else
        {
            peers.Add(firstId, playerId);
            peers.Add(playerId, firstId);
            if (id2selector[firstId].currentLocalPos == new Vector2(1, 1))
            {
                s.currentLocalPos = new Vector2(-1, 1);
            }
            else
            {
                s.currentLocalPos = new Vector2(1, 1);
            }
        }
        pss[(int)pos2character[s.currentLocalPos]].Play();
        srs[(int)pos2character[s.currentLocalPos]].color = showColor;
        names[(int)pos2character[s.currentLocalPos]].color = textShowColor;
        return true;
    }

    public void Ready(int playerId)
    {
        id2InitialPos.Add(playerId, freePosQ.Dequeue());
        GameManager.Ready(playerId, this.color, pos2character[id2selector[playerId].currentLocalPos], id2InitialPos[playerId]);
    }

    public void PlayerLeave(int playerId)
    {
        if (!id2selector.ContainsKey(playerId))
        {
            Debug.Assert(!id2selectorObj.ContainsKey(playerId));
            return;
        }
        if (numPlayerInTeam == 1)
        {
            Debug.Assert(playerId == firstId);
            firstId = 0;
        }
        else if (playerId == firstId)
        {
            firstId = peers[playerId];
        }

        if (id2InitialPos.ContainsKey(playerId))
        {
            freePosQ.Enqueue(id2InitialPos[playerId]);
            id2InitialPos.Remove(playerId);
        }

        Vector2 origin = id2selector[playerId].currentLocalPos;
        pss[(int)pos2character[origin]].Stop();
        srs[(int)pos2character[origin]].color = hideColor;
        names[(int)pos2character[origin]].color = texthideColor;

        peers.Clear();
        Destroy(id2selectorObj[playerId]);
        id2selectorObj.Remove(playerId);
        id2selector.Remove(playerId);
        numPlayerInTeam--;
        return;
    }

    public void PlayerLeaveTeam(int playerId)
    {
        PlayerLeave(playerId);
        PlayersManager.instance.PlayerLeaveTeam(playerId);
    }
}

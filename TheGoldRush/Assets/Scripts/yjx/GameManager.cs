using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParsecGaming;
using ParsecUnity;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //public GameObject[] RedInitialPos;
    //private int redInitialPosIndicator = 0;
    //public GameObject[] BlueInitialPos;
    //private int blueInitialPosIndicator = 0;

    private bool playing = false;


    public GameObject[] characterPrefabs;

    public int numReady = 0;

    public int numPlayer = 4;

    Dictionary<int, PlayerInfo> id2info;
    Dictionary<int, GameObject> id2obj;

    public delegate void OnSceneLoaded();
    public static int[] killCounts;
    public static Text bqText;
    private static bool printing = false;
    public static UnityEngine.Color gameRed = new UnityEngine.Color(161f / 255f, 19f/255f , 64f/255f, 1);
    public static UnityEngine.Color gameBlue = new UnityEngine.Color(0f / 255f, 130f/255f , 255f/255f, 1);

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
        DontDestroyOnLoad(gameObject);
        id2info = new Dictionary<int, PlayerInfo>();
        id2obj = new Dictionary<int, GameObject>();
    }

    void Start()
    {
        killCounts = new int[GameManager.NumPlayer()];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            instance.LoadSceneAsync(1, GameManager.Restart);
        }
    }

    public delegate void OnBeingAbleToPlay();
    public OnBeingAbleToPlay onBeingAbleToPlay;


    public static void Ready(int id, PlayerBrain.Color color, Character c, Vector3 spawnPos)
    {
        if (instance.numReady == instance.numPlayer)
        {
            return;
        }
        instance.id2info.Add(id, new PlayerInfo(color, c, spawnPos));
        //if (color == PlayerBrain.Color.Blue)
        //{
        //    instance.id2info[id].pos = instance.BlueInitialPos[instance.blueInitialPosIndicator++].transform.position;
        //}
        //else
        //{
        //    instance.id2info[id].pos = instance.RedInitialPos[instance.redInitialPosIndicator++].transform.position;
        //}

        instance.numReady++;

        if (instance.numReady == instance.numPlayer)
        {
            instance.onBeingAbleToPlay?.Invoke();
        }
    }

    public void StartGame()
    {
        instance.playing = true;
        instance.LoadSceneAsync(2, instance.StartGame1);
    }

    //public static void PlayerLeave(int id)
    //{
    //    if (!instance.playing)
    //    {
    //        if (instance.id2info.ContainsKey(id))
    //        {
    //            instance.id2info.Remove(id);
    //            instance.numReady--;
    //        }
    //        if (instance.id2obj.ContainsKey(id))
    //        {
    //            instance.id2obj.Remove(id);
    //        }
    //    }
    //}

    public static void addNewKill(int pID){
        killCounts[pID]++;
        if (killCounts[pID] % 4 == 0){
           if (printing){
               instance.StartCoroutine(waitAndShowBQ(pID));
           }
           else{
               instance.StartCoroutine(showBQ(pID));
           }
        }
    }

    static IEnumerator waitAndShowBQ(int pID){
        yield return new WaitForSeconds(2.5f);
        instance.StartCoroutine(showBQ(pID));
    }

    static IEnumerator showBQ(int pID){
        bqText = GameObject.Find("bqText").GetComponent<Text>();
        printing = true;
        // ScoreText.showBQIMG(GetColor(pID), true);
        // bqText.gameObject.SetActive(true);
        bqText.text = "P" + (pID + 1) + " is dominating!";
        if (GetColor(pID) == PlayerBrain.Color.Blue){
            // bqText.color = UnityEngine.Color.blue;
            bqText.color = gameBlue;
        }
        else
        {
            // bqText.color = UnityEngine.Color.red;
            bqText.color = gameRed;
        }
        yield return new WaitForSeconds(2.5f);
        bqText.text = "";
        // bqText.gameObject.SetActive(false);
        printing = false;
        // ScoreText.showBQIMG(GetColor(pID), false);
    }

    public static bool IsPlaying()
    {
        return instance.playing;
    }

    private GameObject playersManager;

    public void StartGame1()
    {
        //Debug.Log("StartGame() called");
        //ParsecManager.StartPlay();
        BGMController.DestroySelf();
        (playersManager = GameObject.Find("PlayersManager")).SetActive(false);

        //foreach (KeyValuePair<int, PlayerInfo> kvp in id2info)
        //{
        //    id2obj.Add(kvp.Key, Instantiate(characterPrefabs[(int)kvp.Value.characotr], kvp.Value.pos, Quaternion.identity));
        //    PlayerBrain pb = id2obj[kvp.Key].GetComponent<PlayerBrain>();
        //    pb.PlayerID = kvp.Key;
        //    pb.character = kvp.Value.characotr;
        //    pb.SpecifyColor(kvp.Value.color);
        //}
        StartCoroutine(GenerateCharactersAfterTs(2));

        EventBus.Publish<StartEvent>(new StartEvent());
        //instance.StartCoroutine(instance.ReleasePlayersAfter(3));
    }

    IEnumerator GenerateCharactersAfterTs(float t)
    {
        yield return new WaitForSeconds(t);
        foreach (KeyValuePair<int, PlayerInfo> kvp in id2info)
        {
            id2obj.Add(kvp.Key, Instantiate(characterPrefabs[(int)kvp.Value.characotr], kvp.Value.pos, Quaternion.identity));
            PlayerBrain pb = id2obj[kvp.Key].GetComponent<PlayerBrain>();
            pb.PlayerID = kvp.Key;
            pb.character = kvp.Value.characotr;
            pb.SpecifyColor(kvp.Value.color);
        }
    }

    private IEnumerator ReleasePlayersAfter(float sec)
    {
        yield return new WaitForSeconds(sec);

        foreach (KeyValuePair<int, GameObject> kvp in id2obj)
        {
            kvp.Value.GetComponent<PlayerBrain>().Release();
            //kvp.Value.GetComponent<PlayerBrain>().playerIDText.enabled = false;
        }
    }

    public void ReleasePlayers()
    {
        foreach (KeyValuePair<int, GameObject> kvp in id2obj)
        {
            kvp.Value.GetComponent<PlayerBrain>().Release();
            //kvp.Value.GetComponent<PlayerBrain>().playerIDText.enabled = false;
        }
    }

    //public void showID(){
    //    foreach (KeyValuePair<int, GameObject> kvp in id2obj)
    //    {
    //        kvp.Value.GetComponent<PlayerBrain>().playerIDText.enabled = true;
    //    }
    //}

    public static int GetCharacter(int i)
    {
        //return (int)instance.characters[i];
        return (int)instance.id2info[i + 1].characotr;
    }

    public static PlayerBrain.Color GetColor(int i)
    {
        //return (int)instance.characters[i];
        return instance.id2info[i + 1].color;
    }

    public static int NumPlayer()
    {
        return instance.numPlayer;
    }

    public void LoadSceneAsync(int index, OnSceneLoaded on)
    {
        Debug.Log("Hello");
        StartCoroutine(LoadAsyncScene(index, on));
    }

    IEnumerator LoadAsyncScene(int index, OnSceneLoaded on)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //on?.Invoke();
        Debug.Log(on == null);
        if (on != null)
        {
            Debug.Log("Try to call on");
            on();
        }
    }

    static public int CurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    static public void Restart()
    {
        instance.playing = false;
        instance.numReady = 0;
        instance.id2info.Clear();
        instance.id2obj.Clear();
        //instance.blueInitialPosIndicator = 0;
        //instance.redInitialPosIndicator = 0;
        //ParsecManager.Restart();
        instance.playersManager.SetActive(true);
        PlayersManager.instance.Restart();
    }

    public void SetNumPlayer(int n)
    {
        numPlayer = n;
    }

    private class PlayerInfo
    {
        public PlayerBrain.Color color;
        public Character characotr;
        public Vector3 pos;

        public PlayerInfo(PlayerBrain.Color c, Character ch, Vector3 p)
        {
            color = c;
            characotr = ch;
            pos = p;
        }
    }

    static public GameObject GetPlayerObj(int id)
    {
        return instance.id2obj[id];
    }
}

public class StartEvent
{
    public StartEvent() { }
}
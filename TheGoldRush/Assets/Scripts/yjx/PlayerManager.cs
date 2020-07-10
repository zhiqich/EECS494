using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParsecGaming;
using ParsecUnity;

public enum Character
{
    PickaxeMan,
    Shooter,
    Thief,
    BomberMan,
};

public class PlayerManager : MonoBehaviour
{
    public int playerId;
    public Character c = Character.PickaxeMan;
    [HideInInspector] public GameObject playerInstance;
    [HideInInspector] public Parsec.ParsecGuest guest;

    public GameObject[] characterCards;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SelectCharacter();
    }

    public GameObject selector;
    public GameObject checkmark;

    private bool confirmed = false;

    public void AssignGuest()
    {
        ParsecInput.AssignGuestToPlayer(guest, playerId);
    }

    private float timeA = 0.1f;
    private float timeD = 0.1f;

    //public GameObject left;
    //public GameObject right;

    void SelectCharacter()
    {
        if (confirmed)
        {
            return;
        }
        Character lastC = c;
        if (ParsecInput.GetKey(playerId, KeyCode.A) && c > Character.PickaxeMan && timeA >= 0.1f)
        {
            timeA = 0f;
            Debug.Log(selector + playerId.ToString());
            //selector.transform.localPosition -= selector.transform.right * 2;
            foreach (GameObject o in characterCards)
            {
                o.transform.localPosition += o.transform.right * 3f;
            }
            characterCards[(int)c - 1].SetActive(false);
            c--;
            characterCards[(int)c - 1].SetActive(true);
        }
        else if (ParsecInput.GetKey(playerId, KeyCode.D) && c < Character.BomberMan && timeD >= 0.1f)
        {
            timeD = 0f;
            Debug.Log(selector + playerId.ToString());
            //selector.transform.localPosition += selector.transform.right * 2;
            foreach (GameObject o in characterCards)
            {
                o.transform.localPosition -= o.transform.right * 3f;
            }
            characterCards[(int)c - 1].SetActive(false);
            c++;
            characterCards[(int)c - 1].SetActive(true);
        }
        timeA += Time.deltaTime;
        timeD += Time.deltaTime;
        if (ParsecInput.GetKey(playerId, KeyCode.Return))
        {
            //if (!GameManager.Ready(playerId - 1,Pl c))
            //{
            //    return;
            //}
            confirmed = true;
            checkmark.SetActive(confirmed);
        }
    }

    public void Restart()
    {
        confirmed = false;
        checkmark.SetActive(false);
    }

    public void BreakDown()
    {
        Destroy(playerInstance);
    }
}

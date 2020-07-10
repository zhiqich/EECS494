using UnityEngine;
using UnityEngine.UI;
using ParsecGaming;
using UnityEngine.Networking;
using System.Collections;

public class ParsecManager : MonoBehaviour
{
    private ParsecStreamGeneral streamer;
    private ParsecUnity.API.SessionResultDataData authdata;

    public bool isStreaming = false;

    public static ParsecManager instance;

    void Awake()
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
        PlayersManager.instance.SpawnPlayer(new Parsec.ParsecGuest());
        streamer = GameObject.Find("Main Camera").gameObject.GetComponent<ParsecStreamGeneral>();
        if (streamer != null)
        {
            streamer.GuestConnected += Streamer_GuestConnected;
            streamer.GuestDisconnected += Streamer_GuestDisconnected;
        }
        DontDestroyOnLoad(gameObject);
    }


    private void Streamer_GuestDisconnected(object sender, Parsec.ParsecGuest guest)
    {
        PlayersManager.instance.PlayerLeave(guest);
    }

    private void Streamer_GuestConnected(object sender, Parsec.ParsecGuest guest)
    {
        if (GameManager.IsPlaying())
        {
            return;
        }
        PlayersManager.instance.SpawnPlayer(guest);
    }


    public string GetAccessCode()
    {
        ParsecUnity.API.SessionData sessionData = streamer.RequestCodeAndPoll("1ZYSeZxj6nRV8I233BG7wojM4QT");
        if ((sessionData != null) && (sessionData.data != null))
        {
            return "https://parsecgaming.com/activate/" + sessionData.data.user_code;
        }
        else
        {
            return "";
        }
    }

    public delegate void OnAuthenticated();
    public OnAuthenticated onAuthenticated;

    public void AuthenticationPoll(ParsecUnity.API.SessionResultDataData data, ParsecUnity.API.SessionResultEnum status)
    {
        switch (status)
        {
            //case ParsecUnity.API.SessionResultEnum.PolledTooSoon:
            //    break;
            //case ParsecUnity.API.SessionResultEnum.Pending:
            //    break;
            case ParsecUnity.API.SessionResultEnum.CodeApproved:
                //PanelAuthentication.gameObject.SetActive(false);
                authdata = data;
                //PanelParsecControl.gameObject.SetActive(true);
                onAuthenticated?.Invoke();
                break;
            //case ParsecUnity.API.SessionResultEnum.CodeInvallidExpiredDenied:
            //    break;
            //case ParsecUnity.API.SessionResultEnum.Unknown:
            //    break;
            default:
                break;
        }
    }

    public string StartParsec()
    {
        streamer.StartParsec(GameManager.NumPlayer(), false, "Bat", "EECS494 P3 by Gozilli", authdata.id);
        string url = streamer.GetInviteUrl(authdata, 3600, 100);
        isStreaming = true;
        return url;
    }

    public void StopParsec()
    {
        streamer.StopParsec();
        PlayersManager.instance.LeaveAllPlayers();
        isStreaming = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    static public void DestroySelf()
    {
        Destroy(instance.gameObject);
    }
}

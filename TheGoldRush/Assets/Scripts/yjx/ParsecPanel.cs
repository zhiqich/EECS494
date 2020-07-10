using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParsecPanel : MonoBehaviour
{
    public GameObject PanelAuthentication;
    public GameObject PanelParsecControl;
    //public InputField VerificationUri;
    public InputField ShortLinkUri;

    public GameObject startParsecBtn;
    public GameObject stopParsecBtn;

    static private string inviteLink = "";

    // Start is called before the first frame update
    void Start()
    {
        if (ParsecManager.instance.isStreaming)
        {
            Debug.Assert(inviteLink != "");
            //Destroy(gameObject);
            PanelAuthentication.SetActive(false);
            PanelParsecControl.SetActive(true);
            ShortLinkUri.text = inviteLink;
            //startParsecBtn.GetComponent<Button>().interactable = false;
            startParsecBtn.SetActive(false);
            return;
        }
        ParsecManager.instance.onAuthenticated += OnAuthencated;
    }

    private void OnDestroy()
    {
        if (ParsecManager.instance != null)
        {
            ParsecManager.instance.onAuthenticated -= OnAuthencated;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartParsec()
    {
        inviteLink = ParsecManager.instance.StartParsec();
        ShortLinkUri.text = inviteLink;
        //startParsecBtn.GetComponent<Button>().interactable = false;
        startParsecBtn.SetActive(false);
    }

    public void StopParsec()
    {
        ParsecManager.instance.StopParsec();
        startParsecBtn.SetActive(true);
        stopParsecBtn.SetActive(false);
        ShortLinkUri.text = "";
        PanelAuthentication.SetActive(true);
        PanelParsecControl.SetActive(false);
    }

    public void GetAccessCode()
    {
        //VerificationUri.text = ParsecManager.instance.GetAccessCode();
        Application.OpenURL(ParsecManager.instance.GetAccessCode());
    }

    public void OnAuthencated()
    {
        PanelAuthentication.gameObject.SetActive(false);
        PanelParsecControl.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScoreText : MonoBehaviour
{

    //public GameObject DataAnalytics;
    private int blue_score = 0;

    private int red_score = 0;

    private int[] playerScores;

    Subscription<ScoreEvent> score_event_subscription;
    Subscription<EndEvent> end_event_subscription;
    Subscription<StartEvent> start_event_subscription;

    //public GameObject topPanel;
    // TopPanel
    public int targetInt;
    public Text targetText;
    public Text redScore;
    public Text blueScore;
    //public Slider blueSlider;
    //public Slider redSlider;

    public GameObject blueBar;
    public GameObject redBar;

    // Tutorial
    public GameObject tutorialPanel;

    // CountDown
    public GameObject countDownPanel;
    public Text countDownText;

    // FinalResult
    public GameObject finalResultPanel;
    public Text winText;
    public Text[] finalText;
    public Image[] finalImages;
    public Text mvpText;

    public Sprite[] characterSprites;

    //public AudioClip winMusic;

    public ParticleSystem redParticle;

    public ParticleSystem blueParticle;
    public GameObject[] finalAnimations;
    private float redplace = -10.5f;
    private float blueplace = 6.5f;
    private bool canLerp = false;
    private finalAniInfo[] animationSet;
    private PlayerBrain.Color whoWin = PlayerBrain.Color.Red;
    private float[] aniPositions;
    public ParticleSystem mvpParticle;
    public GameObject restartButton;
    private static GameObject BQImages;
    // public static int[] killCounts;
    // public Text bqText;


    //Canvas c;

    // Start is called before the first frame update
    void Start()
    {
        Camera mc = GameObject.Find("Main Camera").GetComponent<Camera>();
        mc.orthographic = false;
        GetComponent<Canvas>().worldCamera = mc;
        score_event_subscription = EventBus.Subscribe<ScoreEvent>(_OnScoreUpdated);
        //end_event_subscription = EventBus.Subscribe<EndEvent>(_OnEndUpdated);
        start_event_subscription = EventBus.Subscribe<StartEvent>(_OnStartUpdated);
        playerScores = new int[GameManager.NumPlayer()];
        targetText.text = "Target: " + targetInt.ToString();
        //blueSlider.maxValue = targetInt;
        //redSlider.maxValue = targetInt;
        //DataAnalytics = GameObject.FindWithTag("GA");
        blueBar.GetComponent<Bar>().Set(targetInt);
        redBar.GetComponent<Bar>().Set(targetInt);
        // killCounts = new int[GameManager.NumPlayer()];

        guidanceBlue.SetActive(false);
        guidanceRed.SetActive(false);
        countDownPanel.SetActive(false);
        // BQImages = GameObject.Find("BQImages");
        
    }

    private void Update()
    {
        if (canLerp){
            for (int i = 0; i < GameManager.instance.numPlayer; i++){
                animationSet[i].ani.transform.position = Vector3.Lerp(animationSet[i].ani.transform.position, animationSet[i].des, 0.2f);
            }
        }
    }

    private bool gameOver = false;

    void _OnStartUpdated(StartEvent e)
    {
        StartCoroutine(CountDown());
    }

    public GameObject guidanceBlue;
    public GameObject guidanceRed;

    public AudioClip countDownAudio;
    public AudioClip startAudio;

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(3f);
        countDownPanel.SetActive(true);
        guidanceBlue.SetActive(true);
        int TotalTime = 3;
        while (TotalTime >= 0)
        {
            countDownText.text = TotalTime.ToString();
            GetComponent<AudioSource>().PlayOneShot(countDownAudio);
            yield return new WaitForSeconds(1);
            TotalTime--;
            if (TotalTime == 1)
            {
                //guidanceBlue.SetActive(false);
                guidanceRed.SetActive(true);
            }
            else if (TotalTime == 0)
            {
                countDownText.text = "GO!";
                GameManager.instance.ReleasePlayers();
                GetComponent<AudioSource>().PlayOneShot(startAudio);
                bgmManager.Play();
                yield return new WaitForSeconds(0.8f);
                break;
            }
        }
        countDownPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    void _OnScoreUpdated(ScoreEvent e)
    {
        if (gameOver)
        {
            return;
        }
        if (e.color == PlayerBrain.Color.Blue)
        {
            blue_score = e.score;
            //blueScore.text = "Team blue score: " + e.score.ToString();
            //blueSlider.value = e.score;
            blueBar.GetComponent<Bar>().NewValue(e.score);
            if (e.score >= targetInt - 5)
            {
                if (!blueParticle.isPlaying)
                {
                    blueParticle.Play();
                    bgmManager.instance.intenseBGM(true);
                }
            }
            else
            {
                if (blueParticle.isPlaying)
                {
                    blueParticle.Stop();
                    bgmManager.instance.intenseBGM(false);
                }
            }
            if (e.playerScore > 0){
                playerScores[e.playerID - 1] += e.playerScore;
            }
            if (blue_score >= targetInt)
            {
                // EndGame(true);
                StartCoroutine(NewEndGame(true));
            }
        }
        else 
        {
            red_score = e.score;
            //redScore.text = "Team red score: " + e.score.ToString();
            //redSlider.value = e.score;
            redBar.GetComponent<Bar>().NewValue(e.score);
            if (e.score >= targetInt - 5)
            {
                if (!redParticle.isPlaying)
                {
                    redParticle.Play();
                    //bgmManager.intenseBGM(true);
                    bgmManager.instance.intenseBGM(true);
                }
            }
            else 
            {
                if (redParticle.isPlaying)
                {
                    redParticle.Stop();
                    bgmManager.instance.intenseBGM(false);
                }
            }
            if (e.playerScore > 0){
                playerScores[e.playerID - 1] += e.playerScore;
            }
            if (red_score >= targetInt)
            {
                // EndGame(false);
                StartCoroutine(NewEndGame(false));
            }
        }
    }

    // public void addNewKill(int pID){
    //     killCounts[pID]++;
    //     if (killCounts[pID] >= 5){
    //         bqText.gameObject.SetActive(true);
    //     }
    // }

    // static IEnumerator ShowBQ(){
    //     yield return new WaitForSeconds(1);
    // }

    public GameObject winPanelBackground;
    public GameObject winPanelVictory;

    // void EndGame(bool blueWin)
    // {
    //     gameOver = true;
    //     //GameManager.instance.showID();
    //     //finalResultPanel.SetActive(true);
    //     Animator anim = winPanelBackground.GetComponent<Animator>();
    //     anim.SetTrigger("Close");
    //     if (blueWin) 
    //     {
    //         //winText.text = "Team blue\nWins!";
    //         //winText.color = Color.blue;
    //         whoWin = PlayerBrain.Color.Blue;
    //         anim.SetTrigger("BlueWin");
    //         winPanelVictory.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(300, -300, 0);
    //         blueplace = 2;
            
    //     }
    //     else
    //     {
    //         //winText.text = "Team red\nWins!";
    //         //winText.color = Color.red;
    //         anim.SetTrigger("RedWin");
    //         winPanelVictory.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-300, 300, 0);
    //         redplace = -6.5f;
    //     }
    //     //printFinalText(blueWin);
    //     StartCoroutine(SetTriggerAfterTs(winPanelVictory.GetComponent<Animator>(), "Appear", 1));
    //     StartCoroutine(ShowCharactersAfterTs(1));
    //     StartCoroutine(ShowScoresAfterTs(2));
    //     //winPanelVictory.GetComponent<Animator>().SetTrigger("Appear");
    //     //SendCharacterScore();
    // }

    public static void showBQIMG(PlayerBrain.Color color, bool enable){
        if (color == PlayerBrain.Color.Blue){
            BQImages.transform.GetChild(0).gameObject.SetActive(enable);
        }
        else{
            BQImages.transform.GetChild(1).gameObject.SetActive(enable);
        }
    }

    static public Color ColorHex(int hex)
    {
        float r = (hex / 0xffff) / (float)0xff;
        hex %= 0x10000;
        float g = (hex / 0xff) / (float)0xff;
        hex %= 0x100;
        float b = hex / (float)0xff;
        return new Color(r, g, b);
    }

    IEnumerator NewEndGame(bool blueWin)
    {
        gameOver = true;
        finalResultPanel.gameObject.SetActive(true);
        if (blueWin) 
        {
            winText.text = "Team Blue\nWins!";
            //winText.color = Color.blue;
            //winText.color = new Color(0x00, 0x82, 0xff);
            winText.color = ColorHex(0x0082ff);
        }
        else
        {
            winText.text = "Team Red\nWins!";
            //winText.color = Color.red;
            //winText.color = new Color(0xa1, 0x13, 0x40);
            winText.color = ColorHex(0xa11340);
        }
        yield return new WaitForSeconds(2);
        //if (blueParticle.isPlaying){
        //    blueParticle.Stop();
        //}
        //if (redParticle.isPlaying){
        //    redParticle.Stop();
        //}
        finalResultPanel.gameObject.SetActive(false);
        //GameManager.instance.showID();
        //finalResultPanel.SetActive(true);
        Animator anim = winPanelBackground.GetComponent<Animator>();
        anim.SetTrigger("Close");
        if (blueWin) 
        {
            whoWin = PlayerBrain.Color.Blue;
            anim.SetTrigger("BlueWin");
            winPanelVictory.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(300, -400, 0);
            blueplace = 2;
            
        }
        else
        {
            anim.SetTrigger("RedWin");
            winPanelVictory.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-300, -400, 0);
            redplace = -6.5f;
        }
        //printFinalText(blueWin);
        StartCoroutine(SetTriggerAfterTs(winPanelVictory.GetComponent<Animator>(), "Appear", 2));
        StartCoroutine(ShowCharactersAfterTs(2));
        StartCoroutine(ShowScoresAfterTs(2.5f));
        bgmManager.Win();
        //winPanelVictory.GetComponent<Animator>().SetTrigger("Appear");
        //SendCharacterScore();
    }

    IEnumerator ShowScoresAfterTs(float t){
        yield return new WaitForSeconds(t);
        printNewFinalText(whoWin == PlayerBrain.Color.Blue);
        restartButton.SetActive(true);
    }

    IEnumerator ShowCharactersAfterTs(float t){
        yield return new WaitForSeconds(t);
        animationSet = new finalAniInfo[GameManager.instance.numPlayer];
        aniPositions = new float[GameManager.instance.numPlayer];
        for (int i = 0; i < GameManager.instance.numPlayer; i++){
            GameObject finalcharacter = GameObject.Instantiate(finalAnimations[GameManager.GetCharacter(i)]);;
            if (GameManager.GetColor(i) != whoWin){    
                Destroy(finalcharacter);        
                finalcharacter = GameObject.Instantiate(finalAnimations[GameManager.GetCharacter(i) + 4]);
            }
            Vector3 targetPosition = putToPlace(GameManager.GetColor(i));
            Vector3 currentPosition = targetPosition;
            currentPosition.y += 5;
            finalcharacter.transform.position = currentPosition;
            if (GameManager.GetCharacter(i) == 3){
                finalcharacter.transform.localScale = new Vector3 (0.65f, 0.65f, 1);
            }
            else{
                finalcharacter.transform.localScale = new Vector3 (0.4f, 0.4f, 1);
            }
            aniPositions[i] = targetPosition.x * 70;
            animationSet[i] = new finalAniInfo(finalcharacter, targetPosition);
        }
        canLerp = true;
    }



    Vector3 putToPlace(PlayerBrain.Color color){
        if (color == PlayerBrain.Color.Blue){
            blueplace += 4;
            return new Vector3(blueplace - 4, -1, 0);
        }
        else{
            redplace += 4;
            return new Vector3(redplace - 4, -1, 0);
        }
    }

    IEnumerator SetTriggerAfterTs(Animator a, string trigger, float t)
    {
        yield return new WaitForSeconds(t);
        a.SetTrigger(trigger);
    }


    //void SendCharacterScore()
    //{   
    //    for (int i = 0; i < GameManager.NumPlayer(); i++)
    //    {
    //        DataAnalytics.GetComponent<DataAnalytics>().SendData(GameManager.GetCharacter(i), playerScores[i]);
    //    }
    //    Debug.Log("finished sending score");
    //}

    // void printFinalText(bool blueWin){
    //     int mvp = 0;
    //     int highestScore = 0;
    //     for (int i = 0; i < GameManager.NumPlayer(); i++){
    //         //finalText[i].enabled = true;
    //         finalText[i].gameObject.SetActive(true);
    //         finalText[i].text = "P" + (i + 1).ToString() + " : " + playerScores[i].ToString() + " point(s)";
    //         finalText[i].color = PlayerBrain.GetColor(GameManager.GetPlayerObj(i + 1).GetComponent<PlayerBrain>().color);
    //         Debug.Log((int)GameManager.GetCharacter(i));
    //         //finalImages[i].enabled = true;
    //         finalImages[i].gameObject.SetActive(true);
    //         finalImages[i].sprite = characterSprites[(int)GameManager.GetCharacter(i)];
    //         if (highestScore < playerScores[i])
    //         {
    //             mvp = i;
    //             highestScore = playerScores[i];
    //         }
    //     }

    //     Vector3 mvpPos = mvpText.rectTransform.anchoredPosition3D;
    //     mvpPos.y = -75f * mvp + 112.5f;
    //     mvpText.rectTransform.anchoredPosition3D = mvpPos;
    //     //GetComponent<AudioSource>().Stop();
    //     //GetComponent<AudioSource>().PlayOneShot(winMusic);
    //     bgmManager.Win();
    // }

    //public Text finalBlueScore;
    //public Text finalRedScore;

    void printNewFinalText(bool blueWin)
    {
        //finalBlueScore.gameObject.SetActive(true);
        //finalRedScore.gameObject.SetActive(true);
        //finalBlueScore.text = "Team Blue: " + blue_score.ToString() + " Point(s)";
        //finalRedScore.text = "Team Red: " + red_score.ToString() + " Point(s)";
        //if (blueWin)
        //{
        //    finalRedScore.rectTransform.anchoredPosition3D = new Vector3(-680, -400, 0);
        //}
        //else
        //{
        //    finalBlueScore.rectTransform.anchoredPosition3D = new Vector3(680, 400, 0);
        //}
        int mvp = 0;
        int highestScore = 0;
        for (int i = 0; i < GameManager.NumPlayer(); i++){
            //finalText[i].enabled = true;
            finalText[i].gameObject.SetActive(true);
            Vector3 scorePosition = new Vector3 (aniPositions[i], -150f, 0);
            finalText[i].rectTransform.anchoredPosition3D = scorePosition;
            finalText[i].text = "P" + (i + 1).ToString() + " : " + playerScores[i].ToString() + " point(s)";
            // finalText[i].color = PlayerBrain.GetColor(GameManager.GetPlayerObj(i + 1).GetComponent<PlayerBrain>().color);
            finalText[i].color = UnityEngine.Color.white;
            // finalImages[i].gameObject.SetActive(true);
            // finalImages[i].sprite = characterSprites[(int)GameManager.GetCharacter(i)];
            int mvpCompeteScore = 0;
            if (GameManager.GetColor(i) == whoWin){
                mvpCompeteScore = playerScores[i];
            }
            if (highestScore < mvpCompeteScore)
            {
                mvp = i;
                highestScore = mvpCompeteScore;
            }
        }
        mvpText.gameObject.SetActive(true);
        Vector3 mvpPosition = new Vector3 (aniPositions[mvp], 250f, 0);
        mvpText.rectTransform.anchoredPosition3D = mvpPosition;
        mvpParticle.transform.position = new Vector3 (aniPositions[mvp]/70.0f, 3f, 0);
        mvpParticle.Play();
        //GetComponent<AudioSource>().Stop();
        //GetComponent<AudioSource>().PlayOneShot(winMusic);
    }


    private class finalAniInfo
    {
        public GameObject ani;
        public Vector3 des;

        public finalAniInfo(GameObject a, Vector3 d)
        {
            ani = a;
            des = d;
        }
    }
}

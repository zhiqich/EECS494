using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBrain : MonoBehaviour
{
    [HideInInspector] public enum Direction
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3
    };
    
    [HideInInspector] public Vector3[] DirectionVector = { new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f) };

    public enum Color
    {
        Blue = 0,
        Red = 1,
    }

    public int PlayerID = 0;

    // [HideInInspector] public Color color = Color.Blue;

    public Color color = Color.Blue;

    [HideInInspector] public Direction dir = Direction.Down;

    [HideInInspector] public bool stunned = false;

    public Rigidbody2D rb2d;

    [HideInInspector] public bool invincible = false;

    public bool freezed = true;
    [HideInInspector] public int health;
    public int maxHealth = 5;
    public float speed = 3;
    public int capacity = 3;
    public Slider slider;

    public Character character;
    public Text playerIDText;

    [HideInInspector] public bool moving = false;

    private void Awake()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerIDText.text = "P" + (PlayerID).ToString();
        if (color == PlayerBrain.Color.Blue){
            // playerIDText.color = UnityEngine.Color.blue;
            playerIDText.color = GameManager.gameBlue;
        }
        else{
            // playerIDText.color = UnityEngine.Color.red;
            playerIDText.color = GameManager.gameRed;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject stunnedStars;

    public void Stun(float sec)
    {
        StartCoroutine(stun(sec));
    }

    IEnumerator stun(float sec)
    {
        stunned = true;
        stunnedStars.SetActive(true);
        yield return new WaitForSeconds(sec);
        health = maxHealth;
        slider.value = health;
        invincible = true;
        stunned = false;
        stunnedStars.SetActive(false);
        StartCoroutine(noInvincible(1));
    }

    IEnumerator noInvincible(float sec)
    {
        yield return new WaitForSeconds(sec);
        invincible = false;
    }

    public void changePara(int newHealth, int newSpeed, int newCapacity){
        health += newHealth - maxHealth;
        maxHealth = newHealth;
        speed = newSpeed;
        capacity = newCapacity;
    }

    public GameObject indicator;

    public void SpecifyColor(Color c = Color.Blue)
    {
        color = c;
        if (color == Color.Blue)
        {
            // indicator.GetComponent<SpriteRenderer>().color = UnityEngine.Color.blue;
            // indicator.GetComponent<Image>().color =  UnityEngine.Color.blue;
            indicator.GetComponent<Image>().color =  GameManager.gameBlue;
        }
        else
        {
            // indicator.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
            // indicator.GetComponent<Image>().color =  UnityEngine.Color.red;
            indicator.GetComponent<Image>().color = GameManager.gameRed;
        }
    }

    public void Release()
    {
        freezed = false;
    }

    public Direction GetDirection()
    {
        return dir;
    }

    public void SetMoving(bool b)
    {
        moving = b;
    }

    public bool GetMoving()
    {
        return moving;
    }

    static public UnityEngine.Color GetColor(Color c)
    {
        if (c == Color.Blue)
        {
            // return UnityEngine.Color.blue;
            return GameManager.gameBlue;
        }
        else
        {
            return GameManager.gameRed;
            // return new Color(161f / 255f, 19f/255f , 64f/255f, 1);
        }
    }
}

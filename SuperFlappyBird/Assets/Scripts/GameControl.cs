using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public GameObject gameOverText;
    public Text scoreText;
    public Text scoreText2;
    public bool gameOver = false;
    public bool gameOver2 = false;
    public float scrollSpeed = -1.5f;

    private int score = 0;
    private int score2 = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == true && gameOver2 == true && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void BirdScored(int i)
    {
        if (i == 1)
        {
            if (gameOver)
            {
                return;
            }
            score++;
            scoreText.text = "P1 Score: " + score.ToString();
        }
        if (i == 2)
        {
            if (gameOver2)
            {
                return;
            }
            score2++;
            scoreText2.text = "P2 Score: " + score2.ToString();
        }
    }

    public void BirdDied(int i)
    {
        if (i == 1)
        {
            gameOver = true;
            if (gameOver2 == true)
            {
                gameOverText.SetActive(true);
            }
        }
        if (i == 2)
        {
            gameOver2 = true;
            if (gameOver == true)
            {
                gameOverText.SetActive(true);
            }
        }
    }
}

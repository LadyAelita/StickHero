using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public Text scoreText;
    public Text bestScoreText;

    public void UpdateScores(int currentScore)
    {
        float bestScore = PlayerPrefs.GetInt("HighScore");

        scoreText.text = currentScore.ToString();
        bestScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();

        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }

    public void DisplayOverlay()
    {
        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

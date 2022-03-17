using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    private int[] scores;
    private string[] names;
    public static ScoreKeeper current;
    private int score;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(current);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                current.score = 0;
                current.scoreText.text = "Score : " + score;
            }
            else
            {
                current.scoreText.text = " ";
                print("Setting Blank Scores;");
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                current.score = 0;
                current.scoreText.text = "Score : " + score;
            }
            else
            {
                current.scoreText.text = " ";
            }
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        score = 0;
    }

    public void ChangeScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score;
    }

    public void ResetScore()
    {
        current.score = 0;
        current.scoreText.text = "Score : " + score;
    }

}

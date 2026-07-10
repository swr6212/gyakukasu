using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;

    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreText();
        Debug.Log("ScoreManager‹N“®");
    }

    public void AddScore(int point)
    {
        score += point;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score : " + score;
    }
}
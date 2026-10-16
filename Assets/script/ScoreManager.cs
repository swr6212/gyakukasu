using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private TMP_Text scoreText;

    private int score = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーンを移動しても残す
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateScoreText();

        Debug.Log("ScoreManager起動");
    }

    public void SetScoreText(TMP_Text text)
    {
        scoreText = text;

        Debug.Log("ScoreTextを登録しました");

        UpdateScoreText();
    }

    public void AddScore(int point)
    {
        score += point;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score;
        }
    }

    public int GetScore()
    {
        return score;
    }
}
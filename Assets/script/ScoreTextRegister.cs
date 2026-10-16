using UnityEngine;
using TMPro;

public class ScoreTextRegister : MonoBehaviour
{
    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScoreText(
                GetComponent<TMP_Text>()
            );
        }
    }
}
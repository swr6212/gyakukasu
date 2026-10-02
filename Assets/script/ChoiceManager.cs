using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject gamblePanel;

    private PlayerStatus playerStatus;

    private void Awake()
    {
        playerStatus = FindFirstObjectByType<PlayerStatus>();
    }

    public void OpenChoice()
    {
        choicePanel.SetActive(true);
        gamblePanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void OpenGamble()
    {
        choicePanel.SetActive(false);
        gamblePanel.SetActive(true);
    }

    public void CloseGamble()
    {
        gamblePanel.SetActive(false);
        choicePanel.SetActive(true);
    }

    // パチンコボタン
    public void OpenPachinko()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PachinkoScene");
    }

    public void SelectPowerUp(int type)
    {
        switch (type)
        {
            case 0:
                playerStatus.IncreaseAttack(5f);
                break;

            case 1:
                playerStatus.IncreaseMoveSpeed(1f);
                break;

            case 2:
                playerStatus.IncreaseMaxHP(20f);
                break;
        }

        choicePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
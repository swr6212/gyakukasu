using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public TMP_Text moneyText;

    private int money = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        money += amount;

        Debug.Log("Ç®ã‡Çälìæ: " + amount);
        Debug.Log("åªç›ÇÃÇ®ã‡: " + money);

        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money : " + money;
    }
}
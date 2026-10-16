using UnityEngine;
using TMPro;

public class MoneyTextRegister : MonoBehaviour
{
    private void Start()
    {
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.SetMoneyText(
                GetComponent<TMP_Text>()
            );
        }
    }
}
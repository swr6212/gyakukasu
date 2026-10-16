using UnityEngine;

public class SlotReward : MonoBehaviour
{
    public enum RewardType
    {
        Attack,
        MoveSpeed,
        MaxHP,
        Money,
        Jackpot
    }

    [Header("報酬量")]
    [SerializeField] private float attackAmount = 15f;
    [SerializeField] private float moveSpeedAmount = 2f;
    [SerializeField] private float maxHPAmount = 30f;
    [SerializeField] private int moneyAmount = 500;

    [Header("JACKPOT報酬")]
    [SerializeField] private float jackpotAttack = 30f;
    [SerializeField] private float jackpotMoveSpeed = 3f;
    [SerializeField] private float jackpotMaxHP = 50f;
    [SerializeField] private int jackpotMoney = 1000;

    public void GiveReward(RewardType rewardType)
    {
        PlayerStatus playerStatus =
            FindFirstObjectByType<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogError("PlayerStatusが見つかりません！");
            return;
        }

        switch (rewardType)
        {
            case RewardType.Attack:

                playerStatus.IncreaseAttack(attackAmount);

                Debug.Log(
                    "スロット当たり！ 攻撃力 +" +
                    attackAmount
                );

                break;

            case RewardType.MoveSpeed:

                playerStatus.IncreaseMoveSpeed(moveSpeedAmount);

                Debug.Log(
                    "スロット当たり！ 移動速度 +" +
                    moveSpeedAmount
                );

                break;

            case RewardType.MaxHP:

                playerStatus.IncreaseMaxHP(maxHPAmount);

                Debug.Log(
                    "スロット当たり！ 最大HP +" +
                    maxHPAmount
                );

                break;

            case RewardType.Money:

                if (MoneyManager.Instance != null)
                {
                    MoneyManager.Instance.AddMoney(moneyAmount);

                    Debug.Log(
                        "スロット当たり！ お金 +" +
                        moneyAmount
                    );
                }
                else
                {
                    Debug.LogError("MoneyManagerが見つかりません！");
                }

                break;

            case RewardType.Jackpot:

                playerStatus.IncreaseAttack(jackpotAttack);
                playerStatus.IncreaseMoveSpeed(jackpotMoveSpeed);
                playerStatus.IncreaseMaxHP(jackpotMaxHP);

                if (MoneyManager.Instance != null)
                {
                    MoneyManager.Instance.AddMoney(jackpotMoney);
                }

                Debug.Log("★★★ JACKPOT！ ★★★");

                break;
        }
    }
}
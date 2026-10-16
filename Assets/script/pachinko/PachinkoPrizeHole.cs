using UnityEngine;

public class PachinkoPrizeHole : MonoBehaviour
{
    [Header("入賞設定")]
    [SerializeField] private int prizeIndex = 0;

    [Header("入賞時の報酬")]
    [SerializeField] private float attackBonus = 0f;
    [SerializeField] private float moveSpeedBonus = 0f;
    [SerializeField] private float maxHPBonus = 0f;

    [Header("お金・スコア")]
    [SerializeField] private int moneyReward = 0;
    [SerializeField] private int scoreReward = 0;

    [Header("一度の玉で一回だけ入賞")]
    [SerializeField] private bool oneTimeReward = true;

    // =========================
    // 入賞処理
    // =========================

    private void OnTriggerEnter(Collider other)
    {
        // パチンコ玉以外は無視
        if (!other.CompareTag("Ball"))
        {
            return;
        }

        Debug.Log(
            "入賞口 " +
            prizeIndex +
            " に玉が入りました！"
        );

        // =========================
        // PlayerStatus取得
        // =========================
        //
        // MainGameRootが非表示になっていても検索する
        //
        PlayerStatus playerStatus =
            FindFirstObjectByType<PlayerStatus>(
                FindObjectsInactive.Include
            );

        if (playerStatus == null)
        {
            Debug.LogError(
                "PlayerStatusが見つかりません！"
            );

            return;
        }

        // =========================
        // 通常強化
        // =========================

        if (attackBonus != 0f)
        {
            playerStatus.IncreaseAttack(
                attackBonus
            );

            Debug.Log(
                "攻撃力 +" +
                attackBonus
            );
        }

        if (moveSpeedBonus != 0f)
        {
            playerStatus.IncreaseMoveSpeed(
                moveSpeedBonus
            );

            Debug.Log(
                "移動速度 +" +
                moveSpeedBonus
            );
        }

        if (maxHPBonus != 0f)
        {
            playerStatus.IncreaseMaxHP(
                maxHPBonus
            );

            Debug.Log(
                "最大HP +" +
                maxHPBonus
            );
        }

        // =========================
        // お金
        // =========================

        if (moneyReward != 0)
        {
            if (MoneyManager.Instance != null)
            {
                MoneyManager.Instance.AddMoney(
                    moneyReward
                );

                Debug.Log(
                    "お金 +" +
                    moneyReward
                );
            }
            else
            {
                Debug.LogError(
                    "MoneyManagerが見つかりません！"
                );
            }
        }

        // =========================
        // スコア
        // =========================

        if (scoreReward != 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(
                    scoreReward
                );

                Debug.Log(
                    "スコア +" +
                    scoreReward
                );
            }
            else
            {
                Debug.LogError(
                    "ScoreManagerが見つかりません！"
                );
            }
        }

        // =========================
        // 入賞した玉を削除
        // =========================

        Destroy(other.gameObject);

        Debug.Log(
            "入賞口 " +
            prizeIndex +
            " の報酬処理完了！"
        );
    }
}

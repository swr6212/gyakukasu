using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("接触ダメージ")]
    [SerializeField] private float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("敵がぶつかった: " + collision.gameObject.name);

        // プレイヤー以外なら何もしない
        if (!collision.gameObject.CompareTag("Player1"))
        {
            return;
        }

        Debug.Log("プレイヤーに接触！");

        // 衝突したオブジェクトから親方向へPlayerStatusを探す
        PlayerStatus playerStatus =
            collision.gameObject.GetComponentInParent<PlayerStatus>();

        if (playerStatus != null)
        {
            playerStatus.TakeDamage(damage);

            Debug.Log(
                "プレイヤーに " + damage +
                " ダメージ！ 現在HP: " +
                playerStatus.CurrentHP
            );
        }
        else
        {
            Debug.LogError("PlayerStatusが見つかりません！");
        }
    }
}
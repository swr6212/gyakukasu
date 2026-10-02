using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("接触ダメージ")]
    [SerializeField] private float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("何かにぶつかった: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player1"))
        {
            Debug.Log("プレイヤーに接触！");

            PlayerStatus playerStatus = collision.gameObject.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage);
                Debug.Log("プレイヤーに " + damage + " ダメージ！");
            }
            else
            {
                Debug.LogError("PlayerStatusが見つかりません！");
            }
        }
    }
}
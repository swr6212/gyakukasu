using UnityEngine;
using UnityEngine.UI;

public class ObjectDropper : MonoBehaviour
{
    [Header("玉")]
    [SerializeField] private GameObject objectPrefab;

    [Header("玉の生成位置")]
    [SerializeField] private Transform spawnPoint;

    [Header("発射回数")]
    [SerializeField] private int maxShots = 5;

    [Header("発射ボタン")]
    [SerializeField] private Button dropButton;

    private int currentShots;

    private void Start()
    {
        // 5発からスタート
        currentShots = maxShots;

        UpdateButton();
    }

    // ボタンのOnClickから呼ぶ
    public void DropObject()
    {
        // 発射回数が残っていない
        if (currentShots <= 0)
        {
            Debug.Log("発射できる玉がありません！");
            return;
        }

        // パチンコ中は時間を動かす
        Time.timeScale = 1f;

        if (objectPrefab == null)
        {
            Debug.LogError("objectPrefabが設定されていません！");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("spawnPointが設定されていません！");
            return;
        }

        // 玉を生成
        GameObject obj = Instantiate(
            objectPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // Rigidbody取得
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("生成した玉にRigidbodyがありません！");
            Destroy(obj);
            return;
        }

        // 物理演算を有効化
        rb.isKinematic = false;
        rb.useGravity = true;

        // 初期速度をリセット
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 発射回数を1減らす
        currentShots--;

        Debug.Log("玉生成！");
        Debug.Log("残り発射回数: " + currentShots);
        Debug.Log("TimeScale = " + Time.timeScale);
        Debug.Log("Gravity = " + rb.useGravity);
        Debug.Log("Kinematic = " + rb.isKinematic);
        Debug.Log("生成位置 = " + obj.transform.position);

        // 5回撃ったらボタンを押せなくする
        UpdateButton();
    }

    private void UpdateButton()
    {
        if (dropButton != null)
        {
            dropButton.interactable = currentShots > 0;
        }
    }
}
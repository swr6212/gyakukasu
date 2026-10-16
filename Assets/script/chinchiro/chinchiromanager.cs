using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChinchiroManager : MonoBehaviour
{
    [Header("サイコロ設定")]
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform diceSpawnPoint;
    [SerializeField] private int diceCount = 3;
    [SerializeField] private float spawnSpacing = 0.8f;

    [Header("サイコロの振り方")]
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private float torqueForce = 10f;

    [Header("UI")]
    [SerializeField] private Button rollButton;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text moneyText;

    [Header("報酬")]
    [SerializeField] private int pinzoroReward = 5000;
    [SerializeField] private int tripleReward = 2000;
    [SerializeField] private int shigoroReward = 1500;
    [SerializeField] private int pairRewardPerPip = 200;

    private GameObject[] spawnedDice;
    private bool isRolling;

    private void Start()
    {
        // チンチロはサイコロ3個固定
        diceCount = 3;
        spawnedDice = new GameObject[diceCount];

        if (rollButton != null)
            rollButton.onClick.AddListener(RollDice);

        if (resultText != null)
            resultText.text = "サイコロを振ろう！";

        UpdateMoneyUI();

        Debug.Log("チンチロマネージャー起動");
    }

    public void RollDice()
    {
        if (isRolling)
            return;

        if (dicePrefab == null || diceSpawnPoint == null)
        {
            Debug.LogError(
                "サイコロPrefabまたはDice Spawn Pointが未設定です！");
            return;
        }

        if (rollButton != null)
            rollButton.interactable = false;

        StartCoroutine(RollDiceCoroutine());
    }

    private IEnumerator RollDiceCoroutine()
    {
        isRolling = true;

        if (resultText != null)
            resultText.text = "チンチロ中...";

        // 前回のサイコロを削除
        for (int i = 0; i < spawnedDice.Length; i++)
        {
            if (spawnedDice[i] != null)
                Destroy(spawnedDice[i]);
        }

        yield return new WaitForFixedUpdate();

        spawnedDice = new GameObject[3];

        Debug.Log("===== チンチロ開始 =====");

        // サイコロ3個を生成して振る
        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = new Vector3(
                (i - 1) * spawnSpacing,
                0f,
                0f
            );

            Vector3 spawnPosition =
                diceSpawnPoint.position + offset;

            GameObject die = Instantiate(
                dicePrefab,
                spawnPosition,
                Random.rotation
            );

            spawnedDice[i] = die;

            Rigidbody rb = die.GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogError(
                    $"サイコロ{i + 1}にRigidbodyがありません！");
                Destroy(die);
                spawnedDice[i] = null;
                continue;
            }

            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 force = new Vector3(
                Random.Range(-1f, 1f),
                1f,
                Random.Range(-1f, 1f)
            ).normalized * throwForce;

            rb.AddForce(force, ForceMode.Impulse);

            Vector3 torque =
                Random.insideUnitSphere * torqueForce;

            rb.AddTorque(torque, ForceMode.Impulse);

            Debug.Log($"サイコロ{i + 1}を生成しました");
        }

        // サイコロが止まるまで待つ
        float elapsed = 0f;
        float stoppedTime = 0f;

        while (elapsed < 10f)
        {
            elapsed += Time.deltaTime;

            bool allStopped = true;

            foreach (GameObject die in spawnedDice)
            {
                if (die == null)
                {
                    allStopped = false;
                    break;
                }

                Rigidbody rb = die.GetComponent<Rigidbody>();

                if (rb != null &&
                    (rb.linearVelocity.sqrMagnitude > 0.01f ||
                     rb.angularVelocity.sqrMagnitude > 0.01f))
                {
                    allStopped = false;
                    break;
                }
            }

            if (allStopped)
            {
                stoppedTime += Time.deltaTime;

                if (stoppedTime >= 0.5f)
                    break;
            }
            else
            {
                stoppedTime = 0f;
            }

            yield return null;
        }

        // 出目を取得してConsoleに表示
        int[] values = new int[3];

        for (int i = 0; i < 3; i++)
        {
            if (spawnedDice[i] == null)
            {
                Debug.LogError(
                    $"サイコロ{i + 1}が存在しません。出目を取得できません！");
                values[i] = 0;
                continue;
            }

            ChinchiroDie die =
                spawnedDice[i].GetComponent<ChinchiroDie>();

            if (die == null)
            {
                Debug.LogError(
                    $"サイコロ{i + 1}にChinchiroDieがありません！");
                values[i] = 0;
                continue;
            }

            values[i] = die.GetTopFaceValue();

            Debug.Log($"サイコロ{i + 1}の出目：{values[i]}");
        }

        // 出目が取得できなかった場合は報酬判定しない
        if (values[0] == 0 || values[1] == 0 || values[2] == 0)
        {
            if (resultText != null)
                resultText.text = "出目の取得に失敗しました。Consoleを確認してください。";

            Debug.LogError("チンチロの役判定を中止しました。");

            FinishRolling();
            yield break;
        }

        string handName;
        int reward = JudgeHand(values, out handName);

        Debug.Log($"【チンチロ結果】出目：{values[0]}・{values[1]}・{values[2]}");
        Debug.Log($"【チンチロ結果】役：{handName}");
        Debug.Log($"【チンチロ結果】報酬：{reward}円");

        if (MoneyManager.Instance != null && reward > 0)
        {
            MoneyManager.Instance.AddMoney(reward);
            Debug.Log($"所持金に{reward}円を加算しました");
        }
        else if (MoneyManager.Instance == null)
        {
            Debug.LogError("MoneyManager.Instanceが見つかりません！");
        }

        if (resultText != null)
        {
            resultText.text =
                $"出目：{values[0]}・{values[1]}・{values[2]}\n" +
                $"役：{handName}\n" +
                (reward > 0
                    ? $"獲得金額：+{reward}円"
                    : "獲得金額：0円");
        }

        UpdateMoneyUI();
        FinishRolling();
    }

    private int JudgeHand(int[] values, out string handName)
    {
        int a = values[0];
        int b = values[1];
        int c = values[2];

        if (a == 1 && b == 1 && c == 1)
        {
            handName = "ピンゾロ！";
            return pinzoroReward;
        }

        if (a == b && b == c)
        {
            handName = "ゾロ目！";
            return tripleReward;
        }

        if (IsCombination(values, 4, 5, 6))
        {
            handName = "シゴロ！";
            return shigoroReward;
        }

        if (IsCombination(values, 1, 2, 3))
        {
            handName = "ヒフミ";
            return 0;
        }

        if (a == b)
        {
            handName = $"{c}の目";
            return c * pairRewardPerPip;
        }

        if (a == c)
        {
            handName = $"{b}の目";
            return b * pairRewardPerPip;
        }

        if (b == c)
        {
            handName = $"{a}の目";
            return a * pairRewardPerPip;
        }

        handName = "役なし";
        return 0;
    }

    private bool IsCombination(int[] values, int x, int y, int z)
    {
        int[] sorted = (int[])values.Clone();
        System.Array.Sort(sorted);

        return sorted[0] == x &&
               sorted[1] == y &&
               sorted[2] == z;
    }

    private void UpdateMoneyUI()
    {
        if (moneyText == null)
            return;

        if (MoneyManager.Instance != null)
        {
            moneyText.text =
                $"所持金：{MoneyManager.Instance.GetMoney()}円";
        }
        else
        {
            moneyText.text = "所持金：取得できません";
        }
    }

    private void FinishRolling()
    {
        isRolling = false;

        if (rollButton != null)
            rollButton.interactable = true;
    }

    private void OnDestroy()
    {
        if (rollButton != null)
            rollButton.onClick.RemoveListener(RollDice);
    }
}
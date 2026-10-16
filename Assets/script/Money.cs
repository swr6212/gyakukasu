using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [Header("所持金")]
    [SerializeField] private int money = 0;

    private TMP_Text moneyText;

    private void Awake()
    {
        // すでに別のMoneyManagerが存在する場合
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(
                $"MoneyManagerが重複しています。破棄対象：{gameObject.name}");

            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーンをまたいでも所持金を保持
        DontDestroyOnLoad(gameObject);

        Debug.Log(
            $"MoneyManager初期化成功！ 所持金：{money}円 / Instance：{Instance.name}");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 自分が現在のInstanceなら解除
        if (Instance == this)
        {
            Instance = null;
            Debug.Log("MoneyManagerが破棄されました");
        }
    }

    private void Start()
    {
        UpdateMoneyText();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(
            $"シーン読み込み：{scene.name} / MoneyManager：{(Instance != null ? Instance.name : "null")}");

        UpdateMoneyText();
    }

    // UIのMoneyTextを登録
    public void SetMoneyText(TMP_Text text)
    {
        moneyText = text;

        Debug.Log("MoneyTextを登録しました");

        UpdateMoneyText();
    }

    // お金を増やす
    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("AddMoneyには0以上の値を指定してください");
            return;
        }

        money += amount;

        Debug.Log($"増えたお金：{amount}円");
        Debug.Log($"現在のお金：{money}円");

        UpdateMoneyText();
    }

    // お金を使う
    public bool SpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("SpendMoneyには0以上の値を指定してください");
            return false;
        }

        if (money < amount)
        {
            Debug.Log("お金が足りません");
            return false;
        }

        money -= amount;

        Debug.Log($"使ったお金：{amount}円");
        Debug.Log($"現在のお金：{money}円");

        UpdateMoneyText();

        return true;
    }

    // 現在の所持金を取得
    public int GetMoney()
    {
        return money;
    }

    // UI表示を更新
    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money : " + money;
        }
    }
}
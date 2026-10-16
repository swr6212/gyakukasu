
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SlotManager : MonoBehaviour
{
    [Header("リール")]
    [SerializeField] private Image reel1Image;
    [SerializeField] private Image reel2Image;
    [SerializeField] private Image reel3Image;

    [Header("シンボル画像（5種類以上）")]
    [SerializeField] private Texture2D[] symbols;

    [Header("当たり確率")]
    [Range(0f, 100f)]
    [SerializeField] private float winRate = 25f;

    [Header("リール回転時間")]
    [SerializeField] private float reel1Time = 1.5f;
    [SerializeField] private float reel2Time = 2.0f;
    [SerializeField] private float reel3Time = 2.5f;

    [Header("図柄切り替え間隔")]
    [SerializeField] private float changeInterval = 0.08f;

    [Header("スピンボタン")]
    [SerializeField] private Button spinButton;

    [Header("プレイヤー")]
    [SerializeField] private PlayerStatus playerStatus;

    [Header("通常当たりの報酬")]
    [SerializeField] private float attackReward = 15f;
    [SerializeField] private float moveSpeedReward = 2f;
    [SerializeField] private float maxHPReward = 30f;
    [SerializeField] private int moneyReward = 500;

    [Header("JACKPOT報酬")]
    [SerializeField] private float jackpotAttack = 30f;
    [SerializeField] private float jackpotMoveSpeed = 3f;
    [SerializeField] private float jackpotMaxHP = 50f;
    [SerializeField] private int jackpotMoney = 1000;

    private Sprite[] symbolSprites;

    private bool isSpinning = false;

    private int result1 = -1;
    private int result2 = -1;
    private int result3 = -1;

    private int stoppedReels = 0;

    private void Awake()
    {
        CreateSprites();
        RefreshPlayerStatus();
    }

    // ========================================
    // PlayerStatus取得
    // ========================================

    private bool RefreshPlayerStatus()
    {
        // すでに参照がある場合は、そのまま使用する
        if (playerStatus != null)
        {
            return true;
        }

        // 非アクティブなオブジェクトも含めて検索
        playerStatus =
            FindFirstObjectByType<PlayerStatus>(
                FindObjectsInactive.Include
            );

        // 通常検索で見つからなかった場合の予備検索
        if (playerStatus == null)
        {
            PlayerStatus[] allPlayers =
                Resources.FindObjectsOfTypeAll<PlayerStatus>();

            foreach (PlayerStatus candidate in allPlayers)
            {
                if (candidate == null)
                {
                    continue;
                }

                // Prefabアセットなどを除外し、
                // 読み込み済みシーンのオブジェクトだけを対象にする
                Scene scene = candidate.gameObject.scene;

                if (scene.IsValid() && scene.isLoaded)
                {
                    playerStatus = candidate;
                    break;
                }
            }
        }

        if (playerStatus == null)
        {
            Debug.LogError(
                "PlayerStatusが見つかりません！\n" +
                "MainSceneが読み込まれているか、" +
                "PlayerにPlayerStatusが付いているか確認してください。",
                this
            );

            return false;
        }

        Debug.Log(
            "PlayerStatusを取得しました: " +
            playerStatus.gameObject.name,
            this
        );

        return true;
    }

    // ========================================
    // シンボル画像からSpriteを作成
    // ========================================

    private void CreateSprites()
    {
        if (symbols == null || symbols.Length < 5)
        {
            Debug.LogError(
                "シンボル画像を5種類以上設定してください！",
                this
            );
            return;
        }

        symbolSprites = new Sprite[symbols.Length];

        for (int i = 0; i < symbols.Length; i++)
        {
            if (symbols[i] == null)
            {
                Debug.LogError(
                    "Symbols[" + i + "] が設定されていません！",
                    this
                );
                symbolSprites = null;
                return;
            }

            symbolSprites[i] = Sprite.Create(
                symbols[i],
                new Rect(
                    0,
                    0,
                    symbols[i].width,
                    symbols[i].height
                ),
                new Vector2(0.5f, 0.5f)
            );
        }
    }

    // ========================================
    // スピン開始
    // ========================================

    public void Spin()
    {
        if (isSpinning)
        {
            return;
        }

        if (symbolSprites == null || symbolSprites.Length < 5)
        {
            Debug.LogError(
                "シンボル画像を5種類以上設定してください！",
                this
            );
            return;
        }

        if (reel1Image == null ||
            reel2Image == null ||
            reel3Image == null)
        {
            Debug.LogError(
                "リールのImageが設定されていません！",
                this
            );
            return;
        }

        isSpinning = true;
        stoppedReels = 0;

        result1 = -1;
        result2 = -1;
        result3 = -1;

        if (spinButton != null)
        {
            spinButton.interactable = false;
        }

        // 当たりかハズレかを抽選
        bool isWin = Random.Range(0f, 100f) < winRate;

        Debug.Log(
            "今回の抽選結果: " + (isWin ? "当たり" : "ハズレ")
        );

        SetFinalResults(isWin);

        StartCoroutine(
            SpinReel(reel1Image, reel1Time, 1, result1)
        );

        StartCoroutine(
            SpinReel(reel2Image, reel2Time, 2, result2)
        );

        StartCoroutine(
            SpinReel(reel3Image, reel3Time, 3, result3)
        );

        StartCoroutine(WaitForAllReelsStopped());
    }

    // ========================================
    // 最終結果を決める
    // ========================================

    private void SetFinalResults(bool isWin)
    {
        if (isWin)
        {
            // 当たり：3つすべて同じ図柄
            int winSymbol = Random.Range(0, symbolSprites.Length);

            result1 = winSymbol;
            result2 = winSymbol;
            result3 = winSymbol;

            Debug.Log("当たり図柄 = " + winSymbol);
        }
        else
        {
            // ハズレ：ランダムに図柄を決定
            result1 = Random.Range(0, symbolSprites.Length);
            result2 = Random.Range(0, symbolSprites.Length);
            result3 = Random.Range(0, symbolSprites.Length);

            // 3つ揃った場合は、3つ目を必ず変更する
            if (result1 == result2 && result2 == result3)
            {
                result3 = (result3 + 1) % symbolSprites.Length;
            }

            Debug.Log(
                "ハズレ結果 = " +
                result1 + " | " +
                result2 + " | " +
                result3
            );
        }
    }

    // ========================================
    // リール回転
    // ========================================

    private IEnumerator SpinReel(
        Image reelImage,
        float spinTime,
        int reelNumber,
        int finalResult)
    {
        float timer = 0f;

        while (timer < spinTime)
        {
            int randomIndex =
                Random.Range(0, symbolSprites.Length);

            reelImage.sprite = symbolSprites[randomIndex];

            timer += changeInterval;

            // Time.timeScaleが0でも回転できる
            yield return new WaitForSecondsRealtime(changeInterval);
        }

        // 最終図柄を表示
        reelImage.sprite = symbolSprites[finalResult];

        stoppedReels++;

        Debug.Log("リール" + reelNumber + "停止！");
    }

    // ========================================
    // 全リール停止待ち
    // ========================================

    private IEnumerator WaitForAllReelsStopped()
    {
        yield return new WaitUntil(() => stoppedReels >= 3);

        Debug.Log(
            "最終結果: " +
            result1 + " | " +
            result2 + " | " +
            result3
        );

        CheckResult();

        isSpinning = false;

        if (spinButton != null)
        {
            spinButton.interactable = true;
        }
    }

    // ========================================
    // 結果判定
    // ========================================

    private void CheckResult()
    {
        // 3つ揃っていなければハズレ
        if (result1 != result2 || result2 != result3)
        {
            Debug.Log("ハズレ！報酬はありません。");
            return;
        }

        Debug.Log("★★★ 3つ揃った！ ★★★");

        switch (result1)
        {
            case 0:
                GiveAttackReward();
                break;

            case 1:
                GiveMoveSpeedReward();
                break;

            case 2:
                GiveMaxHPReward();
                break;

            case 3:
                GiveMoneyReward();
                break;

            case 4:
                GiveJackpotReward();
                break;

            default:
                Debug.LogError(
                    "存在しない図柄番号です: " + result1,
                    this
                );
                break;
        }
    }

    // ========================================
    // 攻撃力報酬
    // ========================================

    private void GiveAttackReward()
    {
        if (!RefreshPlayerStatus())
        {
            return;
        }

        playerStatus.IncreaseAttack(attackReward);

        Debug.Log("攻撃力 +" + attackReward);
    }

    // ========================================
    // 移動速度報酬
    // ========================================

    private void GiveMoveSpeedReward()
    {
        if (!RefreshPlayerStatus())
        {
            return;
        }

        playerStatus.IncreaseMoveSpeed(moveSpeedReward);

        Debug.Log("移動速度 +" + moveSpeedReward);
    }

    // ========================================
    // 最大HP報酬
    // ========================================

    private void GiveMaxHPReward()
    {
        if (!RefreshPlayerStatus())
        {
            return;
        }

        playerStatus.IncreaseMaxHP(maxHPReward);

        Debug.Log("最大HP +" + maxHPReward);
    }

    // ========================================
    // お金報酬
    // ========================================

    private void GiveMoneyReward()
    {
        if (MoneyManager.Instance == null)
        {
            Debug.LogError(
                "MoneyManagerが見つかりません！",
                this
            );
            return;
        }

        MoneyManager.Instance.AddMoney(moneyReward);

        Debug.Log("お金 +" + moneyReward);
    }

    // ========================================
    // JACKPOT報酬
    // ========================================

    private void GiveJackpotReward()
    {
        if (!RefreshPlayerStatus())
        {
            return;
        }

        // 攻撃力・移動速度・最大HPを強化
        playerStatus.IncreaseAttack(jackpotAttack);
        playerStatus.IncreaseMoveSpeed(jackpotMoveSpeed);
        playerStatus.IncreaseMaxHP(jackpotMaxHP);

        // お金も追加
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(jackpotMoney);
        }
        else
        {
            Debug.LogError(
                "MoneyManagerが見つからないため、" +
                "JACKPOTのお金報酬を付与できません。",
                this
            );
        }

        Debug.Log("★★★★ JACKPOT！ ★★★★");
        Debug.Log("攻撃力 +" + jackpotAttack);
        Debug.Log("移動速度 +" + jackpotMoveSpeed);
        Debug.Log("最大HP +" + jackpotMaxHP);
        Debug.Log("お金 +" + jackpotMoney);
    }

    private void OnDestroy()
    {
        if (symbolSprites == null)
        {
            return;
        }

        for (int i = 0; i < symbolSprites.Length; i++)
        {
            if (symbolSprites[i] != null)
            {
                Destroy(symbolSprites[i]);
            }
        }
    }
}

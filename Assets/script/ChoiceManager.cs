
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChoiceManager : MonoBehaviour
{
    [Header("強化画面")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject gamblePanel;

    [Header("メインUI")]
    [SerializeField] private GameObject mainUI;

    [Header("メインゲーム本体")]
    [SerializeField] private GameObject mainGameRoot;

    [Header("ギャンブル設定")]
    [SerializeField] private int gambleCost = 500;

    private PlayerStatus playerStatus;
    private bool isChangingScene = false;

    private void Awake()
    {
        playerStatus = FindFirstObjectByType<PlayerStatus>(
            FindObjectsInactive.Include
        );
    }

    // =========================
    // 強化画面
    // =========================

    public void OpenChoice()
    {
        if (choicePanel != null)
            choicePanel.SetActive(true);

        if (gamblePanel != null)
            gamblePanel.SetActive(false);

        Time.timeScale = 0f;
    }

    public void OpenGamble()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (gamblePanel != null)
            gamblePanel.SetActive(true);

        Debug.Log("ギャンブルパネルを開きました");
    }

    public void CloseGamble()
    {
        if (gamblePanel != null)
            gamblePanel.SetActive(false);

        if (choicePanel != null)
            choicePanel.SetActive(true);
    }

    // =========================
    // 通常強化
    // =========================

    public void SelectPowerUp(int type)
    {
        if (playerStatus == null)
        {
            playerStatus = FindFirstObjectByType<PlayerStatus>(
                FindObjectsInactive.Include
            );
        }

        if (playerStatus == null)
        {
            Debug.LogError("PlayerStatusが見つかりません！");
            return;
        }

        switch (type)
        {
            case 0:
                playerStatus.IncreaseAttack(5f);
                Debug.Log("攻撃力 +5");
                break;

            case 1:
                playerStatus.IncreaseMoveSpeed(1f);
                Debug.Log("移動速度 +1");
                break;

            case 2:
                playerStatus.IncreaseMaxHP(20f);
                Debug.Log("最大HP +20");
                break;

            default:
                Debug.LogError("不明な強化タイプです: " + type);
                return;
        }

        if (choicePanel != null)
            choicePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    // =========================
    // パチンコ開始
    // =========================

    public void OpenPachinko()
    {
        if (isChangingScene)
            return;

        Debug.Log("パチンコ開始処理");

        if (!PayGambleCost())
            return;

        isChangingScene = true;

        PauseMainGame();
        HideMainUI();

        if (gamblePanel != null)
            gamblePanel.SetActive(false);

        StartCoroutine(LoadPachinko());
    }

    private IEnumerator LoadPachinko()
    {
        Time.timeScale = 1f;

        AsyncOperation load = SceneManager.LoadSceneAsync(
            "PachinkoScene",
            LoadSceneMode.Additive
        );

        if (load == null)
        {
            Debug.LogError("PachinkoSceneの読み込みを開始できません！");
            ReturnToMainObjects();
            isChangingScene = false;
            yield break;
        }

        while (!load.isDone)
            yield return null;

        Scene scene = SceneManager.GetSceneByName("PachinkoScene");

        if (scene.IsValid() && scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
            Debug.Log("PachinkoSceneを開きました");
        }
        else
        {
            Debug.LogError("PachinkoSceneが見つかりません！");
            yield return StartCoroutine(ReturnToMainScene());
        }

        Time.timeScale = 1f;
        isChangingScene = false;
    }

    // =========================
    // パチンコ終了
    // =========================

    public void ClosePachinko()
    {
        if (isChangingScene)
            return;

        isChangingScene = true;
        Debug.Log("パチンコ終了開始");

        StartCoroutine(ClosePachinkoCoroutine());
    }

    private IEnumerator ClosePachinkoCoroutine()
    {
        yield return StartCoroutine(UnloadGambleScene("PachinkoScene"));
        yield return StartCoroutine(ReturnToMainScene());

        isChangingScene = false;
        Debug.Log("パチンコ終了！");
    }

    // =========================
    // スロット開始
    // =========================

    public void OpenSlot()
    {
        if (isChangingScene)
            return;

        Debug.Log("スロット開始処理");

        if (!PayGambleCost())
            return;

        isChangingScene = true;

        PauseMainGame();
        HideMainUI();

        if (gamblePanel != null)
            gamblePanel.SetActive(false);

        StartCoroutine(LoadSlot());
    }

    private IEnumerator LoadSlot()
    {
        Time.timeScale = 1f;

        AsyncOperation load = SceneManager.LoadSceneAsync(
            "SlotScene",
            LoadSceneMode.Additive
        );

        if (load == null)
        {
            Debug.LogError("SlotSceneの読み込みを開始できません！");
            ReturnToMainObjects();
            isChangingScene = false;
            yield break;
        }

        while (!load.isDone)
            yield return null;

        Scene scene = SceneManager.GetSceneByName("SlotScene");

        if (scene.IsValid() && scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
            Debug.Log("SlotSceneを開きました");
        }
        else
        {
            Debug.LogError("SlotSceneが見つかりません！");
            yield return StartCoroutine(ReturnToMainScene());
        }

        Time.timeScale = 1f;
        isChangingScene = false;
    }

    // =========================
    // スロット終了
    // =========================

    public void CloseSlot()
    {
        if (isChangingScene)
            return;

        isChangingScene = true;
        Debug.Log("スロット終了開始");

        StartCoroutine(CloseSlotCoroutine());
    }

    private IEnumerator CloseSlotCoroutine()
    {
        yield return StartCoroutine(UnloadGambleScene("SlotScene"));
        yield return StartCoroutine(ReturnToMainScene());

        isChangingScene = false;
        Debug.Log("スロット終了！");
    }

    // =========================
    // チンチロ開始
    // =========================

    public void OpenChinchiro()
    {
        if (isChangingScene)
            return;

        Debug.Log("チンチロ開始処理");

        if (!PayGambleCost())
            return;

        isChangingScene = true;

        PauseMainGame();
        HideMainUI();

        if (gamblePanel != null)
            gamblePanel.SetActive(false);

        StartCoroutine(LoadChinchiro());
    }

    private IEnumerator LoadChinchiro()
    {
        Time.timeScale = 1f;

        AsyncOperation load = SceneManager.LoadSceneAsync(
            "ChinchiroScene",
            LoadSceneMode.Additive
        );

        if (load == null)
        {
            Debug.LogError("ChinchiroSceneの読み込みを開始できません！");
            ReturnToMainObjects();
            isChangingScene = false;
            yield break;
        }

        while (!load.isDone)
            yield return null;

        Scene scene = SceneManager.GetSceneByName("ChinchiroScene");

        if (scene.IsValid() && scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
            Debug.Log("ChinchiroSceneを開きました");
        }
        else
        {
            Debug.LogError("ChinchiroSceneが見つかりません！");
            yield return StartCoroutine(ReturnToMainScene());
        }

        Time.timeScale = 1f;
        isChangingScene = false;
    }

    // =========================
    // チンチロ終了
    // =========================

    public void CloseChinchiro()
    {
        if (isChangingScene)
            return;

        isChangingScene = true;
        Debug.Log("チンチロ終了開始");

        StartCoroutine(CloseChinchiroCoroutine());
    }

    private IEnumerator CloseChinchiroCoroutine()
    {
        yield return StartCoroutine(UnloadGambleScene("ChinchiroScene"));
        yield return StartCoroutine(ReturnToMainScene());

        isChangingScene = false;
        Debug.Log("チンチロ終了！");
    }

    // =========================
    // ギャンブルシーン共通アンロード
    // =========================

    private IEnumerator UnloadGambleScene(string sceneName)
    {
        Time.timeScale = 1f;

        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogWarning(sceneName + " は読み込まれていません。");
            yield break;
        }

        AsyncOperation unload = SceneManager.UnloadSceneAsync(scene);

        if (unload != null)
        {
            while (!unload.isDone)
                yield return null;
        }

        Debug.Log(sceneName + " のアンロードが完了しました");
    }

    // =========================
    // 共通処理
    // =========================

    private void PauseMainGame()
    {
        if (mainGameRoot != null)
        {
            mainGameRoot.SetActive(false);
            Debug.Log("メインゲームを停止しました");
        }
        else
        {
            Debug.LogError("mainGameRootが設定されていません！");
        }
    }

    private void ResumeMainGame()
    {
        if (mainGameRoot != null)
        {
            mainGameRoot.SetActive(true);
            Debug.Log("メインゲームを再開しました");
        }
        else
        {
            Debug.LogError("mainGameRootが設定されていません！");
        }
    }

    private bool PayGambleCost()
    {
        if (MoneyManager.Instance == null)
        {
            Debug.LogError("MoneyManagerが見つかりません！");
            return false;
        }

        if (!MoneyManager.Instance.SpendMoney(gambleCost))
        {
            Debug.Log(
                "お金が足りません！ 必要金額: " +
                gambleCost +
                " / 所持金: " +
                MoneyManager.Instance.GetMoney()
            );

            return false;
        }

        Debug.Log("ギャンブル料金 " + gambleCost + "円を支払いました");
        return true;
    }

    private void HideMainUI()
    {
        if (mainUI != null)
        {
            mainUI.SetActive(false);
            Debug.Log("メインUIを非表示にしました");
        }
        else
        {
            Debug.LogError("mainUIが設定されていません！");
        }
    }

    private void ShowMainUI()
    {
        if (mainUI != null)
        {
            mainUI.SetActive(true);
            Debug.Log("メインUIを再表示しました");
        }
        else
        {
            Debug.LogError("mainUIが設定されていません！");
        }
    }

    private void ReturnToMainObjects()
    {
        Scene mainScene = SceneManager.GetSceneByName("MainScene");

        if (mainScene.IsValid() && mainScene.isLoaded)
            SceneManager.SetActiveScene(mainScene);

        ResumeMainGame();
        ShowMainUI();

        Time.timeScale = 1f;
    }

    // =========================
    // MainSceneへ戻る
    // =========================

    private IEnumerator ReturnToMainScene()
    {
        Scene mainScene = SceneManager.GetSceneByName("MainScene");

        if (!mainScene.IsValid() || !mainScene.isLoaded)
        {
            Debug.LogError("MainSceneが見つかりません！");
            yield break;
        }

        SceneManager.SetActiveScene(mainScene);
        Debug.Log("MainSceneに戻りました");

        ResumeMainGame();
        ShowMainUI();

        Time.timeScale = 1f;

        yield return null;

        Debug.Log("メインゲームを再開しました");
    }
}

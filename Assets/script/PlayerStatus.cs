using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    [Header("基本ステータス")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private float moveSpeed = 5f;

    private float currentHP;

    [Header("HP UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float AttackPower => attackPower;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        UpdateHPUI();
    }

    // 攻撃力アップ
    public void IncreaseAttack(float amount)
    {
        attackPower += amount;
    }

    // 移動速度アップ
    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    // 最大HPアップ
    public void IncreaseMaxHP(float amount)
    {
        maxHP += amount;

        // 最大HPが増えた分、現在HPも増やす
        currentHP += amount;

        UpdateHPUI();
    }

    // ダメージ
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        UpdateHPUI();
    }

    // 回復
    public void Heal(float amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        UpdateHPUI();
    }

    // HP UI更新
    private void UpdateHPUI()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        if (hpText != null)
        {
            hpText.text = $"{currentHP:0} / {maxHP:0}";
        }
    }

    private void Die()
    {
        Debug.Log("プレイヤー死亡");
    }
}
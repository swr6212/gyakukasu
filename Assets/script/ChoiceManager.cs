using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public void SelectPowerUp(int type)
    {
        switch(type)
        {
            case 0:
                Debug.Log("攻撃力アップ");
                break;

            case 1:
                Debug.Log("移動速度アップ");
                break;

            case 2:
                Debug.Log("最大HPアップ");
                break;
        }

        Time.timeScale = 1f;

        gameObject.SetActive(false);
    }
}
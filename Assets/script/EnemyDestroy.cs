using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public int maxHits = 5;
    public int currentHits = 0;
    public int point = 100;

    public GameObject dropObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Range(0, 100)]
    public float dropChance = 12.5f;
    private void OnCollisionEnter(Collision collision) //ぶつかったら消える命令文開始
    {
        Debug.Log("何かに当たった: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Bullet"))

        {
            currentHits++;
            if (currentHits >= maxHits)
            {
                ScoreManager.Instance.AddScore(point);
                DropItem();
                Destroy(gameObject);//このゲームオブジェクトを消滅させる
            }
        }

    }
    void DropItem()
    {
        float random = Random.Range(0, 100);

        if (random < dropChance)
        {
            Instantiate(
                dropObject,
                transform.position,
                Quaternion.identity
                );

            Debug.Log("アイテムドロップ");
        }
        else
        {
            Debug.Log("ドロップせんで");
        }
    }
}

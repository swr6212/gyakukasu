using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    // プレイヤー
    public GameObject target;
    public float speed = 3f;
    public float stopDistance = 1.5f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player1");
    }


    void Update()
    {
        if (target == null) return;

        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0f;

        float distance = dir.magnitude;

        if (distance > stopDistance)
        {
            dir.Normalize();
            transform.position += dir * speed * Time.deltaTime;

            // プレイヤーの方向を見る
            transform.forward = dir;
        }
    }
}
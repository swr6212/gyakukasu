using UnityEngine;

public class PachinkoBall : MonoBehaviour
{
    [SerializeField] private float randomForce = 0.5f;
    [SerializeField] private float interval = 0.2f;

    private Rigidbody rb;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("PachinkoBallにRigidbodyがありません！");
            return;
        }

        // 物理を確実に有効化
        rb.useGravity = true;
        rb.isKinematic = false;

        // 玉同士を干渉させない
        int ballLayer = LayerMask.NameToLayer("ball");

        if (ballLayer != -1)
        {
            Physics.IgnoreLayerCollision(ballLayer, ballLayer);
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        timer += Time.fixedDeltaTime;

        if (timer >= interval)
        {
            timer = 0f;

            float randomX = Random.Range(-randomForce, randomForce);

            rb.AddForce(
                new Vector3(randomX, 0f, 0f),
                ForceMode.Impulse
            );
        }
    }
}
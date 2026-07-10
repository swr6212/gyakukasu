using UnityEngine;

[RequireComponent(typeof(Rigidbody))]  //自動でRididbodyがつくようにしている
public class ObjectController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 500.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 前後左右の移動
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }
}
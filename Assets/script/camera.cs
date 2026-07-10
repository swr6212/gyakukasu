using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 0.1f;

    private float yaw = 0f;
    private float pitch = 10f; // 上下角

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // 左右回転
        yaw += mouseDelta.x * sensitivity;

        // 上下回転
        pitch -= mouseDelta.y * sensitivity;

        // 上下の制限（超重要）
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 offset = new Vector3(0, 0, -distance);

        Vector3 targetPos = target.position + Vector3.up * height;

        transform.position = targetPos + rotation * offset;

        transform.LookAt(targetPos);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveInput;
    public Transform cameraTransform;

    private void Update()
    {
        if (cameraTransform == null) return;

        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDir;
        }

        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
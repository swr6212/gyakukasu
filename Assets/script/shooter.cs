using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform cameraTransform;
    public float bulletSpeed = 10f;

    float seconds;

    void Update()
    {
        seconds += Time.deltaTime;

        if (seconds >= 0.5f)
        {
            Vector3 dir = cameraTransform.forward;
            dir.y = 0;
            dir.Normalize();

            Vector3 spawnPos = transform.position + dir * 1.0f;

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.AddForce(dir * bulletSpeed, ForceMode.Impulse);

            Destroy(bullet, 3f);

            seconds = 0;
        }
    }
}
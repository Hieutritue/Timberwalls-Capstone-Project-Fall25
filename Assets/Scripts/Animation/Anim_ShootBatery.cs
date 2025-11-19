using UnityEngine;

public class ShootBattery : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject projectile;
    public float height = 3f;
    public Transform spawnPoint;
    public Transform targetPosition;
    private Rigidbody rb;

    private void Awake()
    {
        rb = projectile.GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("Projectile must have a Rigidbody!");
    }

    public void Shoot()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;

        Vector3 start = transform.position;
        Vector3 end = targetPosition.position;

        float gravity = Mathf.Abs(Physics.gravity.y);

        // Vertical velocity (to reach peak height)
        float Vy = Mathf.Sqrt(2 * gravity * height);
        float t1 = Vy / gravity;

        // Time to fall from peak to end.y
        float h2 = start.y + height - end.y;
        float t2 = Mathf.Sqrt(2 * h2 / gravity);

        float totalTime = t1 + t2;

        // Horizontal velocity
        Vector3 horizontal = new Vector3(end.x - start.x, 0, end.z - start.z);
        Vector3 Vx = horizontal / totalTime;

        // Final velocity
        Vector3 resultVelocity = Vx + Vector3.up * Vy;

        rb.linearVelocity = resultVelocity/2;
    }

    public void SpawnBattery()
    {
        projectile.SetActive(true);
    }
}
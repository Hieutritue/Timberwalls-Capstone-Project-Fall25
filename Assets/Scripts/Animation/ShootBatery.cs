using UnityEngine;

public class ShootBatery : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject projectile;      
    public float forceAmount = 10f;

    [Header("Direction Settings")]
    public Vector2 shootDirection = new Vector2(1f, 1f);

    private Rigidbody rb;

    private void Awake()
    {
        rb = projectile.GetComponent<Rigidbody>();

        if (rb == null)
            Debug.LogError("Projectile must have a Rigidbody!");
    }

    public void Shoot()
    {
        // 1. Move projectile to shooter
        projectile.transform.position = transform.position;

        // 2. Reset velocity so previous shots don’t influence new shoots
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 4. Normalized direction
        Vector3 dir = new Vector3(shootDirection.x, shootDirection.y, 0).normalized;

        // 5. Add force
        rb.AddForce(dir * forceAmount, ForceMode.Impulse);
    }
}

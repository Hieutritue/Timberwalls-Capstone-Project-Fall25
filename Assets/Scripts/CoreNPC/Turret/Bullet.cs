using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeoutDelay = 5.0f;
    public BulletSO stats;

    private Rigidbody rb;
    private Collider bulletCollider; // Add reference to collider
    private IObjectPool<Bullet> Pool;
    private Coroutine deactivationCoroutine;
    private bool hasHit = false; // Track if bullet has hit something

    public IObjectPool<Bullet> pool { set => Pool = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>(); // Get the collider
    }

    public void deactivate()
    {
        hasHit = false; // Reset on activation
        if (bulletCollider != null)
            bulletCollider.enabled = true; // Re-enable collider
        deactivationCoroutine = StartCoroutine(deactivateRoutine(timeoutDelay));
    }

    IEnumerator deactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Pool.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return; // Ignore if already hit something
        hasHit = true;

        // Disable collider immediately to prevent multiple hits
        if (bulletCollider != null)
            bulletCollider.enabled = false;

        // Stop the deactivation coroutine
        if (deactivationCoroutine != null)
        {
            StopCoroutine(deactivationCoroutine);
            deactivationCoroutine = null;
        }

        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(stats.damage, stats.name);
        }

        Pool.Release(this);
    }
}


using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Base bullet class supporting pooling, lifecycle hooks, and basic physics launch.
/// Designed for inheritance (e.g., ArchingBullet, AoEBullet, HomingBullet).
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected BulletSO stats;
    [SerializeField] protected float timeoutDelay = 5f;

    [Header("Physics Settings")]
    [SerializeField] protected bool useGravity = false;
    [SerializeField] protected float gravityScale = 1f;

    [Header("Debuffs (applied on hit)")]
    protected Debuff[] debuffsToApply;
    // These are "template" debuffs (SlowDebuff, BurnDebuff, etc.)

    protected Rigidbody rb;
    protected Collider bulletCollider;
    protected IObjectPool<Bullet> Pool;
    protected Coroutine deactivationCoroutine;
    protected bool hasHit;

    public IObjectPool<Bullet> pool { get => Pool; set => Pool = value; }
    public BulletSO Stats => stats;

    // ============================================================
    // INITIALIZATION
    // ============================================================
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();
        debuffsToApply = stats.debuffs;
    }

    // ============================================================
    // LIFECYCLE
    // ============================================================
    public virtual void Activate()
    {
        hasHit = false;

        bulletCollider.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = useGravity;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        OnActivate();

        if (deactivationCoroutine != null)
            StopCoroutine(deactivationCoroutine);
        deactivationCoroutine = StartCoroutine(DeactivateAfter(timeoutDelay));
    }
    protected void ApplyDebuffs(IDebuffable target)
    {
        if (stats.debuffs == null || stats.debuffs.Length == 0)
            return;

        foreach (var debuff in stats.debuffs)
        {
            if (debuff == null) continue;

            Debuff instance = debuff.Clone();
            target.ApplyDebuff(instance);
        }
    }

    public virtual void Launch(Vector3 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    IEnumerator DeactivateAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }

    protected virtual void Deactivate()
    {
        StopAllCoroutines();
        OnDeactivate();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        Pool?.Release(this);
    }

    // ============================================================
    // COLLISION
    // ============================================================
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        hasHit = true;

        bulletCollider.enabled = false;

        // DAMAGE
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(stats.damage, stats.name);

            // Apply debuffs here for SINGLE-TARGET bullets
            if (other.TryGetComponent<IDebuffable>(out var debuffable))
            {
                ApplyDebuffs(debuffable);
            }

            OnHit(damageable);
        }

        Deactivate();
    }

    // ============================================================
    // EXTENSION HOOKS (for subclasses)
    // ============================================================
    /// <summary>Called right after bullet activation (before movement begins).</summary>
    protected virtual void OnActivate() { }

    /// <summary>Called right before bullet is returned to pool.</summary>
    protected virtual void OnDeactivate() { }

    /// <summary>Called when bullet hits a target implementing IDamageable.</summary>
    protected virtual void OnHit(IDamageable target) { }
}


using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet that explodes on contact, dealing AOE damage and applying debuffs.
/// Does NOT use falloff or knockback.
/// </summary>
public class ExplosiveBullet : Bullet
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionDamage = 30f;
    [SerializeField] private LayerMask hitMask;

    [Header("Optional VFX (Reserved for future use)")]
    [SerializeField] private string explosionVFX_ID = "";
    // (We only store the ID/name for now — no spawning logic yet.)

    /// <summary>
    /// Trigger explosion when bullet hits anything damageable.
    /// </summary>
    protected override void OnHit(IDamageable target)
    {
        base.OnHit(target);
        Explode();
    }

    private void Explode()
    {
        Vector3 center = transform.position;
        Collider[] hits = Physics.OverlapSphere(center, explosionRadius, hitMask);

        foreach (Collider c in hits)
        {
            if (c.TryGetComponent<IDamageable>(out var dmg))
                dmg.Damage((int)explosionDamage, "Explosion");

            if (c.TryGetComponent<IDebuffable>(out var deb))
                ApplyDebuffsTo(deb);   // <-- Uses base-class logic
        }

        TriggerVFX(center);
        Deactivate();
    }

    /// <summary>
    /// Placeholder VFX hook (future-ready).
    /// Currently does nothing but is kept for future implementation.
    /// </summary>
    private void TriggerVFX(Vector3 pos)
    {
        if (string.IsNullOrEmpty(explosionVFX_ID))
            return;

        // Future version:
        // VFXManager.Instance.SpawnEffect(explosionVFX_ID, pos);

        // For now we leave it blank for prototyping.
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.35f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
#endif
}


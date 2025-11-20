using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 3f;
    

    // Ensures the explosion happens ONLY once
    private bool hasExploded = false;

    protected override void OnActivate()
    {
        base.OnActivate();
        hasExploded = false;
    }

    protected override void OnHit(IDamageable target)
    {
        Explode();
        // Do NOT release the bullet; base Bullet handles deactivation automatically.
    }

    private void Explode()
    {
        if (hasExploded) 
            return;

        hasExploded = true;

        // Find all targets in explosion radius
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (var hit in hits)
        {
            // Damage enemies
            if (hit.TryGetComponent<IDamageable>(out var dmg))
            {
                dmg.Damage(stats.damage, stats.name);

                // Also apply debuffs (if bullet has debuffs defined in BulletSO)
                if (hit.TryGetComponent<IDebuffable>(out var debuffable))
                {
                    ApplyDebuffs(debuffable);
                }
            }
        }
    }

    // Optional spot for VFX later
    protected override void OnDeactivate()
    {
        // When the bullet becomes inactive, VFX can be spawned here
        // (Use a pooled explosion effect later)
        base.OnDeactivate();
    }

#if UNITY_EDITOR
    // For visualization in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.35f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
#endif
}

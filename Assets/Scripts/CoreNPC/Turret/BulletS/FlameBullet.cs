using UnityEngine;

public class FlameBullet : Bullet
{
    [Header("Flame Settings")]
    [SerializeField] private float damageInterval = 0.2f;
    [SerializeField] private BoxCollider flameCollider;   // flame hitbox
    [SerializeField] public FireConeVFX vfx;

    private float lastDamageTime;

    protected override void Awake()
    {
        base.Awake();

        // Safety — ensure collider exists & is trigger
        if (flameCollider == null)
            flameCollider = GetComponent<BoxCollider>();

        if (flameCollider != null)
            flameCollider.isTrigger = true;
    }

    protected override void OnActivate()
    {
        // Turn on VFX
        if (vfx != null)
        {
            vfx.gameObject.SetActive(true);
            vfx.ApplySize();
            vfx.ResetAlpha();
        }

        // No physics movement
        rb.isKinematic = true;
        rb.useGravity = false;

        lastDamageTime = -999f;

        
    }

    protected override void OnDeactivate()
    {
        if (vfx != null)
            vfx.BeginFade();
    }

    // Automatically resize to match your FireConeVFX
    private void UpdateColliderSize()
    {
        if (flameCollider == null || vfx == null) return;

        flameCollider.size = new Vector3(vfx.coneWidth, 1f, vfx.coneLength);
        flameCollider.center = new Vector3(0f, 0f, vfx.coneLength * 0.5f);
    }

    // MAIN DAMAGE LOGIC — via the collider
    private void OnTriggerStay(Collider other)
    {
        if (Time.time < lastDamageTime + damageInterval)
            return;

        if (!other.TryGetComponent<IDamageable>(out var damageable))
            return;

        lastDamageTime = Time.time;

        damageable.Damage(stats.damage, stats.name);

        if (other.TryGetComponent<IDebuffable>(out var deb))
            ApplyDebuffs(deb);
    }
}

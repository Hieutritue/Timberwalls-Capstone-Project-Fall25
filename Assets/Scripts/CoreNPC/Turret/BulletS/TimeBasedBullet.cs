using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AreaBullet with automatic ParticleSystem control (for flames, lasers, etc.).
/// Supports both Trigger and Overlap AoE modes, time-based deactivation, and DPS scaling.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class TimeBasedBullet: Bullet
{
    public enum AreaMode { Trigger, Overlap }

    [Header("Area Settings")]
    [SerializeField] private AreaMode mode = AreaMode.Overlap;
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1.5f, 0.5f, 1.5f);
    [SerializeField] private Vector3 localCenterOffset = Vector3.zero;
    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private float tickInterval = 0.25f;
    [SerializeField] private float damageMultiplier = 1f;

    [Header("VFX Settings")]
    [Tooltip("Optional looping ParticleSystem to play while active.")]
    [SerializeField] private ParticleSystem activeVFX;
    [Tooltip("Should particle system stop instantly on deactivate (true) or allow finish (false)?")]
    [SerializeField] private bool stopImmediately = true;

    private Coroutine tickRoutine;
    private BoxCollider triggerBox;
    private readonly HashSet<IDamageable> damagedThisTick = new();

    // ============================================================
    // INITIALIZATION
    // ============================================================
    protected override void Awake()
    {
        base.Awake();
        triggerBox = GetComponent<BoxCollider>();
        triggerBox.isTrigger = true;
    }

    // ============================================================
    // ACTIVATION & DEACTIVATION
    // ============================================================
    protected override void OnActivate()
    {
        base.OnActivate();

        // Start particles
        if (activeVFX != null)
        {
            activeVFX.Play(true);
        }

        // Start AoE tick loop
        if (tickRoutine != null)
            StopCoroutine(tickRoutine);

        tickRoutine = StartCoroutine(DamageRoutine());
    }

    protected override void OnDeactivate()
    {
        base.OnDeactivate();

        // Stop VFX gracefully
        if (activeVFX != null)
        {
            if (stopImmediately)
                activeVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            else
                activeVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        // Stop ticking
        if (tickRoutine != null)
        {
            StopCoroutine(tickRoutine);
            tickRoutine = null;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Intentionally empty — this bullet does AoE damage, not one-shot
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (mode != AreaMode.Trigger)
            return;

        if (((1 << other.gameObject.layer) & damageableLayer) == 0)
            return;

        if (other.TryGetComponent(out IDamageable target))
        {
            if (damagedThisTick.Add(target))
            {
                float tickDamage = stats.damage * tickInterval * damageMultiplier;
                target.Damage((int)tickDamage, stats.name); 
                OnHit(target);
            }
        }
    }

    // ============================================================
    // DAMAGE LOOP
    // ============================================================
    private IEnumerator DamageRoutine()
    {
        float elapsed = 0f;

        while (elapsed < timeoutDelay)
        {
            damagedThisTick.Clear();

            if (mode == AreaMode.Overlap)
                ApplyOverlapDamage();

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        Deactivate();
    }

    private void ApplyOverlapDamage()
    {
        Vector3 worldCenter = transform.TransformPoint(localCenterOffset);
        Quaternion rot = transform.rotation;
        Collider[] hits = Physics.OverlapBox(worldCenter, boxHalfExtents, rot, damageableLayer);

        float tickDamage = stats.damage * tickInterval * damageMultiplier;
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable target))
            {
                if (damagedThisTick.Add(target))
                {
                    target.Damage((int)tickDamage, stats.name);
                    OnHit(target);
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = mode == AreaMode.Trigger ? Color.magenta : Color.yellow;
        Vector3 worldCenter = transform.TransformPoint(localCenterOffset);
        Gizmos.matrix = Matrix4x4.TRS(worldCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningBullet : Bullet
{
    [Header("Chain Lightning Settings")]
    [SerializeField] private int maxChains = 3;
    [SerializeField] private float chainRange = 4f;
    [SerializeField] private float chainDelay = 0.05f;
    [SerializeField] private LineRenderer arcPrefab;

    private readonly List<EnemyBase> hitEnemies = new List<EnemyBase>();
    private bool isChaining = false;

    protected override void OnActivate()
    {
        base.OnActivate();                     // <-- IMPORTANT
        isChaining = false;
        hitEnemies.Clear();

        rb.isKinematic = false;
        rb.useGravity = false;

        bulletCollider.enabled = true;
    }

    // Override so the base class DOES NOT auto-deactivate
    protected override void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        hasHit = true;

        bulletCollider.enabled = false; // prevent multiple triggers

        if (other.TryGetComponent(out IDamageable dmg))
            OnHit(dmg);   // let our chain logic handle deactivate
    }

    protected override void OnHit(IDamageable initialTarget)
    {
        if (isChaining) return;

        if (initialTarget is EnemyBase firstEnemy)
        {
            isChaining = true;
            hitEnemies.Add(firstEnemy);
            StartCoroutine(ChainRoutine(firstEnemy));
        }
        else
        {
            Deactivate();
        }
    }

    private IEnumerator ChainRoutine(EnemyBase current)
    {
        // Damage first target
        current.Damage(stats.damage, "Thundah");

        if (current.TryGetComponent<IDebuffable>(out var deb))
            ApplyDebuffs(deb);

        // chain to new targets
        for (int i = 0; i < maxChains; i++)
        {
            EnemyBase next = FindNextTarget(current);
            if (next == null)
                break;

            hitEnemies.Add(next);

            next.Damage(stats.damage, stats.name);
            if (next.TryGetComponent<IDebuffable>(out var deb2))
                ApplyDebuffs(deb2);

            SpawnArc(current.transform.position, next.transform.position);

            current = next;

            yield return new WaitForSeconds(chainDelay);
        }

        Deactivate();
    }

    private EnemyBase FindNextTarget(EnemyBase from)
    {
        Collider[] hits = Physics.OverlapSphere(from.transform.position, chainRange, enemyLayer);

        EnemyBase best = null;
        float bestDist = Mathf.Infinity;

        foreach (var h in hits)
        {
            if (!h.TryGetComponent(out EnemyBase e))
                continue;

            if (hitEnemies.Contains(e))
                continue;

            float d = Vector3.Distance(from.transform.position, e.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = e;
            }
        }
        return best;
    }

    private void SpawnArc(Vector3 start, Vector3 end)
    {
        if (arcPrefab == null) return;

        LineRenderer arc = Instantiate(arcPrefab);
        arc.positionCount = 2;
        arc.SetPosition(0, start);
        arc.SetPosition(1, end);
        Destroy(arc.gameObject, 0.15f);
    }

    protected override void OnDeactivate()
    {
        base.OnDeactivate();

        bulletCollider.enabled = false;
        isChaining = false;
        hitEnemies.Clear();
    }
}


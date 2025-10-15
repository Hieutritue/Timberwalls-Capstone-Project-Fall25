using UnityEngine;

public class Enemy : EnemyBase
{
    protected override void AttackTarget()
    {
        base.AttackTarget();

    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void ExecuteState()
    {
        base.ExecuteState();
    }

    protected override void PerformAttack()
    {
        base.PerformAttack();
        if (target.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(stats.attackDamage, stats.name);
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateState()
    {
        base.UpdateState();
    }

    protected override void WalkLeft()
    {
        base.WalkLeft();
    }
}
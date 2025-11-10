using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    protected GameObject target;

    [Header("Stats")]
    [SerializeField] protected EnemySO stats;

    protected Transform targetTransform;
    protected float lastAttackTime;
    protected State CurrentState;
    protected Vector3 walkDir;
    protected enum State { Walk, Attack, Death }

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            targetTransform = target.transform;
            if (this.transform.position.x - targetTransform.position.x < 0) walkDir = Vector3.right;
            else walkDir = Vector3.left;
        }

        CurrentState = State.Walk;
    }

    protected virtual void Update()
    {
        if (CurrentState == State.Death) return;

        UpdateState();
        ExecuteState();
    }

    protected virtual void UpdateState()
    {
        float distanceToTarget = 10000.0f;
        if (target != null) distanceToTarget = Mathf.Abs(transform.position.x - targetTransform.position.x);
        Debug.Log(distanceToTarget);


        if (distanceToTarget <= stats.attackRange)
        {
            CurrentState = State.Attack;
        }
        else
        {
            CurrentState = State.Walk;
        }
    }

    protected virtual void ExecuteState()
    {
        Debug.Log($"{CurrentState} is running!");
        switch (CurrentState)
        {

            case State.Walk:
                WalkLeft();
                break;
            case State.Attack:
                AttackTarget();
                break;
        }
    }

    protected virtual void WalkLeft()
    {
        transform.position += walkDir * stats.movementSpeed * Time.deltaTime;
    }

    protected virtual void AttackTarget()
    {
        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void PerformAttack()
    {
        Debug.Log($"{gameObject.name} is attacking!");

        // Implement attack logic here:
        // - Trigger attack animation
        // - Deal damage to target
        // - Play attack effects/sounds

        // Example damage dealing:
        // if (target.TryGetComponent<IDamageable>(out var damageable))
        // {
        //     damageable.TakeDamage(stats.attackDamage);
        // }
    }

    public virtual void ResetEnemy()
    {
        // Reset health and any other state

        // Reset animations, velocities, etc.
    }

    protected virtual void Die()
    {
        // Return to pool instead of destroying
        PooledEnemy pooledEnemy = GetComponent<PooledEnemy>();
        if (pooledEnemy != null)
        {
            pooledEnemy.ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (stats == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);

        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

    public virtual void Damage(int amount, string EnemyName)
    {
        Debug.Log($"Damage amount is {amount} by {EnemyName}");
    }
}

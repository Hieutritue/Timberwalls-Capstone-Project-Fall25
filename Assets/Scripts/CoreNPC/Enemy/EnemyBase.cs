using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] protected GameObject target;

    [Header("Stats")]
    [SerializeField] protected EnemySO stats;

    protected Transform targetTransform;
    protected float lastAttackTime;
    protected State CurrentState;
    protected enum State { Walk, Attack, Death }

    protected virtual void Start()
    {

        if (target != null)
        {
            targetTransform = target.transform;
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
        transform.position += Vector3.left * stats.movementSpeed * Time.deltaTime;
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

    protected virtual void Die()
    {
        CurrentState = State.Death;
        // Add death logic here (animation, disable, destroy, etc.)
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
}

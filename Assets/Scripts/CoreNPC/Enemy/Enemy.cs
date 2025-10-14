using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject target;

    [Header("Stats")]
    [SerializeField] private EnemySO stats;

    private Transform targetTransform;
    private float lastAttackTime;
    private State currentState;

    private enum State { Walk, Attack, Death }

    void Start()
    {
       
        if (target != null)
        {
            targetTransform = target.transform;
        }

        currentState = State.Walk;
    }

    void Update()
    {
        if (currentState == State.Death) return;

        UpdateState();
        ExecuteState();
    }

    void UpdateState()
    {
        float distanceToTarget = 10000.0f;
        if (target != null) distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);


        if (distanceToTarget <= stats.attackRange)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Walk;
        }
    }

    void ExecuteState()
    {
        Debug.Log($"{currentState} is running!");
        switch (currentState)
        {
            
            case State.Walk:
                WalkLeft();
                break;
            case State.Attack:
                AttackTarget();
                break;
        }
    }

    void WalkLeft()
    {
        transform.position += Vector3.left * stats.movementSpeed * Time.deltaTime;
    }

    void AttackTarget()
    {
        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformAttack()
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

    public void Die()
    {
        currentState = State.Death;
        // Add death logic here (animation, disable, destroy, etc.)
    }

    void OnDrawGizmosSelected()
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
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable, IDebuffable
{
    protected GameObject target;

    [Header("Stats")]
    [SerializeField] protected EnemySO stats;

    protected Transform targetTransform;
    protected float lastAttackTime;
    protected State CurrentState;
    protected Vector3 walkDir;
    protected int health;
    protected enum State { Walk, Attack, Death }

    private DebuffManager debuffs;

    private bool isDead = false;

    protected virtual void Awake()
    {
        ResetEnemy();
    }

    protected virtual void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 0)
        {
            GameObject closest = null;
            float closestDist = float.MaxValue;
            Vector3 myPos = transform.position;

            foreach (GameObject p in players)
            {
                float dist = (p.transform.position - myPos).sqrMagnitude; // fast distance check

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = p;
                }
            }

            target = closest;

            if (target != null)
            {
                targetTransform = target.transform;

                // Determine walking direction based on X-axis
                if (transform.position.x - targetTransform.position.x < 0)
                    walkDir = Vector3.right;
                else
                    walkDir = Vector3.left;
            }
        }

        CurrentState = State.Walk;
    }

    protected virtual void Update()
    {
        if (CurrentState == State.Death) return;

        debuffs.Update(Time.deltaTime);

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
        if (targetTransform == null) return;

        bool targetRight = targetTransform.position.x > transform.position.x;
        walkDir = targetRight ? Vector3.right : Vector3.left;

        // Optional: flip enemy visually
        transform.localScale = new Vector3(
            targetRight ? 1 : -1,
            transform.localScale.y,
            transform.localScale.z);

        float speed = debuffs.ModifyMovement(stats.movementSpeed);
        transform.position += walkDir * speed * Time.deltaTime;
    }

    protected virtual void AttackTarget()
    {
        if (Time.time >= lastAttackTime + debuffs.ModifyAttackCooldown(stats.attackCooldown))
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

public virtual bool checkDead() => isDead;
    public virtual void ResetEnemy()
    {
        // Reset health
        health = stats.health;
        isDead = false;

        // Reset debuffs (optional but recommended)
        debuffs = new DebuffManager(this);

        // Recalculate walking direction (IMPORTANT FIX)
        if (targetTransform != null)
        {
            walkDir = (targetTransform.position.x > transform.position.x)
                ? Vector3.right
                : Vector3.left;
        }

        // Reset state
        CurrentState = State.Walk;

        // Reset animation/timers if you use them later
        lastAttackTime = 0f;
    }

    protected virtual void Die()
{
    if (isDead) return;    // <<---- IMPORTANT FIX
    isDead = true;

    PooledEnemy pooledEnemy = GetComponent<PooledEnemy>();
    if (pooledEnemy != null)
         pooledEnemy.ReturnToPool();
    else
         Destroy(gameObject);
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
        float damageMult = debuffs.ModifyDamageTaken(1f);
        health -= Mathf.RoundToInt(amount * damageMult);
        if (health <= 0) Die();
    }

    public void ApplyDebuff(Debuff debuff)
    {
        if (debuff == null) return;
        debuffs.Apply(debuff, this);
    }
}

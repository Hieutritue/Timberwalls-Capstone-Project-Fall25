using System;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] private SO.EnemySo _enemySo;
        public SO.EnemySo EnemySo => _enemySo;

        public EnemyRuntimeStats Stats { get; private set; }

        [field: SerializeField]
        public float CurrentHealth { get; private set; }

        public float MoveSpeed => Stats.MoveSpeed;
        public float AttackDamage => Stats.AttackDamage;
        public float AttackRange => Stats.AttackRange;
        public float AttackCooldown => Stats.AttackCooldown;

        public SO.EnemyType EnemyType => _enemySo.EnemyType;

        private ShieldSystem.ShieldSystem _shieldSystem;
        private BoxCollider _targetCol;
        private float attackCooldown = 0f;

        // --------------------------------------------------------
        // Every time pooled object wakes up again, stats reset here
        // --------------------------------------------------------
        private void OnEnable()
        {
            EnemyManager.Instance.AddEnemyInstance(this);

            if (Stats == null)
                Stats = new EnemyRuntimeStats();

            // Reset runtime stats from base SO
            Stats.Health         = _enemySo.Health;
            Stats.AttackDamage   = _enemySo.AttackDamage;
            Stats.MoveSpeed      = _enemySo.MoveSpeed;
            Stats.AttackRange    = _enemySo.AttackRange;
            Stats.AttackCooldown = _enemySo.AttackCooldown;

            CurrentHealth = Stats.Health;
            attackCooldown = 0f;

            _shieldSystem = ShieldSystem.ShieldSystem.Instance;
            enabled = true;
        }

        private void OnDisable()
        {
            EnemyManager.Instance.RemoveEnemyInstance(this);
        }

        void Update()
        {
            if (_shieldSystem == null) return;
            if (_targetCol == null) return;

            Vector3 closestPoint = _targetCol.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);

            if (distance > Stats.AttackRange)
                MoveTowards(closestPoint);
            else
                TryAttack();
        }

        public void SetTarget(bool spawnedFromLeft)
        {
            _targetCol = spawnedFromLeft ? _shieldSystem.ShieldWall.LeftWallCollider
                                         : _shieldSystem.ShieldWall.RightWallCollider;

            Debug.Log(
                $"[EnemyInstance] {name} SetTarget → {(spawnedFromLeft ? "LEFT" : "RIGHT")}"
            );
        }

        void MoveTowards(Vector3 p)
        {
            Vector3 dir = (p - transform.position).normalized;
            transform.position += dir * (Stats.MoveSpeed * Time.deltaTime);
        }

        void TryAttack()
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown > 0) return;

            attackCooldown = Stats.AttackCooldown;
            _shieldSystem.ShieldWall.ReceiveDamage(Stats.AttackDamage);

            Debug.Log($"[EnemyInstance] {EnemySo.name} DID DAMAGE: {Stats.AttackDamage}");
        }

        public void TakeDamage(float dmg)
        {
            CurrentHealth -= dmg;
            if (CurrentHealth <= 0)
                Die();
        }

        void Die()
        {
            // pooling callback in PooledEnemy handles returning to pool
        }

        public void ApplyFinalStats(float health, float dmg, float speed, float range, float cd)
        {
            Stats.Health         = health;
            Stats.AttackDamage   = dmg;
            Stats.MoveSpeed      = speed;
            Stats.AttackRange    = range;
            Stats.AttackCooldown = cd;

            CurrentHealth = Stats.Health;
        }

        public void PrintFinalStats(string side)
        {
            Debug.Log(
                $"[EnemyInstance] {name} ({EnemySo.name}) " +
                $"HP={Stats.Health:F1}, DMG={Stats.AttackDamage:F1}, SPD={Stats.MoveSpeed:F2}, " +
                $"RNG={Stats.AttackRange:F2}, CD={Stats.AttackCooldown:F2} | Side={side}"
            );
        }
    }

    public class EnemyRuntimeStats
    {
        public float Health;
        public float AttackDamage;
        public float MoveSpeed;
        public float AttackRange;
        public float AttackCooldown;
    }
}

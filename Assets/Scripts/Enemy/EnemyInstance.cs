
using System;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class EnemyInstance : MonoBehaviour
    {
        [SerializeField] private SO.EnemySo _enemySo;

        [field:SerializeField]
        public float CurrentHealth { get; private set; }
        public float MoveSpeed => _enemySo.MoveSpeed;
        public float AttackDamage => _enemySo.AttackDamage;
        public float AttackRange => _enemySo.AttackRange;
        public float AttackCooldown => _enemySo.AttackCooldown;
        public SO.EnemyType EnemyType => _enemySo.EnemyType;

        private ShieldSystem.ShieldSystem _shieldSystem;
        private BoxCollider _targetCol;
        private float attackCooldown = 0f;

        private void OnEnable()
        {
            EnemyManager.Instance.AddEnemyInstance(this);
        }

        private void OnDisable()
        {
            EnemyManager.Instance.RemoveEnemyInstance(this);
        }

        void Start()
        {
            _shieldSystem = ShieldSystem.ShieldSystem.Instance;
            CurrentHealth = _enemySo.Health;
            SetTarget(true);
        }
        
        public void SetTarget(bool spawnedFromLeft)
        {
            _targetCol = spawnedFromLeft ? _shieldSystem.ShieldWall.LeftWallCollider
                                        : _shieldSystem.ShieldWall.RightWallCollider;
        }

        void Update()
        {
            if (ShieldSystem.ShieldSystem.Instance == null) return;

            // Find the closest point to move toward
            Vector3 closestPoint = _targetCol.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);

            // Move if outside attack range
            if (distance > AttackRange)
                MoveTowards(closestPoint);
            else
                TryAttack();
        }

        // -----------------------------------------------
        // Movement depends on enemy type
        // -----------------------------------------------
        void MoveTowards(Vector3 point)
        {
            Vector3 direction = (point - transform.position).normalized;
            transform.position += direction * (MoveSpeed * Time.deltaTime);
        }

        // -----------------------------------------------
        // Attack logic
        // -----------------------------------------------
        void TryAttack()
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown > 0) return;

            attackCooldown = AttackCooldown;

            // You can damage target here
            _shieldSystem.ReceiveDamage(AttackDamage);
        }

        // -----------------------------------------------
        public void TakeDamage(float dmg)
        {
            CurrentHealth -= dmg;
            if (CurrentHealth <= 0)
                Die();
        }

        void Die()
        {
            // ObjectPoolManager.Instance.Release(this.gameObject);
        }
    }
}
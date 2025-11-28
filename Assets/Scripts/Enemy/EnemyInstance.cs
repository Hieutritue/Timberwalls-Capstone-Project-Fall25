using System;
using DefaultNamespace.ScheduleSystem;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using Util;

namespace DefaultNamespace.Enemy
{
    public class EnemyInstance : MonoBehaviour
    {
        [Header("Assigned in Inspector")] [SerializeField]
        private SO.EnemySo _enemySo;

        [SerializeField] private Animator _animator; // ← NEW OPTIONAL SUPPORT
        [SerializeField] private MMF_Player _hurtFeedback;

        public SO.EnemySo EnemySo => _enemySo;

        [field: SerializeField] public float CurrentHealth { get; private set; }

        public float MoveSpeed { get; set; }
        public float AttackDamage { get; set; }
        public float AttackRange { get; set; }
        public float AttackCooldown { get; set; }

        private bool _isLeftSide;
        private ShieldSystem.ShieldSystem _shieldSystem;
        private BoxCollider _targetCol;
        private bool _isDead = false;
        private bool _isAttacking = false; // prevents spam restarting animation
        private float _attackCooldown;
        private static readonly int Attack = Animator.StringToHash("Attack");


        [Header("Spawn Weight Curve (Day -> Weight)")]
        public AnimationCurve SpawnWeightCurve = AnimationCurve.Linear(0, 1f, 30, 3f);

        public float GetWeight(int day)
        {
            return SpawnWeightCurve.Evaluate(day);
        }

        // --------------------------------------------------------
        // Pool Wake — always run when activated
        // --------------------------------------------------------
        private void OnEnable()
        {
            EnemyManager.Instance.AddEnemyInstance(this);
            transform.GetChild(0).transform.localScale = Vector3.one;

            CurrentHealth = GetHP(GameTimeManager.Instance.CurrentDay);
            MoveSpeed = _enemySo.MoveSpeed;
            AttackDamage = GetDamage(GameTimeManager.Instance.CurrentDay);
            AttackRange = _enemySo.AttackRange;
            AttackCooldown = _enemySo.AttackCooldown;

            _isAttacking = false;
            _isDead = false;
            _attackCooldown = AttackCooldown;

            _shieldSystem = ShieldSystem.ShieldSystem.Instance;
            SetTarget(true);
        }

        private void OnDisable()
        {
            EnemyManager.Instance.RemoveEnemyInstance(this);
        }
        
        public float GetHP(int day)
        {
            return _enemySo.Health + _enemySo.HealthPerDay * day;
        }
        
        public float GetDamage(int day)
        {
            return _enemySo.AttackDamage + _enemySo.AttackDamagePerDay * day;
        }

        // --------------------------------------------------------
        // MAIN LOOP
        // --------------------------------------------------------
        void Update()
        {
            if (_isDead) return;

            if (!GameTimeManager.Instance.IsNight)
            {
                var pos = new Vector3(_isLeftSide ? -200 : 200, 4, 0);
                MoveTowards(pos);
                transform.DORotate(new Vector3(transform.rotation.x, _isLeftSide ? -90 : 90, transform.rotation.z),
                    2);
                return;
            }

            if (_shieldSystem == null || _targetCol == null) return;

            Vector3 p = _targetCol.ClosestPoint(transform.position);
            float dist = Vector3.Distance(transform.position, p);

            if (dist > AttackRange)
            {
                MoveTowards(p);
            }
            else
            {
                TryAttack();
            }
        }

        // --------------------------------------------------------
        public void SetTarget(bool left)
        {
            _isLeftSide = left;
            _targetCol = left
                ? _shieldSystem.ShieldWall.LeftWallCollider
                : _shieldSystem.ShieldWall.RightWallCollider;


            transform.rotation = Quaternion.Euler(0f, left ? 90f : -90f, 0f); // facing
        }

        // --------------------------------------------------------
        void MoveTowards(Vector3 p)
        {
            Vector3 dir = (p - transform.position).normalized;
            transform.position += dir * (MoveSpeed * Time.deltaTime);
        }

        // --------------------------------------------------------
        void TryAttack()
        {
            if (_isDead) return;
            _attackCooldown -= Time.deltaTime;

            // If still recharging → idle but DO NOT restart Attack
            if (_attackCooldown > 0f)
            {
                return;
            }

            _animator.SetTrigger(Attack);
            _attackCooldown = AttackCooldown;
            _shieldSystem.ShieldWall.ReceiveDamage(AttackDamage);
        }

        // =====================================================================
        // DAMAGE + POOL-RETURN DEATH
        // =====================================================================
        public void TakeDamage(float dmg)
        {
            if (_isDead) return;
            CurrentHealth -= dmg;
            _hurtFeedback.PlayFeedbacks();
            if (CurrentHealth <= 0)
                Die();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Bound"))
            {
                Die();
            }
        }

        private void Die()
        {
            ObjectPoolManager.Instance.Release(gameObject);
        }
    }
}
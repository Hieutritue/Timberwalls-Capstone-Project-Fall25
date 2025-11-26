using System;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class EnemyInstance : MonoBehaviour
    {
        [Header("Assigned in Inspector")]
        [SerializeField] private SO.EnemySo _enemySo;
        [SerializeField] private Animator animator; // ← NEW OPTIONAL SUPPORT

        public SO.EnemySo EnemySo => _enemySo;
        public EnemyRuntimeStats Stats { get; private set; }

        [field: SerializeField] public float CurrentHealth { get; private set; }

        public float MoveSpeed => Stats.MoveSpeed;
        public float AttackDamage => Stats.AttackDamage;
        public float AttackRange => Stats.AttackRange;
        public float AttackCooldown => Stats.AttackCooldown;

        private ShieldSystem.ShieldSystem _shieldSystem;
        private BoxCollider _targetCol;
        private float attackCooldown = 0f;
        private bool isDead = false;
        private bool isAttacking = false; // prevents spam restarting animation


        // --------------------------------------------------------
        // Pool Wake — always run when activated
        // --------------------------------------------------------
        private void OnEnable()
        {
            EnemyManager.Instance.AddEnemyInstance(this);

            if (Stats == null)
                Stats = new EnemyRuntimeStats();

            Stats.Health = _enemySo.Health;
            Stats.AttackDamage = _enemySo.AttackDamage;
            Stats.MoveSpeed = _enemySo.MoveSpeed;
            Stats.AttackRange = _enemySo.AttackRange;
            Stats.AttackCooldown = _enemySo.AttackCooldown;

            CurrentHealth = Stats.Health;
            attackCooldown = 0f;
            isAttacking = false;
            isDead = false;

            _shieldSystem = ShieldSystem.ShieldSystem.Instance;

            PlayAnim_Idle();
        }

        private void OnDisable()
        {
            EnemyManager.Instance.RemoveEnemyInstance(this);
        }

        // --------------------------------------------------------
        // MAIN LOOP
        // --------------------------------------------------------
        void Update()
        {
            if (isDead) return;
            if (_shieldSystem == null || _targetCol == null) return;

            Vector3 p = _targetCol.ClosestPoint(transform.position);
            float dist = Vector3.Distance(transform.position, p);

            if (dist > AttackRange)
            {
                MoveTowards(p);
                PlayAnim_Walk();
            }
            else
            {
                TryAttack();
            }
        }

        // --------------------------------------------------------
        public void SetTarget(bool left)
        {
            _targetCol = left ? _shieldSystem.ShieldWall.LeftWallCollider
                              : _shieldSystem.ShieldWall.RightWallCollider;


            transform.rotation = Quaternion.Euler(0f, left ? 90f : -90f, 0f);   // facing

        }

        // --------------------------------------------------------
        void MoveTowards(Vector3 p)
        {
            Vector3 dir = (p - transform.position).normalized;
            transform.position += dir * (MoveSpeed * Time.deltaTime);

            // OPTIONAL — animation speed scaling
            if (animator != null)
                animator.speed = Mathf.Lerp(1f, MoveSpeed, 0.5f);
        }

        // --------------------------------------------------------
        void TryAttack()
        {
            if (isDead) return;
            attackCooldown -= Time.deltaTime;

            // If still recharging → idle but DO NOT restart Attack
            if (attackCooldown > 0f)
            {
                if (!isAttacking) PlayAnim_Idle();  // ONLY idle if not inside attack
                return;
            }

            // -------------------------------------------------------
            // Attack is ready → trigger ONCE, then lock until animation event unlocks
            // -------------------------------------------------------
            if (!isAttacking)
            {

                attackCooldown = AttackCooldown;
                isAttacking = true;

                Debug.Log($"<color=#FF9A00>[ATTACK]</color> {EnemySo.name} Attack Started");
                PlayAnim_Attack();    // animation event will apply damage + unlock attack
            }
        }


        // Event Hook if you want animation-timed damage instead
        // Animation Event Hook (called from Animator)
        public void AnimationEvent_DoDamage()
        {
            Debug.Log($"<color=#FFD300><b>⚡ ANIM EVENT</b></color> {EnemySo.name} HIT → <b>{AttackDamage}</b> damage");

            _shieldSystem.ShieldWall.ReceiveDamage(AttackDamage);
            // <-- unlocks next attack only when animation event fires
            isAttacking = false;
        }



        // =====================================================================
        // DAMAGE + POOL-RETURN DEATH
        // =====================================================================
        public void TakeDamage(float dmg)
        {
            if (isDead) return;
            CurrentHealth -= dmg;

            if (CurrentHealth <= 0)
                TriggerDeath();
        }

        void TriggerDeath()
        {
            isDead = true;
            PlayAnim_Death();
        }

        private void ReturnToPoolSafely()
        {
            Debug.Log($"<color=#00E5FF><b>[POOL]</b></color> {EnemySo.name} → Attempting ReturnToPool()");

            if (TryGetComponent(out PooledEnemy pooled))
            {
                Debug.Log($"<color=#00FF8A><b>[POOL SUCCESS]</b></color> {EnemySo.name} returned to object pool");
                pooled.ReturnToPool();
            }
            else
            {
                Debug.LogWarning($"<color=#FF4A4A><b>[POOL FAIL]</b></color> {EnemySo.name} had NO pool reference — Destroying instance");
                Destroy(gameObject);
            }
        }


        // 🔥 Animation event calls THIS at the last frame
        public void AnimationEvent_DeathFinished()
        {
            ReturnToPoolSafely();  // ← FINALLY RETURNS TO OBJECT POOL
        }

        public void AnimationEvent_DeathFinished_callDeath() => isDead = true;
        // --------------------------------------------------------
        // STATS OVERRIDE (used by spawner)
        // --------------------------------------------------------
        public void ApplyFinalStats(float hp, float dmg, float sp, float rng, float cd)
        {
            Stats.Health = hp;
            Stats.AttackDamage = dmg;
            Stats.MoveSpeed = sp;
            Stats.AttackRange = rng;
            Stats.AttackCooldown = cd;
            CurrentHealth = hp;
        }

        public void PrintFinalStats(string side)
        {
            Debug.Log($"[EnemyInstance Stats] {EnemySo.name} | HP={Stats.Health} DMG={Stats.AttackDamage} SPD={Stats.MoveSpeed} RNG={Stats.AttackRange} CD={Stats.AttackCooldown} | Side={side}");
        }

        // --------------------------------------------------------
        // ANIMATION WRAPPER
        // --------------------------------------------------------
        void PlayAnim_Idle() => PlayAnim(_enemySo.animStates.Idle);
        void PlayAnim_Walk() => PlayAnim(_enemySo.animStates.Walk);
        void PlayAnim_Attack() => PlayAnim(_enemySo.animStates.Attack);
        void PlayAnim_Death() => PlayAnim(_enemySo.animStates.Death);

        void PlayAnim(string state)
        {
            if (animator == null || string.IsNullOrEmpty(state)) return;

            animator.Play(state);
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

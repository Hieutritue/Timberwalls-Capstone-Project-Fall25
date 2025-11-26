using UnityEngine;

namespace DefaultNamespace.Enemy.SO
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/Enemy/EnemySO", order = 1)]
    public class EnemySo : ScriptableObject
    {
        [Header("Base Stats")]
        public float Health;
        public float AttackDamage;
        public float MoveSpeed;
        public float AttackRange;
        public float AttackCooldown;

        [Header("Per-Enemy Caps")]
        [Tooltip("Maximum movement speed this enemy can reach after scaling (0 = no cap).")]
        public float MaxMoveSpeed = 0f;

        [Tooltip("Maximum attack range this enemy can reach after scaling (0 = no cap).")]
        public float MaxAttackRange = 0f;

        [Header("Tier/Base Multiplier and Animation set")]
        [Tooltip("Base multipliers applied before day scaling. Use different assets for 'tier' behaviour.")]
        public EnemyStatMultiplierSO tierMultiplier;
        public AnimationStateSetSO animStates;
    }

    [CreateAssetMenu(fileName = "EnemyStatMultiplierSO", menuName = "ScriptableObjects/Enemy/EnemyStatMultiplierSO", order = 2)]
    public class EnemyStatMultiplierSO : ScriptableObject
    {
        [Header("Multipliers (Tier/Base)")]
        public float HealthMult = 1f;
        public float DamageMult = 1f;
        public float SpeedMult = 1f;
        public float RangeMult = 1f;

        [Tooltip("If > 1, enemies attack faster (cooldown is divided by this).")]
        public float CooldownMult = 1f;
    }

    [CreateAssetMenu(fileName = "AnimSet", menuName = "ScriptableObjects/Enemy/AnimationStateSet")]
public class AnimationStateSetSO : ScriptableObject
{
    [Header("Animation State Names")]
    public string Idle = "Idle";
    public string Walk = "Walk";
    public string Attack = "Attack";
    public string Death = "Death";
}
}

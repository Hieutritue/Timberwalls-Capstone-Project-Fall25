using UnityEngine;

namespace DefaultNamespace.Enemy.SO
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/Enemy/EnemySO", order = 1)]
    public class EnemySo : ScriptableObject
    {
        [Header("Base Stats")] public float Health;
        public float AttackDamage;
        public float MoveSpeed;
        public float AttackRange;
        public float AttackCooldown;

        [Header("Increase Stat")] public float AttackDamagePerDay;
        public float HealthPerDay;
        
        [Header("Other")]
        public Sprite Image;
    }
}
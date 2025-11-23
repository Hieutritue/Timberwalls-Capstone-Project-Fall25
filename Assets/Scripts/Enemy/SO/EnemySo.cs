using UnityEngine;

namespace DefaultNamespace.Enemy.SO
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/Enemy/EnemySO", order = 1)]
    public class EnemySo : ScriptableObject
    {
        public float Health;
        public float AttackDamage;
        public float MoveSpeed;
        public float AttackRange;
        public float AttackCooldown;
        public EnemyType EnemyType;
    }
    
    public enum EnemyType
    {
        Walking,
        Flying
    }
}
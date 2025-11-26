using UnityEngine;

namespace DefaultNamespace.Enemy.SO
{
    [CreateAssetMenu(fileName = "EnemyStatMultiplierSO", menuName = "ScriptableObjects/Enemy/EnemyStatMultiplier", order = 2)]
    public class EnemyStatMultiplierSO : ScriptableObject
    {
        public float HealthMult = 1f;
        public float DamageMult = 1f;
        public float SpeedMult = 1f;
        public float RangeMult = 1f;
        public float CooldownMult = 1f; // divisor multiplier
    }
}
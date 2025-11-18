using UnityEngine;

namespace DefaultNamespace.ShieldSystem
{
    [CreateAssetMenu(fileName = "ShieldLevelSO", menuName = "ScriptableObjects/ShieldSystem/ShieldLevelSO", order = 1)]
    public class ShieldLevelSO : ScriptableObject
    {
        public float Health;
        public float BaseRecoverySpeed;
    }
}
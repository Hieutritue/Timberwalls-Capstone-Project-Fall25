using UnityEngine;

namespace ShieldSystem
{
    [CreateAssetMenu(fileName = "ShieldMaintainabilityLevelSO",
        menuName = "ScriptableObjects/ShieldSystem/ShieldMaintainabilityLevelSO",
        order = 1)]
    public class ShieldMaintainabilityLevelSo : ScriptableObject
    {
        public float BaseRecoverySpeed;
    }
}
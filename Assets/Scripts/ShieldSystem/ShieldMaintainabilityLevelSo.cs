using UnityEngine;

namespace ShieldSystem
{
    [CreateAssetMenu(fileName = "ShieldMaintainabilityLevelSO",
        menuName = "ScriptableObjects/ShieldSystem/ShieldMaintainabilityLevelSO",
        order = 1)]
    public class ShieldMaintainabilityLevelSo : PlaceableSO
    {
        public float BaseRecoverySpeed;
        public int Tier;
    }
}
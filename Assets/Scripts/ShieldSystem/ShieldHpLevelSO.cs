using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.ShieldSystem
{
    [CreateAssetMenu(fileName = "ShieldHpLevelSO", menuName = "ScriptableObjects/ShieldSystem/ShieldHpLevelSO", order = 1)]
    public class ShieldHpLevelSO : PlaceableSO
    {
        public float Health;
        public int Tier;
    }
}
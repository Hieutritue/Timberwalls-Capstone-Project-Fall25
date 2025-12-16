using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingSystem
{
    [CreateAssetMenu(fileName = "TurretFurnitureSo", menuName = "ScriptableObjects/Building/TurretFurnitureSo")]
    public class TurretFurnitureSo : PlaceableSO
    {
        [Header("Turret Stats")] public float BulletSpeed;
        public float AttackRange;
        public float BaseFireRate;
        public float BaseDamage;
        public float BaseTraverseSpeed;
    }
}
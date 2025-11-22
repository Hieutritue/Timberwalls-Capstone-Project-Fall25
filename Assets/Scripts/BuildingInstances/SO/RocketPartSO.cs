using UnityEngine;

namespace BuildingSystem
{
    
    [CreateAssetMenu(fileName = "RocketPartSo", menuName = "ScriptableObjects/Building/RocketPartSo")]
    public class RocketPartSO : PlaceableSO
    {
        public RocketPartType RocketPartType;
    }
    
    public enum RocketPartType
    {
        Hull,
        Engine,
    }
}
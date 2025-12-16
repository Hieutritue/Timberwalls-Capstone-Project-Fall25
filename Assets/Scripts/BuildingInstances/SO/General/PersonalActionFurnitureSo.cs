using System.Collections.Generic;
using DefaultNamespace.ColonistSystem;
using UnityEngine;

namespace BuildingSystem
{
    [CreateAssetMenu(fileName = "PersonalActionFurnitureSO", menuName = "ScriptableObjects/Building/PersonalActionFurnitureSO", order = 1)]
    public class PersonalActionFurnitureSo : PlaceableSO
    {
        [Header("Colonist Stat Multipliers")]
        public Dictionary<StatType,float> StatMultipliers;
        public List<ResourceWithAmount> Consumption;
    }
}
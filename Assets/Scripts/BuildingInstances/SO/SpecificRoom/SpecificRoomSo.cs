using System.Collections.Generic;
using DefaultNamespace.ColonistSystem;
using Pathfinding.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuildingSystem.SpecificRoom
{
    [CreateAssetMenu(fileName = "SpecificRoomSO", menuName = "ScriptableObjects/SpecificRoomSO", order = 1)]
    public class SpecificRoomSo : SerializedScriptableObject
    {
        public Dictionary<StatType, float> StatMultipliers;
        public List<BuildingSubCategory> RequiredFurnitureSubCategories;
        public List<BuildingSubCategory> MustNotHaveFurnitureSubCategories;
    }
}
using System;
using System.Collections.Generic;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.EffectSystem.SO;
using UnityEngine;

namespace BuildingSystem
{
    [CreateAssetMenu(fileName = "ColonistMultiplierAffectingFurnitureSO", menuName = "ScriptableObjects/Furniture/ColonistMultiplierAffectingFurnitureSO", order = 2)]
    public class ColonistMultiplierAffectingFurnitureSO : FurnitureSO
    {
        [Header("Colonist Stat Multipliers")]
        public ColonistMultiplierModifyingSO ColonistMultiplierModifyingSo;
    }
    
    [Serializable]
    public class StatWithMultiplier
    {
        public StatType StatType;
        public float Multiplier;
    }
}
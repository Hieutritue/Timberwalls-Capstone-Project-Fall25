using System.Collections.Generic;
using BuildingSystem;
using UnityEngine;

namespace DefaultNamespace.EffectSystem.SO
{
    [CreateAssetMenu(fileName = "ColonistMultiplierModifyingSO", menuName = "ScriptableObjects/Effect/ColonistMultiplierModifyingSO", order = 1)]
    public class ColonistMultiplierModifyingSO : ScriptableObject
    {
        public List<StatWithMultiplier> StatIncreaseMultipliers;
        public List<StatWithMultiplier> StatDecreaseMultipliers;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using ResourceSystem; // Uses your existing ResourceSO

namespace BuildingSystem
{
    public enum BuildingCategory
    {
        ResourceGathering,
        Science,
        Bedroom,
        Bathroom,
        Kitchen,
        Decoration,
        Medical,
        Farm,
        Rocket,
        Building,
        Shield,
        Weapon,
    }

    [Serializable]
    public class ResourceWithAmount
    {
        public ResourceSO Resource;
        public int Amount;
    }

    [CreateAssetMenu(menuName = "Game/BuildingData", fileName = "NewBuilding")]
    public class FurnitureSO : ScriptableObject
    {
        [Header("Basic Info")]
        public string Name;
        public Sprite Icon;
        public BuildingCategory Category;
        [TextArea] public string Description;

        [Header("Construction")]
        public List<ResourceWithAmount> Costs;
        public float BaseBuildTime;
    }
}
using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingSystem
{
    [CreateAssetMenu(fileName = "ProductionBuildingSO", menuName = "ScriptableObjects/Building/ResourceGatheringFurnitureSO", order = 1)]
    public class ResourceGatheringFurnitureSo : PlaceableSO
    {
        [Header("Production")]
        public float BaseTimeToProduce;
        public List<ResourceWithAmount> Consumption;
        public List<ResourceWithAmount> OutputResource;
    }
}
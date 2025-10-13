using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace BuildingSystem
{
    public class ProductionFurnitureSO : FurnitureSO
    {
        [Header("Production")]
        public float BaseProductionPerMin; // resource/unit per min
        public List<ResourceWithAmount> Consumption; // resources consumed per unit
        public List<ResourceWithAmount> OutputResource;
    }
}
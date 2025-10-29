

using UnityEngine;

namespace BuildingSystem
{
    public class Furniture : Building
    {

        public Transform ActionPoint;
        public float GetActualBuildTime(float engineeringLevel)
        {
            return PlaceableSo.BaseBuildTime * (1 - engineeringLevel * 0.05f);
        }
    }
}
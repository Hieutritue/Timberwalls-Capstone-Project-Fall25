using UnityEngine;

namespace DefaultNamespace
{
    public class BuildingSystemManager : MonoSingleton<BuildingSystemManager>
    {
        public PlacementSystem PlacementSystem;
        public Grid Grid;
        public GameObject CellIndicator;
        public ObjectPlacer ObjectPlacer;
        [Header("Material")]
        public MaterialSwapper MaterialSwapper;
        public Material PlaceableMaterial;
        public Material NotPlaceableMaterial;
        public Material RemovePlaceableMaterial;
        public Material UnderConstructionMaterial;
        [Header("Ray")]
        public float RayDistance = 100f;
        public LayerMask RayTargetLayers;
    }
}
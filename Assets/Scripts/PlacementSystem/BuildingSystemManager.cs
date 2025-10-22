using UnityEngine;

namespace DefaultNamespace
{
    public class BuildingSystemManager : MonoSingleton<BuildingSystemManager>
    {
        public InputManager InputManager;
        public PlacementSystem PlacementSystem;
        public Grid Grid;
        public GameObject CellIndicator;
        public ObjectPlacer ObjectPlacer;
        public MaterialSwapper MaterialSwapper;
        public Material PlaceableMaterial;
        public Material NotPlaceableMaterial;
        public Material RemovePlaceableMaterial;
        public Material UnderConstructionMaterial;
    }
}
using System.Linq;
using _Scripts.StateMachine;
using BuildingSystem;
using DefaultNamespace.PlaceableInstances;
using DefaultNamespace.TaskSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.PlacementStates
{
    public class DeleteModePlacementState : AState<PlacementSystem>
    {
        private PlaceableType _placeableType;
        private PlaceableInstance _lastPlaceableInstance;

        public DeleteModePlacementState(PlacementSystem behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            InputManager.Instance.OnMouseLeftClick += CreateTaskDemolishPlaceableAtMouse;
            // InputManager.Instance.OnMouseRightClick += _behaviour.TransitionToIdleState;
            // InputManager.Instance.OnMouseRightClick += ResetMaterial;

            _lastPlaceableInstance = null;
            _behaviour.ResetLastGridPosition();
        }

        private void CreateTaskDemolishPlaceableAtMouse()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            // get placeable at mouse position
            var gridPosition = _behaviour.GridPositionOfMouse(_behaviour.MousePosition);
            var gridData = _behaviour.GetGridData(_placeableType);
            var placeableInstance = gridData.GetPlaceableInstanceAt(gridPosition);

            if (!placeableInstance) return;

            var building = placeableInstance.GetComponent<Building>();
            
            if (building && (building.IsUnderConstruction() 
                             || building.IsDemolishing())) 
                return;

            building.TransitionToDemolishing();
            if (building.ActiveTasks.Any(t=>t is DemolishingTask)) return;
            var demolishingTask = new DemolishingTask(building, TaskType.Demolishing);
            demolishingTask.OnComplete += () =>
            {
                ResourceManager.Instance.RefundResourcesForPlaceable(placeableInstance.PlaceableSo);
                CheckRemoval(placeableInstance);
                gridData.RemovePlaceableInstanceOccupiedAt(gridPosition);
            };
            building.ActiveTasks.Add(demolishingTask);
        }

        // remove placeable from any containers or rooms before deleting
        private void CheckRemoval(PlaceableInstance placeableInstance)
        {
            if (placeableInstance is FurniturePlaceableInstance itemInstance)
            {
                itemInstance.ContainingRoomPlaceable?.RemoveItemFromRoom(itemInstance);
            }
            else if (placeableInstance is RoomPlaceableInstance roomInstance)
            {
                foreach (var containedItem in roomInstance.ContainedItems.ToArray())
                {
                    _behaviour.GetGridData(PlaceableType.Furniture).RemovePlaceableInstance(containedItem);
                }

                roomInstance.ContainedItems.Clear();
            }
        }

        public override void Tick()
        {
            var gridPosition = _behaviour.GridPositionOfMouse(_behaviour.MousePosition);
            var placeableInstance = _behaviour.GetGridData(_placeableType).GetPlaceableInstanceAt(gridPosition);

            var building = placeableInstance?.GetComponent<Building>();

            if (building && (placeableInstance == _lastPlaceableInstance 
                             || building.IsUnderConstruction() 
                             || building.IsDemolishing())) 
                return;

            var materialSwapper = BuildingSystemManager.Instance.MaterialSwapper;
            if (_lastPlaceableInstance)
            {
                ResetMaterial();
            }
            
            if (placeableInstance)
            {
                materialSwapper.ApplyHighlight(placeableInstance.gameObject,
                    BuildingSystemManager.Instance.RemovePlaceableMaterial);
            }
            
            _lastPlaceableInstance = placeableInstance;
            
        }

        public override void Exit()
        {
            InputManager.Instance.OnMouseLeftClick -= CreateTaskDemolishPlaceableAtMouse;
            if (_lastPlaceableInstance) ResetMaterial();
            // InputManager.Instance.OnMouseRightClick -= _behaviour.TransitionToIdleState;
            // InputManager.Instance.OnMouseRightClick -= ResetMaterial;
        }

        public void ChangePlaceableType(PlaceableType placeableType)
        {
            _placeableType = placeableType;
        }

        public void ResetMaterial()
        {
            var lastBuilding = _lastPlaceableInstance?.GetComponent<Building>();
            if (lastBuilding && (lastBuilding.IsUnderConstruction() || lastBuilding.IsDemolishing()))
                return;
            if (_lastPlaceableInstance)
                BuildingSystemManager.Instance.MaterialSwapper.RemoveHighlight(_lastPlaceableInstance.gameObject);
        }
    }
}
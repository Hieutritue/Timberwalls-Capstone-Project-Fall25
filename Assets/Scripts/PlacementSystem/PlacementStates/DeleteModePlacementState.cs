using _Scripts.StateMachine;
using BuildingSystem;
using DefaultNamespace.PlaceableInstances;
using DefaultNamespace.TaskSystem;
using UnityEngine;

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
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick += CreateTaskDemolishPlaceableAtMouse;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick += _behaviour.ExitPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick += ResetMaterial;

            _lastPlaceableInstance = null;
            _behaviour.ResetLastGridPosition();
        }

        private void CreateTaskDemolishPlaceableAtMouse()
        {
            // get placeable at mouse position
            var gridPosition = _behaviour.GridPositionOfMouse(_behaviour.MousePosition);
            var gridData = _behaviour.GetGridData(_placeableType);
            var placeableInstance = gridData.GetPlaceableInstanceAt(gridPosition);

            if (!placeableInstance) return;

            var building = placeableInstance.GetComponent<Building>();

            building.TransitionToDemolishing();
            var demolishingTask = new DemolishingTask(building, TaskType.Demolishing);
            demolishingTask.OnComplete += () =>
            {
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
                             || building.IsDemolishing())) return;

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
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick -= CreateTaskDemolishPlaceableAtMouse;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick -= _behaviour.ExitPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick -= ResetMaterial;
        }

        public void ChangePlaceableType(PlaceableType placeableType)
        {
            _placeableType = placeableType;
        }

        public void ResetMaterial()
        {
            if (_lastPlaceableInstance)
                BuildingSystemManager.Instance.MaterialSwapper.RemoveHighlight(_lastPlaceableInstance.gameObject);
        }
    }
}
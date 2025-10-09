using _Scripts.StateMachine;
using DefaultNamespace.PlaceableInstances;
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
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick += RemovePlaceableAtMouse;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick += _behaviour.ExitPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick += ResetMaterial;
            
            _lastPlaceableInstance = null;
            _behaviour.ResetLastGridPosition();
        }

        private void RemovePlaceableAtMouse()
        {
            var gridPosition = _behaviour.GridPositionOfMouse(_behaviour.MousePosition);
            var gridData = _behaviour.GetGridData(_placeableType);
            var placeableInstance = gridData.GetPlaceableInstanceAt(gridPosition);
            
            if (!placeableInstance) return;
            
            CheckRemoval(placeableInstance);
            gridData.RemovePlaceableInstanceOccupiedAt(gridPosition); 
        }
        
        
        private void CheckRemoval(PlaceableInstance placeableInstance)
        {
            if (placeableInstance is ItemInstance itemInstance)
            {
                itemInstance.ContainingRoom?.RemoveItemFromRoom(itemInstance);
            }
            else if (placeableInstance is RoomInstance roomInstance)
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
            
            if (placeableInstance == _lastPlaceableInstance) return;
            
            var materialSwapper = BuildingSystemManager.Instance.MaterialSwapper;
            if (_lastPlaceableInstance)
            {
                ResetMaterial();
            }
            if (placeableInstance)
            {
                materialSwapper.ApplyHighlight(placeableInstance.gameObject, BuildingSystemManager.Instance.RemovePlaceableMaterial);
            }
            
            _lastPlaceableInstance = placeableInstance;
        }
        public override void Exit()
        {
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick -= RemovePlaceableAtMouse;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick -= _behaviour.ExitPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick -= ResetMaterial;
        }
        
        public void ChangePlaceableType(PlaceableType placeableType)
        {
            _placeableType = placeableType;
        }
        
        public void ResetMaterial()
        {
            BuildingSystemManager.Instance.MaterialSwapper.Restore();
        }
    }
}
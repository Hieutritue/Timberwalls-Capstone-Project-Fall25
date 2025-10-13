using System.Collections;
using System.Collections.Generic;
using _Scripts.StateMachine;
using UnityEngine;

namespace DefaultNamespace.PlacementStates
{
    public class PreviewPlacementState : AState<PlacementSystem>
    {
        
        private int _currentPlaceableIndex = -1;
        public PreviewPlacementState(PlacementSystem behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            BuildingSystemManager.Instance.InputManager.OnClickNum += _behaviour.EnterPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick += _behaviour.ExitPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick += PlaceCurrentObject;
            HidePreview();
            ShowPreview();
        }

        public override void Tick()
        {
            var gridPosition = _behaviour.GridPositionOfMouse(_behaviour.MousePosition);
            ChangePreviewMaterial(gridPosition);
            BuildingSystemManager.Instance.CellIndicator.transform.position =
                BuildingSystemManager.Instance.Grid.CellToWorld(gridPosition);
        }

        public override void Exit()
        {
            BuildingSystemManager.Instance.InputManager.OnClickNum -= _behaviour.EnterPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseRightClick -= _behaviour.ExitPlacementMode;
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick -= PlaceCurrentObject;
            
            HidePreview();
        }
        
        

        public PlaceableSO GetCurrentObjectToPlace()
        {
            return _behaviour.PlaceableCollectionSo.GetPlaceableByIndex(_currentPlaceableIndex);
        }
        
        private void ChangePreviewMaterial(Vector3Int gridPosition)
        {
            var material = CheckPlacementValidity(gridPosition, GetCurrentObjectToPlace())
                ? BuildingSystemManager.Instance.PlaceableMaterial
                : BuildingSystemManager.Instance.NotPlaceableMaterial;
            BuildingSystemManager.Instance.MaterialSwapper.ApplyHighlight(
                BuildingSystemManager.Instance.CellIndicator.transform.GetChild(0).gameObject, material);
        }
        
        private bool CheckPlacementValidity(Vector3Int gridPosition, PlaceableSO placeableSo)
        {
            var selectedData = _behaviour.GetGridData(placeableSo.Type);
            var objectToPlace = GetCurrentObjectToPlace();
            return selectedData != null && selectedData.CanPlaceAt(gridPosition, objectToPlace, _behaviour.GetGridData(PlaceableType.Room));
        }
        
        public void PlaceCurrentObject()
        {
            if (_currentPlaceableIndex == -1) return;
            Vector3 mousePosition = _behaviour.MousePosition;
            Vector3Int gridPosition = _behaviour.GridPositionOfMouse(mousePosition);
            Vector3 spawnPosition = BuildingSystemManager.Instance.Grid.CellToWorld(gridPosition);

            var canPlace = CheckPlacementValidity(gridPosition, GetCurrentObjectToPlace());
            if (!canPlace) return;

            var newPlaceable = BuildingSystemManager.Instance.ObjectPlacer.PlaceObject(GetCurrentObjectToPlace()?.Prefab, spawnPosition);
            var placeableInstance = _behaviour.GetGridData(GetCurrentObjectToPlace().Type).AddObjectAt(gridPosition,
                GetCurrentObjectToPlace(),
                newPlaceable
            );
            
            if(placeableInstance.PlaceableSo.Type != PlaceableType.Room)
                AssignItemToRoom(placeableInstance, spawnPosition);

            _behaviour.ResetLastGridPosition();
        }

        private void AssignItemToRoom(PlaceableInstance itemInstance, Vector3 spawnPosition)
        {
            var roomInstance = _behaviour.GetGridData(PlaceableType.Room).GetPlaceableInstanceAt(Vector3Int.FloorToInt(spawnPosition)) as PlaceableInstances.RoomInstance;
            if (roomInstance)
            {
                roomInstance.AddItemToRoom(itemInstance as PlaceableInstances.ItemInstance);
            }
        }
        
        
        public void ShowPreview()
        {
            _behaviour.InstantiatePreviewObject(GetCurrentObjectToPlace()?.Prefab);
            _behaviour.StartCoroutine(DelayChangeMaterial());
        }
        
        private IEnumerator DelayChangeMaterial()
        {
            yield return null;
            ChangePreviewMaterial(_behaviour.GridPositionOfMouse(_behaviour.MousePosition));
        }
        public void HidePreview()
        {
            _behaviour.DestroyPreviewObject();
            BuildingSystemManager.Instance.CellIndicator.SetActive(false);
        }
        
        public void ChangeCurrentPlaceableIndex(int index)
        {
            _currentPlaceableIndex = index;
            HidePreview();
            ShowPreview();
        }

    }
}
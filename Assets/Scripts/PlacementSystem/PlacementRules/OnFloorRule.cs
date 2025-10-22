using UnityEngine;

namespace DefaultNamespace.PlacementRules
{
    public class OnFloorRule : IPlacementRule
    {
        private Vector2Int _objectSize;
        private Vector3Int _gridPosition;

        public OnFloorRule(Vector2Int objectSize, Vector3Int gridPosition)
        {
            _objectSize = objectSize;
            _gridPosition = gridPosition;
        }

        public bool IsValid(GridData gridData)
        {
            var belowPos = new Vector3Int(_gridPosition.x, _gridPosition.y, _gridPosition.z);
            var placeableInstance = gridData.GetPlaceableInstanceAt(belowPos);
            var floorOfInstanceContainingBelowPos = placeableInstance?.GetFloorCells();

            return floorOfInstanceContainingBelowPos != null 
                   && floorOfInstanceContainingBelowPos.Contains(belowPos) 
                   && !placeableInstance.Building.IsUnderConstruction();
        }
    }
}
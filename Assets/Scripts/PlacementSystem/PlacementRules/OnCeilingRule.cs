using UnityEngine;

namespace DefaultNamespace.PlacementRules
{
    public class OnCeilingRule : IPlacementRule
    {
        private Vector2Int _objectSize;
        private Vector3Int _gridPosition;

        public OnCeilingRule(Vector2Int objectSize, Vector3Int gridPosition)
        {
            _objectSize = objectSize;
            _gridPosition = gridPosition;
        }
        public bool IsValid(GridData gridData)
        {
            var topPos = new Vector3Int(_gridPosition.x, _gridPosition.y + _objectSize.y - 1, _gridPosition.z);
            var ceilingOfInstanceContainingBelowPos = gridData.GetPlaceableInstanceAt(topPos)?.GetCeilingCells();
            
            return ceilingOfInstanceContainingBelowPos != null && ceilingOfInstanceContainingBelowPos.Contains(topPos);
        }
    }
}
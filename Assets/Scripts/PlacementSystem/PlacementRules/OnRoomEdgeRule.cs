using UnityEngine;

namespace DefaultNamespace.PlacementRules
{
    public class OnRoomEdgeRule : IPlacementRule
    {
        private Vector2Int _objectSize;
        private Vector3Int _gridPosition;

        public OnRoomEdgeRule(Vector2Int objectSize, Vector3Int gridPosition)
        {
            _objectSize = objectSize;
            _gridPosition = gridPosition;
        }
        public bool IsValid(GridData gridData)
        {
            var leftPos = new Vector3Int(_gridPosition.x, _gridPosition.y, _gridPosition.z);
            var rightPos = new Vector3Int(_gridPosition.x + _objectSize.x - 1, _gridPosition.y, _gridPosition.z);
            var edgesOfInstanceContainingBelowPos = gridData.GetPlaceableInstanceAt(leftPos)?.GetEdgeCells();
            
            return edgesOfInstanceContainingBelowPos != null 
                   && (edgesOfInstanceContainingBelowPos.Contains(leftPos) 
                       || edgesOfInstanceContainingBelowPos.Contains(rightPos));
        }
    }
}
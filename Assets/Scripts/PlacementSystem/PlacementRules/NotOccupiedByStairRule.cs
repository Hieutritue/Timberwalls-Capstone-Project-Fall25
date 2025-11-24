using UnityEngine;

namespace DefaultNamespace.PlacementRules
{
    public class NotOccupiedByStairRule : IPlacementRule
    {
        private Vector2Int _objectSize;
        private Vector3Int _gridPosition;

        public NotOccupiedByStairRule(Vector2Int objectSize, Vector3Int gridPosition)
        {
            _objectSize = objectSize;
            _gridPosition = gridPosition;
        }
        public bool IsValid(GridData gridData)
        {
            var positionToOccupy = gridData.CalculatePositions(_gridPosition, _objectSize);
            // Only return false if one of the positions is already occupied by a stair.
            foreach (var pos in positionToOccupy)
            {
                if (gridData.PlacedInstances.TryGetValue(pos, out var instance) && instance != null && instance.PlaceableSo != null)
                {
                    if (instance.PlaceableSo.IsStair)
                        return false;
                }
            }

            return true;
        }
    }
}
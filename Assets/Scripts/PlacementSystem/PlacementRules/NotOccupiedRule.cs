using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.PlacementRules
{
    public class NotOccupiedRule : IPlacementRule
    {
        private Vector2Int _objectSize;
        private Vector3Int _gridPosition;

        public NotOccupiedRule(Vector2Int objectSize, Vector3Int gridPosition)
        {
            _objectSize = objectSize;
            _gridPosition = gridPosition;
        }
        public bool IsValid(GridData gridData)
        {
            var positionToOccupy = gridData.CalculatePositions(_gridPosition, _objectSize);
            return positionToOccupy.All(pos => !gridData.PlacedInstances.ContainsKey(pos));
        }
    }
}
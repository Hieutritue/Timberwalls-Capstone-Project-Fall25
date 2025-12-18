using UnityEngine;

namespace DefaultNamespace.PlacementRules
{
    public class RocketRule : IPlacementRule
    {
        private Vector3Int _pos;

        public RocketRule(Vector3Int pos)
        {
            _pos = pos;
        }

        public bool IsValid(GridData gridData)
        {
            var highTechRoom = gridData.GetPlaceableInstanceAt(_pos);
            if (!highTechRoom || highTechRoom.PlaceableSo.Size.x != 20 || highTechRoom.Building.IsUnderConstruction()) return false;
            return _pos.x == highTechRoom.OccupiedCells[0].x + 7 && _pos.y == highTechRoom.OccupiedCells[0].y + 1;
        }
    }
}
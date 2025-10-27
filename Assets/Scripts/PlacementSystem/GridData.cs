using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.PlacementRules;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class GridData
    {
        public Dictionary<Vector3Int, PlaceableInstance> PlacedInstances { get; } = new();

        public PlaceableInstance AddObjectAt(Vector3Int gridPosition,
            PlaceableSO placeableSo,
            GameObject gameObject
            )
        {
            var positionToOccupy = CalculatePositions(gridPosition, placeableSo.Size);
            
            var placeableInstance = RegisterInstance(positionToOccupy, placeableSo, gameObject);
            
            foreach (var position in positionToOccupy.Where(position => !PlacedInstances.TryAdd(position, placeableInstance)))
            {
                throw new Exception($"Occupied position at {position}");
            }

            return placeableInstance;
        }

        private PlaceableInstance RegisterInstance(List<Vector3Int> positionToOccupy, PlaceableSO placeableSo, GameObject gameObject)
        {
            switch (placeableSo.Type)
            {
                case PlaceableType.Room:
                    var roomInstance = gameObject.AddComponent<PlaceableInstances.RoomPlaceableInstance>();
                    roomInstance.OccupiedCells = positionToOccupy;
                    roomInstance.PlaceableSo = placeableSo;
                    return roomInstance;
                default:
                    var itemInstance = gameObject.AddComponent<PlaceableInstances.FurniturePlaceableInstance>();
                    itemInstance.OccupiedCells = positionToOccupy;
                    itemInstance.PlaceableSo = placeableSo;
                    return itemInstance;
            }
        }

        public List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
        {
            List<Vector3Int> returnVal = new();
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    returnVal.Add(new Vector3Int(gridPosition.x + x, gridPosition.y + y, gridPosition.z));
                }
            }

            return returnVal;
        }
        
        public bool CanPlaceAt(Vector3Int gridPosition, PlaceableSO placeableSo, GridData roomGridData = null)
        {
            // Then check conditions
            foreach (var condition in placeableSo.PlacementConditions)
            {
                switch (condition)
                {
                    case PlacementConditionType.NotOccupied:
                        var rule1 = new NotOccupiedRule(placeableSo.Size, gridPosition);
                        if (!rule1.IsValid(this))
                            return false;
                        break;
                    case PlacementConditionType.OnFloor:
                        var rule2 = new OnFloorRule(placeableSo.Size, gridPosition);
                        if (!rule2.IsValid(roomGridData))
                            return false;
                        break;
                    case PlacementConditionType.None:
                        break;
                    case PlacementConditionType.OnEdge:
                        var rule3 = new OnRoomEdgeRule(placeableSo.Size, gridPosition);
                        if (!rule3.IsValid(roomGridData))
                            return false;
                        break;
                    case PlacementConditionType.OnCeiling:
                        var rule4 = new OnCeilingRule(placeableSo.Size, gridPosition);
                        if (!rule4.IsValid(roomGridData))
                            return false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return true;
        }
        
        public PlaceableInstance GetPlaceableInstanceAt(Vector3Int gridPosition)
        {
            PlacedInstances.TryGetValue(gridPosition, out var placeableInstance);
            return placeableInstance;
        }

        
        public void RemovePlaceableInstance(PlaceableInstance placeableInstance)
        {
            foreach (var cell in placeableInstance.OccupiedCells)
            {
                PlacedInstances.Remove(cell);
            }
            placeableInstance.DestroyGameObject();
        }
        
        public PlaceableInstance RemovePlaceableInstanceOccupiedAt(Vector3Int gridPosition)
        {
            if (!PlacedInstances.TryGetValue(gridPosition, out var placeableInstance)) return null;
            RemovePlaceableInstance(placeableInstance);
            return placeableInstance;
        }
    }

}
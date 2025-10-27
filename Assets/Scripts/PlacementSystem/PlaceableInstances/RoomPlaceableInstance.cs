using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.PlaceableInstances
{
    public class RoomPlaceableInstance : PlaceableInstance
    {
        public List<FurniturePlaceableInstance> ContainedItems { get; set; } = new List<FurniturePlaceableInstance>();
        

        public void AddItemToRoom(FurniturePlaceableInstance furniturePlaceableInstance)
        {
            ContainedItems.Add(furniturePlaceableInstance);
            furniturePlaceableInstance.ContainingRoomPlaceable = this;
        }
        
        public void RemoveItemFromRoom(FurniturePlaceableInstance furniturePlaceableInstance)
        {
            ContainedItems.Remove(furniturePlaceableInstance);
            furniturePlaceableInstance.ContainingRoomPlaceable = null;
        }
    }
}
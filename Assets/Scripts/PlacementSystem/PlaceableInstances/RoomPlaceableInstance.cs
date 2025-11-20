using System;
using System.Collections.Generic;
using BuildingSystem;
using UnityEngine;

namespace DefaultNamespace.PlaceableInstances
{
    public class RoomPlaceableInstance : PlaceableInstance
    {
        public List<FurniturePlaceableInstance> ContainedItems { get; set; } = new List<FurniturePlaceableInstance>();

        public Action<Furniture> OnAddFurnitureToRoom;
        public Action<Furniture> OnRemoveFurnitureFromRoom;

        public void AddItemToRoom(FurniturePlaceableInstance furniturePlaceableInstance)
        {
            ContainedItems.Add(furniturePlaceableInstance);
            furniturePlaceableInstance.ContainingRoomPlaceable = this;

            furniturePlaceableInstance.Building = furniturePlaceableInstance.GetComponent<Furniture>();
            OnAddFurnitureToRoom?.Invoke((Furniture)furniturePlaceableInstance.Building);
        }

        public void RemoveItemFromRoom(FurniturePlaceableInstance furniturePlaceableInstance)
        {
            ContainedItems.Remove(furniturePlaceableInstance);
            furniturePlaceableInstance.ContainingRoomPlaceable = null;

            OnRemoveFurnitureFromRoom?.Invoke((Furniture)furniturePlaceableInstance.Building);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.PlaceableInstances
{
    public class RoomInstance : PlaceableInstance
    {
        public List<ItemInstance> ContainedItems { get; set; } = new List<ItemInstance>();
        

        public void AddItemToRoom(ItemInstance itemInstance)
        {
            ContainedItems.Add(itemInstance);
            itemInstance.ContainingRoom = this;
        }
        
        public void RemoveItemFromRoom(ItemInstance itemInstance)
        {
            ContainedItems.Remove(itemInstance);
            itemInstance.ContainingRoom = null;
        }
    }
}
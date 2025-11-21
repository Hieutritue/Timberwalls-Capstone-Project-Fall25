

using UnityEngine;

namespace BuildingSystem
{
    public class Furniture : Building
    {
        public Room ContainingRoom;
        
        public void InitFurniture(Room room)
        {
            ContainingRoom = room;
        }
    }
}
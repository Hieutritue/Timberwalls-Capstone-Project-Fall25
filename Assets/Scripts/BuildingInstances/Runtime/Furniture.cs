

using UnityEngine;

namespace BuildingSystem
{
    public class Furniture : Building
    {
        public Room ContainingRoom;

        public override void Start()
        {
            base.Start();
            SetCollidersToTrigger(true);
        }

        public void InitFurniture(Room room)
        {
            ContainingRoom = room;
        }
    }
}
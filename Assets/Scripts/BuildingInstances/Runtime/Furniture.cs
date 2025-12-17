using Assets.Scripts.UI.WorldSpaceUISystem.ConstructTooltip;
using UnityEngine;
using UnityEngine.Playables;

namespace BuildingSystem
{
    public class Furniture : Building
    {
        public Room ContainingRoom;
        public FurnitureMouseEventController MouseEventController;

        public override void Start()
        {
            base.Start();
            //SetCollidersToTrigger(true);
            MouseEventController.Setup(this);
        }

        public void InitFurniture(Room room)
        {
            ContainingRoom = room;
        }

        public virtual FurnitureTooltipData GetTooltipData()
        {
            return new BasicFurnitureTooltipData
            {
                Name = PlaceableSo.Name,
                Description = PlaceableSo.Description
            };
        }
    }
}
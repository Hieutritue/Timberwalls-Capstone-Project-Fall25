using System.Linq;

namespace BuildingSystem
{
    public class HighTechRoom : Room
    {
        public override void CheckRoomFurniture()
        {
            if (_roomPlaceableInstance.ContainedItems.Count(f =>
                    f.PlaceableSo.Name.Equals("Battery Cargo") && !f.Building.IsUnderConstruction()) == 2 &&
                _roomPlaceableInstance.ContainedItems.Any(f =>
                    f.PlaceableSo.Name.Equals("Bonium Rocket Hull") && !f.Building.IsUnderConstruction()))
            {
                WinGame();
            }
        }

        public void WinGame()
        {
            GameManager.Instance.Win();
        }
    }
}
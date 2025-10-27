using _Scripts.StateMachine;
using DefaultNamespace;

namespace BuildingSystem.RoomStates
{
    public class DemolishingBuildingState : AState<Building>
    {
        public DemolishingBuildingState(Building behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            BuildingSystemManager.Instance.MaterialSwapper.ApplyHighlight(_behaviour.gameObject,
                BuildingSystemManager.Instance.RemovePlaceableMaterial);
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
        }
    }
}
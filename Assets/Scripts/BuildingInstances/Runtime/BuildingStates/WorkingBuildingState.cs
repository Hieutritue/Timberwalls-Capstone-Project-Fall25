using _Scripts.StateMachine;

namespace BuildingSystem.RoomStates
{
    public class WorkingBuildingState : AState<Building>
    {
        public WorkingBuildingState(Building behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
            if (_behaviour is ResourceGatheringFurniture gatheringFurniture)
            {
                gatheringFurniture.ClearTask();
            }
        }
    }
}
using _Scripts.StateMachine;
using DefaultNamespace.TaskSystem;

namespace BuildingSystem.RoomStates
{
    public class IdleBuildingState : AState<Building>
    {
        public IdleBuildingState(Building behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            if(_behaviour is ResourceGatheringFurniture gatheringFurniture)
            {
                gatheringFurniture.CreateTask();
            }
        }

        public override void Tick()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
using System.Linq;
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
        }

        public override void Tick()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
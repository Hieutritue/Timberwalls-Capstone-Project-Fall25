
using _Scripts.StateMachine;
namespace BuildingSystem.RoomStates
{
    public class IdleBuildingState : AState<Building>
    {
        public IdleBuildingState(Building behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            if(_behaviour is Extercom extercom) extercom.AddContactPoint();
        }

        public override void Tick()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
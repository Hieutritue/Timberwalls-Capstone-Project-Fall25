using _Scripts.StateMachine;

namespace BuildingSystem.States
{
    public class IdleFurnitureState : AState<Furniture>
    {
        public IdleFurnitureState(Furniture behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
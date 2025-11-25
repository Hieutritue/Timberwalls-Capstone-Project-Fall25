using _Scripts.StateMachine;

namespace DefaultNamespace.ColonistSystem.States
{
    public class RunningToFacilityColonistState : AState<Colonist>
    {
        public RunningToFacilityColonistState(Colonist behaviour) : base(behaviour)
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
            throw new System.NotImplementedException();
        }
    }
}
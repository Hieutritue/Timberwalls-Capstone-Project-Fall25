using _Scripts.StateMachine;

namespace DefaultNamespace.ColonistSystem.States
{
    public class WorkingColonistState : AState<Colonist>
    {
        public WorkingColonistState(Colonist behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            _behaviour.CurrentTask.OnComplete += _behaviour.TransitionToIdle;
        }

        public override void Tick()
        {
            _behaviour.CurrentTask.UpdateProgress(_behaviour);
        }

        public override void Exit()
        {
            
        }
        
    }
}
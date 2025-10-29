using _Scripts.StateMachine;

namespace DefaultNamespace.ColonistSystem.States
{
    public class RunningToWorkColonistState : AState<Colonist>
    {
        public RunningToWorkColonistState(Colonist behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            _behaviour.AiDestinationSetter.enabled = true;
            _behaviour.FollowerEntity.enabled = true;
        }

        public override void Tick()
        {
            _behaviour.RunToTask();
        }

        public override void Exit()
        {
            _behaviour.AiDestinationSetter.enabled = false;
            _behaviour.FollowerEntity.enabled = false;
        }
    }
}
using _Scripts.StateMachine;
using DefaultNamespace.TaskSystem;

namespace DefaultNamespace.ColonistSystem.States
{
    public class WorkingColonistState : AState<Colonist>
    {
        private ITask _currentTask;
        public WorkingColonistState(Colonist behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            _currentTask = _behaviour.CurrentTask;
            _behaviour.AiDestinationSetter.enabled = false;
            _behaviour.FollowerEntity.enabled = false;
            _behaviour.CurrentTask.ColonistStartWork(_behaviour);
        }

        public override void Tick()
        {
            if(_currentTask != _behaviour.CurrentTask) return;
            _behaviour.CurrentTask.UpdateProgress(_behaviour);
        }

        public override void Exit()
        {
            _behaviour.CurrentTask?.ColonistStopWork(_behaviour);
            _behaviour.AiDestinationSetter.enabled = true;
            _behaviour.FollowerEntity.enabled = true;
        }
    }
}
using _Scripts.StateMachine;
using DefaultNamespace.TaskSystem;

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
            _behaviour.CurrentTask.OnComplete += _behaviour.CurrentTask.Building.TransitionToIdle;
            _behaviour.CurrentTask.OnRemove += _behaviour.TransitionToIdle;

            if (_behaviour.CurrentTask.TaskType == TaskType.Mining)
            {
                _behaviour.CurrentTask.Building.TransitionToWorking();
            }
        }

        public override void Tick()
        {
            _behaviour.CurrentTask.UpdateProgress(_behaviour);
        }

        public override void Exit()
        {
            _behaviour.CurrentTask.OnComplete -= _behaviour.TransitionToIdle;
            _behaviour.CurrentTask.OnComplete -= _behaviour.CurrentTask.Building.TransitionToIdle;
            _behaviour.CurrentTask.OnRemove -= _behaviour.TransitionToIdle;
        }
    }
}
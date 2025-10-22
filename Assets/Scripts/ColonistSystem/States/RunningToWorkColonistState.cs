using _Scripts.StateMachine;
using DefaultNamespace.General;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem.States
{
    public class RunningToWorkColonistState : AState<Colonist>
    {
        public RunningToWorkColonistState(Colonist behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            _behaviour.CurrentTask.OnRemove += _behaviour.TransitionToIdle;
            RunToTask();
        }

        public override void Tick()
        {
            var distanceToTarget =
                Vector3.Distance(_behaviour.transform.position, _behaviour.CurrentTask.Transform.position);
            if (distanceToTarget < GameManager.Instance.GeneralNumberSO.ConstructionRange)
            {
                _behaviour.StateMachine.TransitionTo(_behaviour.WorkingState);
            }
        }

        public override void Exit()
        {
            _behaviour.AiDestinationSetter.enabled = false;
            _behaviour.CurrentTask.OnRemove -= _behaviour.TransitionToIdle;
        }
        
        public void RunToTask()
        {
            _behaviour.AiDestinationSetter.enabled = true;
            _behaviour.AiDestinationSetter.target = TaskManager.Instance.AssignedTasks[_behaviour].Transform;
        }
    }
}
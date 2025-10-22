using _Scripts.StateMachine;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem.States
{
    public class IdleColonistState : AState<Colonist>
    {
        public IdleColonistState(Colonist behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Tick()
        {
            if(TaskManager.Instance.AssignedTasks.ContainsKey(_behaviour))
            {
                _behaviour.StateMachine.TransitionTo(_behaviour.RunningToWorkState);
                // Debug.Log($"Task found: {_behaviour.CurrentTask}");
            }
        }

        public override void Exit()
        {
            
        }
    }
}
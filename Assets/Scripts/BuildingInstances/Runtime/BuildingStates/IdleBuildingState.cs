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
            if (_behaviour is ITaskCreator taskCreator &&
                !_behaviour.ActiveTasks.Any(t => t is not BuildingTask && t is not DemolishingTask))
            {
                taskCreator.CreateTask();
            }

        }

        public override void Tick()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
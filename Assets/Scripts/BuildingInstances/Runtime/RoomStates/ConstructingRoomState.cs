using _Scripts.StateMachine;
using DefaultNamespace;
using DefaultNamespace.TaskSystem;

namespace BuildingSystem.RoomStates
{
    public class ConstructingRoomState : AState<Room>
    {
        ConstructRoomTask _constructRoomTask;
        public ConstructingRoomState(Room behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            BuildingSystemManager.Instance.MaterialSwapper.ApplyHighlight(_behaviour.gameObject,
                BuildingSystemManager.Instance.UnderConstructionMaterial);
            _constructRoomTask = new ConstructRoomTask(_behaviour);
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
            BuildingSystemManager.Instance.MaterialSwapper.RemoveHighlight(_behaviour.gameObject);
        }
    }
}
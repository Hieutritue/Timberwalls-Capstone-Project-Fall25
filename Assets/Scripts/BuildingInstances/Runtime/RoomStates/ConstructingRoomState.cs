using _Scripts.StateMachine;
using DefaultNamespace;

namespace BuildingSystem.RoomStates
{
    public class ConstructingRoomState : AState<Room>
    {
        public ConstructingRoomState(Room behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            BuildingSystemManager.Instance.MaterialSwapper.ApplyHighlight(_behaviour.gameObject,
                BuildingSystemManager.Instance.UnderConstructionMaterial);
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
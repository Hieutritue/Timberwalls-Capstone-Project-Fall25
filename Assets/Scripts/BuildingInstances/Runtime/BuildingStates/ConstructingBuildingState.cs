using _Scripts.StateMachine;
using DefaultNamespace;
using DefaultNamespace.TaskSystem;

namespace BuildingSystem.RoomStates
{
    public class ConstructingBuildingState : AState<Building>
    {
        public ConstructingBuildingState(Building behaviour) : base(behaviour)
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
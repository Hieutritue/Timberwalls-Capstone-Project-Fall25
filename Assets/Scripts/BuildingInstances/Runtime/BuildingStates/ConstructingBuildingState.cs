using System.Linq;
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
            // _behaviour.Colliders.ToList().ForEach(c => c.enabled = false);
            BuildingSystemManager.Instance.MaterialSwapper.ApplyHighlight(_behaviour.gameObject,
                BuildingSystemManager.Instance.UnderConstructionMaterial);
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
            // _behaviour.SetCollidersToTrigger(false);
            // _behaviour.Colliders.ToList().ForEach(c => c.enabled = true);
            AstarPath.active.Scan();
            BuildingSystemManager.Instance.MaterialSwapper.RemoveHighlight(_behaviour.gameObject);
            _behaviour.Constructed();
            
            if(_behaviour is Room room)
                room.InitRoom();
        }
    }
}
using _Scripts.StateMachine;
using DefaultNamespace;
using UnityEngine;

namespace BuildingSystem.RoomStates
{
    public class DemolishingBuildingState : AState<Building>
    {
        public DemolishingBuildingState(Building behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            BuildingSystemManager.Instance.MaterialSwapper.ApplyHighlight(_behaviour.gameObject,
                BuildingSystemManager.Instance.RemovePlaceableMaterial);
        }

        public override void Tick()
        {
        }

        public override void Exit()
        {
            BuildingSystemManager.Instance.MaterialSwapper.RemoveHighlight(_behaviour.gameObject);
            _behaviour.OnDemolished();
        }
    }
}
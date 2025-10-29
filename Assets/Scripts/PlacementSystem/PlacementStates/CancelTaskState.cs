using _Scripts.StateMachine;
using UnityEngine;

namespace DefaultNamespace.PlacementStates
{
    public class CancelTaskState : AState<CancelTaskState>
    {
        public CancelTaskState(CancelTaskState behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick += CancelTaskPointingAt;
        }

        public override void Tick()
        {
        }

        public void CancelTaskPointingAt()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit,
                    BuildingSystemManager.Instance.RayDistance,
                    BuildingSystemManager.Instance.RayTargetLayers))
            {
                Object.Destroy(hit.collider.gameObject);
                Debug.Log($"Deleted object: {hit.collider.name}");
            }
        }

        public override void Exit()
        {
            BuildingSystemManager.Instance.InputManager.OnMouseLeftClick -= CancelTaskPointingAt;
        }
    }
}
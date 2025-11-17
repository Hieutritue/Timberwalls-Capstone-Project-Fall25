using System.Linq;
using _Scripts.StateMachine;
using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace DefaultNamespace.PlacementStates
{
    public class CancelTaskState : AState<PlacementSystem>
    {
        public CancelTaskState(PlacementSystem behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            InputManager.Instance.OnMouseRightClick += _behaviour.TransitionToIdleState;
            InputManager.Instance.OnMouseLeftClick += CancelTaskPointingAt;
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
                Building building = hit.collider.GetComponentInParent<Building>();

                if (building.IsDemolishing())
                {
                    building.ActiveTasks.FirstOrDefault(t=>t is DemolishingTask)?.RemoveTask();
                    building.TransitionToIdle();
                }

                else if (building.IsUnderConstruction())
                {
                    var placeableInstance = building.GetComponent<PlaceableInstance>();
                    var gridData = _behaviour.GetGridData(placeableInstance.PlaceableSo.Type);
                    gridData.RemovePlaceableInstance(placeableInstance);
                    Object.Destroy(building.gameObject);
                    Debug.Log($"Deleted object: {hit.collider.name}");
                }
            }
        }

        public override void Exit()
        {
            InputManager.Instance.OnMouseLeftClick -= CancelTaskPointingAt;
            InputManager.Instance.OnMouseRightClick -= _behaviour.TransitionToIdleState;
        }
    }
}
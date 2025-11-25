using System.Linq;
using _Scripts.StateMachine;
using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;
using Util;

namespace DefaultNamespace.PlacementStates
{
    public class CancelTaskState : AState<PlacementSystem>
    {
        private Building _building;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit,
                    BuildingSystemManager.Instance.RayDistance,
                    BuildingSystemManager.Instance.RayTargetLayers,
                    QueryTriggerInteraction.Collide))
            {
                Building building = hit.collider.GetComponentInParent<Building>();

                if (_building == building) return;

                if (_building)
                {
                    LayerUtils.SetLayerRecursively(_building.gameObject, LayerMask.NameToLayer("Building"));
                }

                if (!building)
                {
                    return;
                }

                _building = building;
                if (!building.IsUnderConstruction() && !building.IsDemolishing()) return;
                LayerUtils.SetLayerRecursively(building.gameObject, LayerMask.NameToLayer("HoveringBuilding"));
            }
            else
            {
                if (_building)
                {
                    LayerUtils.SetLayerRecursively(_building.gameObject, LayerMask.NameToLayer("Building"));
                    _building = null;
                }
            }
        }

        public void CancelTaskPointingAt()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit,
                    BuildingSystemManager.Instance.RayDistance,
                    BuildingSystemManager.Instance.RayTargetLayers,
                    QueryTriggerInteraction.Collide))
            {
                Building building = hit.collider.GetComponentInParent<Building>();

                if (building.IsDemolishing())
                {
                    building.ActiveTasks.FirstOrDefault(t => t is DemolishingTask)?.RemoveTask();
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
            if (_building)
            {
                LayerUtils.SetLayerRecursively(_building.gameObject, LayerMask.NameToLayer("Building"));
                _building = null;
            }
        }
    }
}
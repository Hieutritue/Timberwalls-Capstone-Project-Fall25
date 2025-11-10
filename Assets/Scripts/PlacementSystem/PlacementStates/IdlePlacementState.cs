using _Scripts.StateMachine;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;


public class IdlePlacementState : AState<PlacementSystem>
{
    public IdlePlacementState(PlacementSystem behaviour) : base(behaviour)
    {
    }

    public override void Enter()
    {
        BuildingSystemManager.Instance.InputManager.OnClickNum += _behaviour.EnterPlacementMode;
        BuildingSystemManager.Instance.InputManager.OnClickRemovePlaceable += _behaviour.EnterDeleteMode;
        BuildingSystemManager.Instance.InputManager.OnClickCancelKey += _behaviour.TransitionToCancelTaskState;
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        BuildingSystemManager.Instance.InputManager.OnClickNum -= _behaviour.EnterPlacementMode;
        BuildingSystemManager.Instance.InputManager.OnClickRemovePlaceable -= _behaviour.EnterDeleteMode;
        BuildingSystemManager.Instance.InputManager.OnClickCancelKey -= _behaviour.TransitionToCancelTaskState;
    }

}
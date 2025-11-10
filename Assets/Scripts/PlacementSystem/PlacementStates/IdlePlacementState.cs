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
        InputManager.Instance.OnClickRemovePlaceable += _behaviour.EnterDeleteMode;
        InputManager.Instance.OnClickCancelKey += _behaviour.EnterCancelMode;
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        InputManager.Instance.OnClickRemovePlaceable -= _behaviour.EnterDeleteMode;
        InputManager.Instance.OnClickCancelKey -= _behaviour.EnterCancelMode;
    }

}
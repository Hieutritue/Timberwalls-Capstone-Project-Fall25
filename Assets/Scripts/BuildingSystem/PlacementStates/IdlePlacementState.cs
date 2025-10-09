using _Scripts.StateMachine;
using DefaultNamespace;
using Unity.VisualScripting;


public class IdlePlacementState : AState<PlacementSystem>
{
    public IdlePlacementState(PlacementSystem behaviour) : base(behaviour)
    {
    }

    public override void Enter()
    {
        BuildingSystemManager.Instance.InputManager.OnClickNum += _behaviour.EnterPlacementMode;
        BuildingSystemManager.Instance.InputManager.OnClickRemovePlaceable += _behaviour.EnterDeleteMode;
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        BuildingSystemManager.Instance.InputManager.OnClickNum -= _behaviour.EnterPlacementMode;
        BuildingSystemManager.Instance.InputManager.OnClickRemovePlaceable -= _behaviour.EnterDeleteMode;
    }

}
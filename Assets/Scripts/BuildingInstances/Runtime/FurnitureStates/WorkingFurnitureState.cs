using _Scripts.StateMachine;
using DefaultNamespace.ColonistSystem;

namespace BuildingSystem.States
{
    public class WorkingFurnitureState : AState<Furniture>
    {
        public Colonist Colonist { get; set; }
        public WorkingFurnitureState(Furniture behaviour) : base(behaviour)
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
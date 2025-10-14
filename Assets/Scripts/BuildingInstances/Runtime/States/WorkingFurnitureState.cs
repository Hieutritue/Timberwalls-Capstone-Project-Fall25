using _Scripts.StateMachine;
using DefaultNamespace.ColonistSystem;

namespace BuildingSystem.States
{
    public class WorkingFurnitureState : AState<FurnitureInstance>
    {
        public Colonist Colonist { get; set; }
        public WorkingFurnitureState(FurnitureInstance behaviour) : base(behaviour)
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
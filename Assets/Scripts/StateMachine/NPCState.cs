using UnityEngine;

namespace _Scripts.StateMachine
{
    public abstract class NPCState : IState
    {
        protected readonly Animator _animator;

        public NPCState(Animator animator)
        {
            _animator = animator;
        }

        public abstract void Enter();

        public abstract void Tick();

        public abstract void FixedUpdate();

        public abstract void Exit();
    }
}

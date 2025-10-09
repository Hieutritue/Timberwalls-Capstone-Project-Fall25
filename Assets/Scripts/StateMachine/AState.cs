using UnityEngine;

namespace _Scripts.StateMachine
{
    public abstract class AState<T> : IState
    {
        protected T _behaviour;

        public AState(T behaviour)
        {
            _behaviour = behaviour;
        }

        public abstract void Enter();

        public abstract void Tick();

        public abstract void Exit();
    }
}
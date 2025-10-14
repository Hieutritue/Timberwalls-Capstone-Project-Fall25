using System;

namespace _Scripts.StateMachine
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        public IState LastState { get; private set; }

        // event to notify other objects of the state change
        public event Action<IState> OnStateChanged;
        
        // set the starting state
        public void Initialize(IState state)
        {
            CurrentState = state;
            state?.Enter();
	

            // notify other objects that state has changed
            OnStateChanged?.Invoke(state);
        }
	

        // exit this state and enter another
        public void TransitionTo(IState nextState)
        {
            if(CurrentState==nextState) return;
            CurrentState?.Exit();
            CurrentState = nextState;
            nextState?.Enter();
	

            // notify other objects that state has changed
            OnStateChanged?.Invoke(nextState);
        }
	

        // allow the StateMachine to update this state
        public void Update()
        {
            CurrentState?.Tick();
        }
    }
}
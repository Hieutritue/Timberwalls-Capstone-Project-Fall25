using _Scripts.StateMachine;
using BuildingSystem.RoomStates;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuildingSystem
{
    public class Room : MonoBehaviour
    {
        public BuildingSO BuildingSo;
        public Transform ProgressPoint;
        
        private StateMachine _stateMachine;
        private ConstructingRoomState _constructingRoomState;
        private IdleRoomState _idleRoomState;
        private PlacingState _placingState;
        
        public void Start()
        {
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine();
            _constructingRoomState = new ConstructingRoomState(this);
            _idleRoomState = new IdleRoomState(this);
            _placingState = new PlacingState(this);
            _stateMachine.Initialize(_constructingRoomState);
        }

        [Button]
        public void SwitchStateToIdle()
        {
            _stateMachine.TransitionTo(_idleRoomState);
        }
        
        public void SwitchStateToConstruction()
        {
            _stateMachine.TransitionTo(_constructingRoomState);
        }
    }
}
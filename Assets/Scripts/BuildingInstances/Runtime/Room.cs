using _Scripts.StateMachine;
using BuildingSystem.RoomStates;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuildingSystem
{
    public class Room : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private ConstructingRoomState _constructingRoomState;
        private IdleRoomState _idleRoomState;
        
        public void Start()
        {
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine();
            _constructingRoomState = new ConstructingRoomState(this);
            _idleRoomState = new IdleRoomState(this);
            _stateMachine.Initialize(_constructingRoomState);
        }

        [Button]
        public void SwitchState()
        {
            _stateMachine.TransitionTo(_idleRoomState);
        }

        [Button]
        public void ChangeMat()
        {
            BuildingSystemManager.Instance.MaterialSwapper.RemoveHighlight(gameObject);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.StateMachine;
using BuildingSystem.RoomStates;
using DefaultNamespace.TaskSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace BuildingSystem
{
    public class Building : MonoBehaviour
    {
        public PlaceableSO PlaceableSo;
        public Transform ProgressPoint;
        
        protected StateMachine _stateMachine;
        protected ConstructingBuildingState _constructingBuildingState;
        protected IdleBuildingState _idleBuildingState;
        protected WorkingBuildingState _workingBuildingState;
        protected DemolishingBuildingState _demolishingBuildingState;
        
        public List<ITask> ActiveTasks = new List<ITask>();
        [HideInInspector]
        public Collider[] Colliders;
        
        public IState CurrentState => _stateMachine.CurrentState;
        
        public virtual void Start()
        {
            Colliders = GetComponentsInChildren<Collider>();
            // SetCollidersToTrigger(true);
            InitStateMachine();
        }
        
        public void SetCollidersToTrigger(bool isTrigger)
        {
            foreach (var collider in Colliders)
            {
                collider.isTrigger = isTrigger;
            }
        }

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine();
            _constructingBuildingState = new ConstructingBuildingState(this);
            _idleBuildingState = new IdleBuildingState(this);
            _workingBuildingState = new WorkingBuildingState(this);
            _demolishingBuildingState = new DemolishingBuildingState(this);
            _stateMachine.Initialize(_constructingBuildingState);
        }

        public void TransitionToIdle()
        {
            _stateMachine.TransitionTo(_idleBuildingState);
        }
        
        public void TransitionToWorking()
        {
            _stateMachine.TransitionTo(_workingBuildingState);
        }

        public void TransitionToDemolishing()
        {
            _stateMachine.TransitionTo(_demolishingBuildingState);
        }
        
        public bool IsUnderConstruction()
        {
            return _stateMachine.CurrentState == _constructingBuildingState;
        }
        public bool IsDemolishing()
        {
            return _stateMachine.CurrentState == _demolishingBuildingState;
        }

        private void OnDestroy()
        {
            foreach (var task in ActiveTasks.ToArray())
            {
                task.RemoveTask();
            }
            
            AstarPath.active?.Scan();
            Demolished();
        }

        protected void AddTask(ITask task)
        {
            ActiveTasks.Add(task);
        }

        [Button]
        public void LogTasks()
        {
            Debug.Log($"Building {name} has {ActiveTasks.Count} active tasks.\n" +
                      $"{string.Join("\n", ActiveTasks.Select(t => t.TaskType.ToString()))}");
        }
        
        private void ChangeLayer(LayerMask layerMask)
        {
            LayerUtils.SetLayerRecursively(gameObject, layerMask);
        }

        public Action OnConstructed;
        public Action OnDemolished;
        
        public virtual void Constructed()
        {
            OnConstructed?.Invoke();
        }
        
        public virtual void Demolished()
        {
            OnDemolished?.Invoke();
        }
    }
}
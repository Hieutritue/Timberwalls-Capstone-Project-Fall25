using System;
using System.Collections.Generic;
using _Scripts.StateMachine;
using DefaultNamespace.ColonistSystem.States;
using DefaultNamespace.General;
using DefaultNamespace.TaskSystem;
using Pathfinding;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    public class Colonist : MonoBehaviour
    {
        public ColonistSO ColonistSo;
        public Dictionary<StatType, StatRuntime> StatDict = new();
        
        [Header("PathFinding")]
        public AIDestinationSetter AiDestinationSetter;
        
        public ITask CurrentTask => TaskManager.Instance.AssignedTasks.ContainsKey(this) ? TaskManager.Instance.AssignedTasks[this] : null;
        
        public StateMachine StateMachine = new StateMachine();
        public FindingFacilityColonistState FindingFacilityState;
        public RunningToFacilityColonistState RunningToFacilityState;
        public FulfillingNeedColonistState FulfillingNeedState;
        public IdleColonistState IdleState;
        public RunningToWorkColonistState RunningToWorkState;
        public WorkingColonistState WorkingState;
        private void Start()
        {
            InitData();
            InitStateMachine();
            ColonistManager.Instance.Colonists.Add(this);
        }

        private void Update()
        {
            StateMachine.Update();
        }
        
        private void InitData()
        {
            foreach (var stat in ColonistSo.Stats)
                StatDict[stat.StatType] = new StatRuntime(stat);
        }

        private void InitStateMachine()
        {
            FindingFacilityState = new FindingFacilityColonistState(this);
            RunningToFacilityState = new RunningToFacilityColonistState(this);
            FulfillingNeedState = new FulfillingNeedColonistState(this);
            IdleState = new IdleColonistState(this);
            RunningToWorkState = new RunningToWorkColonistState(this);
            WorkingState = new WorkingColonistState(this);
            StateMachine.Initialize(IdleState);
        }

        private void OnDestroy()
        {
            ColonistManager.Instance.Colonists.Remove(this);
        }
        
        public void TransitionToIdle()
        {
            StateMachine.TransitionTo(IdleState);
        }
    }
}
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
        
        public ITask CurrentTask { get; set; }

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
        }

        private void Update()
        {
            // StateMachine.Update();
            if (CurrentTask == null)
            {
                TaskManager.Instance.AssignTaskForColonist(this);
            }
            else
            {
                var distanceToTarget =
                    Vector3.Distance(transform.position, CurrentTask.Transform.position);
                if (distanceToTarget < GameManager.Instance.GeneralNumberSO.ConstructionRange)
                {
                    CurrentTask.UpdateProgress(this);
                }
                else
                {
                    RunToTask();
                }
            }
        }
        
        
        public void RunToTask()
        {
            AiDestinationSetter.enabled = true;
            AiDestinationSetter.target = CurrentTask.Transform;
        }

        public bool CanReachPosition(Transform target)
        {
            var startNode = AstarPath.active.GetNearest(transform.position).node;
            var endNode = AstarPath.active.GetNearest(target.position).node;

            // Check if both nodes belong to the same area
            bool reachable = (startNode.Area == endNode.Area);
            Debug.Log($"Checking reachability from {transform.position} to {target.position}: {reachable}");

            return reachable;
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
            ColonistManager.Instance.RemoveColonist(this);
        }
        
        public void TransitionToIdle()
        {
            StateMachine.TransitionTo(IdleState);
        }
    }
}
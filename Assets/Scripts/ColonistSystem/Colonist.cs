using System;
using System.Collections.Generic;
using _Scripts.StateMachine;
using DefaultNamespace.ColonistSystem.States;
using DefaultNamespace.General;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    public class Colonist : MonoBehaviour
    {
        public ColonistSO ColonistSo;
        public Dictionary<StatType, StatRuntime> StatDict = new();
        
        private StateMachine _stateMachine = new StateMachine();
        private FindingFacilityColonistState _findingFacilityState;
        private RunningToFacilityColonistState _runningToFacilityState;
        private FulfillingNeedColonistState _fulfillingNeedState;
        private IdleColonistState _idleState;
        private RunningToWorkColonistState _runningToWorkState;
        private WorkingColonistState _workingState;
        private void Start()
        {
            InitData();
            InitStateMachine();
        }

        private void InitData()
        {
            foreach (var stat in ColonistSo.Stats)
                StatDict[stat.StatType] = new StatRuntime(stat);
        }

        private void InitStateMachine()
        {
            _findingFacilityState = new FindingFacilityColonistState(this);
            _runningToFacilityState = new RunningToFacilityColonistState(this);
            _fulfillingNeedState = new FulfillingNeedColonistState(this);
            _idleState = new IdleColonistState(this);
            _runningToWorkState = new RunningToWorkColonistState(this);
            _workingState = new WorkingColonistState(this);
            _stateMachine.Initialize(_idleState);
        }
    }
}
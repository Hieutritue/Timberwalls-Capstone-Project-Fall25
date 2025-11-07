using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.ColonistSystem.States;
using DefaultNamespace.ColonistSystem.UI;
using DefaultNamespace.General;
using DefaultNamespace.TaskSystem;
using Pathfinding;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using StateMachine = _Scripts.StateMachine.StateMachine;

public class Colonist : MonoBehaviour
{
    public ColonistSO ColonistSo;
    [SerializeField] private CurrentStateWordSO _currentStateWordSo;
    [ShowInInspector] public Dictionary<StatType, float> StatDict = new();

    [Header("PathFinding")] public AIDestinationSetter AiDestinationSetter;
    public WanderingDestinationSetter WanderingDestinationSetter;
    public FollowerEntity FollowerEntity;

    public ITask CurrentTask { get; set; }

    private StateMachine _stateMachine = new StateMachine();
    private IdleColonistState _idleState;
    private RunningToWorkColonistState _runningToWorkState;
    private WorkingColonistState _workingState;

    private string _currentState;
    public Action<string> OnCurrentStateChanged;
    public Action<StatType, float> OnStatChanged;
    
    public ColonistMouseEventController MouseEventController;

    public string CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            OnCurrentStateChanged?.Invoke(_currentState);
        }
    }

    private void Start()
    {
        InitData();
        InitStateMachine();
        MouseEventController.Setup(this);
    }

    private float _timerToCheckState = 0f;
    private float _timerToDecreaseStats = 0f;
    private bool _autoDecreaseStatsEnabled = true;

    public bool AutoDecreaseStatsEnabled
    {
        get => _autoDecreaseStatsEnabled;
        set => _autoDecreaseStatsEnabled = value;
    }

    private void Update()
    {
        _stateMachine.Update();
        AutoDecreaseStats();

        _timerToCheckState += Time.deltaTime;
        if (_timerToCheckState >= 0.2f)
        {
            StateMachineStateCheck();
            _timerToCheckState = 0f;
        }
    }

    private void StateMachineStateCheck()
    {
        if (CurrentTask == null)
        {
            _stateMachine.TransitionTo(_idleState);
            return;
        }

        float distanceToTarget =
            Vector3.Distance(transform.position, CurrentTask.GetBuildingProgressPoint().position);
        bool isConstructionTask = CurrentTask is BuildingTask or DemolishingTask;

        float allowedRange = isConstructionTask
            ? DataTable.Instance.GeneralNumberSo.ConstructionRange
            : DataTable.Instance.GeneralNumberSo.WorkRange;

        if (distanceToTarget > allowedRange)
        {
            _stateMachine.TransitionTo(_runningToWorkState);
            return;
        }

        _stateMachine.TransitionTo(_workingState);
    }

    public void AutoDecreaseStats()
    {
        if (!_autoDecreaseStatsEnabled) return;
        _timerToDecreaseStats += Time.deltaTime;
        if (_timerToDecreaseStats >= 1f)
        {
            foreach (var stat in ColonistSo.Stats)
            {
                var laborMultiplier = 1f;

                if (CurrentTask != null)
                {
                    var actionEffects = DataTable.Instance.ColonistActionCollectionSo
                        .ColonyActionsWithEffects
                        .GetValueOrDefault(CurrentTask.TaskType);

                    if (actionEffects != null && actionEffects.TryGetValue(stat.StatType, out float effect))
                        laborMultiplier = effect;
                }

                var decreaseRate = FormulaCollection.GetRateOfDecrease(
                    stat.BaseRateOfDecrease,
                    laborMultiplier,
                    1f,
                    1f);

                SetStat(stat.StatType, StatDict[stat.StatType] - decreaseRate);
            }


            _timerToDecreaseStats = 0f;
        }
    }

    public void SetStat(StatType statType, float value)
    {
        if (StatDict.ContainsKey(statType))
        {
            StatDict[statType] = Mathf.Clamp(value, 0, ColonistSo.Stats.Find(s => s.StatType == statType).MaxValue);
            OnStatChanged?.Invoke(statType, StatDict[statType]);
        }
    }


    public void RunToTask()
    {
        AiDestinationSetter.enabled = true;
        AiDestinationSetter.target = CurrentTask.GetBuildingProgressPoint();
    }

    private void InitData()
    {
        foreach (var stat in ColonistSo.Stats)
            StatDict[stat.StatType] = stat.MaxValue;
    }

    private void InitStateMachine()
    {
        _idleState = new IdleColonistState(this);
        _runningToWorkState = new RunningToWorkColonistState(this);
        _workingState = new WorkingColonistState(this);

        _stateMachine.OnStateChanged += (state) =>
        {
            CurrentState = state switch
            {
                FindingFacilityColonistState => "FindingFacilityState",
                RunningToFacilityColonistState => "RunningToFacilityState",
                FulfillingNeedColonistState => "FulfillingNeedState",
                IdleColonistState => _currentStateWordSo.IdleWord,
                RunningToWorkColonistState =>
                    $"{_currentStateWordSo.RunningToWord} {CurrentTask.Building.PlaceableSo.Name}",
                WorkingColonistState =>
                    $"{CurrentTask.TaskType.ToString()} {_currentStateWordSo.AtWord} {CurrentTask.Building.PlaceableSo.Name}",
                _ => "UnknownState"
            };
        };

        _stateMachine.Initialize(_idleState);
    }

    private void OnDestroy()
    {
        ColonistManager.Instance.RemoveColonist(this);
    }

    [Button]
    public void LogTaskInfo()
    {
        if (CurrentTask != null)
        {
            Debug.Log($"Current Task: {CurrentTask.TaskType} at {CurrentTask.Building.name}");
        }
        else
        {
            Debug.Log("No current task assigned.");
        }
    }

    [Button]
    public void LogStateInfo()
    {
        Debug.Log($"Current State: {_stateMachine.CurrentState.GetType().Name}");
    }

    public void TransitionToIdleState()
    {
        _stateMachine.TransitionTo(_idleState);
    }
}
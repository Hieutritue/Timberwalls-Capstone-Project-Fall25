using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.ColonistSystem.States;
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
    [ShowInInspector] public Dictionary<StatType, float> StatDict = new();

    [Header("PathFinding")] public AIDestinationSetter AiDestinationSetter;
    public WanderingDestinationSetter WanderingDestinationSetter;
    public FollowerEntity FollowerEntity;

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
        StateMachine.Update();
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
            StateMachine.TransitionTo(IdleState);
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
            StateMachine.TransitionTo(RunningToWorkState);
            return;
        }

        StateMachine.TransitionTo(WorkingState);
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

                Debug.Log(
                    $"colonist: {gameObject.name}" +
                    $", stat: {stat.StatType}" +
                    $", base decrease rate: {stat.BaseRateOfDecrease}" +
                    $", labor multiplier: {laborMultiplier}" +
                    $", final decrease rate: {decreaseRate}");

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
        Debug.Log($"Current State: {StateMachine.CurrentState.GetType().Name}");
    }
}
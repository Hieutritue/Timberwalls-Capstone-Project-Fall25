using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.ScheduleSystem;
using DefaultNamespace.TaskSystem;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ScheduleMenu : MonoSingleton<ScheduleMenu>
{
    public List<ScheduleInfo> ScheduleInfos;
    public ScheduleOfColonist ScheduleOfColonistPrefab;
    public Transform ScheduleOfColonistsContainer;
    [ReadOnly] public ScheduleInfo CurrentScheduleInfo;
    [ReadOnly] public List<ScheduleOfColonist> ScheduleOfColonists;
    [SerializeField] private ScheduleOfColonist _defaultScheduleOfColonist;
    [SerializeField] private List<ScheduleInfo> _defaultScheduleInfosOfColonist;

    void Start()
    {
        gameObject.SetActive(false);
    }

    [Button]
    public void Setup()
    {
        SelectSchedule(ScheduleInfos[0]);

        if (ColonistManager.Instance.GetColonistCount() > 0)
        {
            foreach (var colonist in ColonistManager.Instance.Colonists)
            {
                AddScheduleOfColonist(colonist);
            }
        }

        for (int i = 0; i < _defaultScheduleInfosOfColonist.Count; i++)
        {
            var defaultBox = _defaultScheduleOfColonist.HourBoxes[i];
            defaultBox.ScheduleInfo = _defaultScheduleInfosOfColonist[i];
        }
    }

    public void SelectSchedule(ScheduleInfo scheduleInfo)
    {
        if (CurrentScheduleInfo)
        {
            CurrentScheduleInfo.Button.interactable = true;
        }

        CurrentScheduleInfo = scheduleInfo;
        CurrentScheduleInfo.Button.interactable = false;
    }

    public void SetScheduleBox(ScheduleHourBox scheduleHourBox)
    {
        scheduleHourBox.ScheduleInfo = CurrentScheduleInfo;
    }

    public void AddScheduleOfColonist(Colonist colonist)
    {
        var scheduleOfColonist = Instantiate(ScheduleOfColonistPrefab, ScheduleOfColonistsContainer);
        scheduleOfColonist.Setup(colonist, _defaultScheduleOfColonist);
        ScheduleOfColonists.Add(scheduleOfColonist);
    }

    public void RemoveScheduleOfColonist(Colonist colonist)
    {
        var scheduleOfColonist = ScheduleOfColonists.Find(s => s.Colonist == colonist);
        if (scheduleOfColonist != null)
        {
            ScheduleOfColonists.Remove(scheduleOfColonist);
            Destroy(scheduleOfColonist.gameObject);
        }
    }
}

public enum ScheduleType
{
    Sleep,
    Eat,
    Work,
    Dump,
    Wash,
    Entertain
}

public static class ScheduleTypeExtension
{
    public static List<TaskType> GetAssociatedTaskTypes(this ScheduleType scheduleType)
    {
        return scheduleType switch
        {
            ScheduleType.Sleep => new List<TaskType> { TaskType.Sleeping },
            ScheduleType.Eat => new List<TaskType> { TaskType.Eating },
            ScheduleType.Dump => new List<TaskType> { TaskType.Pooping },
            ScheduleType.Wash => new List<TaskType> { TaskType.Washing },
            ScheduleType.Entertain => new List<TaskType> { TaskType.Playing },
            ScheduleType.Work => new List<TaskType>
            {
                TaskType.Mining,
                TaskType.Smithing,
                TaskType.Farming,
                TaskType.Building,
                TaskType.Fixing,
                TaskType.Demolishing,
                TaskType.Cleaning,
                TaskType.Cooking,
                TaskType.Research,
                TaskType.ManufacturingMeds,
                TaskType.ManningTurrets,
            },
            _ => new List<TaskType>()
        };
    }
}
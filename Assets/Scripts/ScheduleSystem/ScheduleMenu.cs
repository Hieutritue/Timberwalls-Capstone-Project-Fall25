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
    [ReadOnly]
    public ScheduleInfo CurrentScheduleInfo;
    [ReadOnly]
    public List<ScheduleOfColonist> ScheduleOfColonists;
    void Start()
    {
        gameObject.SetActive(false);
        SelectSchedule(ScheduleInfos[0]);
    }
    
    [Button]
    public void Setup()
    {
        if (ColonistManager.Instance.GetColonistCount() > 0)
        {
            foreach (var colonist in ColonistManager.Instance.Colonists)
            {
                AddScheduleOfColonist(colonist);
            }
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
        scheduleOfColonist.Setup(colonist);
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

using System;
using System.Collections.Generic;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    public class ColonistManager : MonoSingleton<ColonistManager>
    {
        [SerializeField] private int _maxColonistCount = 20;
        [field: SerializeField] public List<Colonist> Colonists { get; set; }

        public Action<Colonist> OnColonistAdded;
        public Action<Colonist> OnColonistRemoved;


        private void Start()
        {
            OnColonistAdded += TaskPriorityMatrix.Instance.AddRow;
            OnColonistRemoved += TaskPriorityMatrix.Instance.RemoveRow;

            OnColonistAdded += ScheduleMenu.Instance.AddScheduleOfColonist;
            OnColonistRemoved += ScheduleMenu.Instance.RemoveScheduleOfColonist;
            
            OnColonistAdded += colonist =>
            {
                UIManager.Instance.UpdatePopulationText(Colonists.Count, _maxColonistCount);
            };
            
            TaskPriorityMatrix.Instance.Setup();
            ScheduleMenu.Instance.Setup();
            UIManager.Instance.UpdatePopulationText(Colonists.Count, _maxColonistCount);
        }


        public void AddColonist(Colonist colonist)
        {
            if (Colonists.Count >= _maxColonistCount)
            {
                Debug.LogWarning("Cannot add more colonists. Maximum capacity reached.");
                return;
            }
            if (!Colonists.Contains(colonist))
            {
                Colonists.Add(colonist);
                OnColonistAdded?.Invoke(colonist);
            }
        }

        public void RemoveColonist(Colonist colonist)
        {
            if (Colonists.Contains(colonist))
            {
                Colonists.Remove(colonist);
                OnColonistRemoved?.Invoke(colonist);
            }
        }

        public int GetColonistCount()
        {
            return Colonists.Count;
        }
    }
}
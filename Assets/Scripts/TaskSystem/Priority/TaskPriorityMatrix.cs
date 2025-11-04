using System;
using System.Collections.Generic;
using DefaultNamespace.ColonistSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class TaskPriorityMatrix : MonoSingleton<TaskPriorityMatrix>
    {
        public Transform Container;
        public PriorityRow PriorityRowPrefab;
        List<PriorityRow> _priorityRows = new();

        private void Start()
        {
            gameObject.SetActive(false);
        }

        [Button]
        public void Setup()
        {
            foreach (var colonist in ColonistManager.Instance.Colonists)
            {
                AddRow(colonist);
            }
        }
        
        public PriorityRow GetRow(Colonist colonist)
        {
            return _priorityRows.Find(row => row.Colonist == colonist);
        }
        
        public void AddRow(Colonist colonist)
        {
            var priorityRow = Instantiate(PriorityRowPrefab, Container);
            priorityRow.Setup(colonist);
            _priorityRows.Add(priorityRow);
        }
        
        [Button]
        public void LogRows()
        {
            foreach (var row in _priorityRows)
            {
                Debug.Log($"Colonist: {row.Colonist.ColonistSo.NPCName}");
                foreach (var entry in row.PriorityBoxes)
                {
                    Debug.Log($"  Task Type: {entry.TaskType}, Priority: {entry.PriorityLevel}");
                }
            }
        }
        
        public void RemoveRow(Colonist colonist)
        {
            var rowToRemove = _priorityRows.Find(row => row.Colonist == colonist);
            if (rowToRemove != null)
            {
                _priorityRows.Remove(rowToRemove);
                Destroy(rowToRemove.gameObject);
            }
        }
    }
}
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
            ColonistManager.Instance.OnColonistAdded += AddRow;
            ColonistManager.Instance.OnColonistRemoved += RemoveRow;
        }

        [Button]
        public void Setup()
        {
            if (ColonistManager.Instance.GetColonistCount() > 0)
            {
                foreach (var colonist in ColonistManager.Instance.Colonists)
                {
                    AddRow(colonist);
                }
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
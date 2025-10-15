using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.ColonistSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class TaskManager : MonoSingleton<TaskManager>
    {
        public List<ITask> Tasks = new();
        public Dictionary<Colonist, ITask> AssignedTasks = new();

        public void AssignTask(Colonist colonist, ITask task)
        {
            if (AssignedTasks.ContainsKey(colonist))
            {
                AssignedTasks[colonist] = task;
            }
            else
            {
                AssignedTasks.Add(colonist, task);
            }
        }

        public void UnassignTask(Colonist colonist)
        {
            if (AssignedTasks.ContainsKey(colonist))
            {
                AssignedTasks.Remove(colonist);
            }
        }
        
        public void AddTask(ITask task)
        {
            if (!Tasks.Contains(task))
            {
                Tasks.Add(task);
                CheckTaskAssignments();
            }
        }

        [Button]
        public void CheckTaskAssignments()
        {
            var colonists = ColonistManager.Instance.Colonists;
            Tasks.ForEach(task =>
            {
                // first colonist without a task
                var availableColonist = colonists.FirstOrDefault(c => !AssignedTasks.ContainsKey(c));
                if (availableColonist != null && !AssignedTasks.ContainsValue(task))
                {
                    AssignTask(availableColonist, task);
                }
            });
        }

        [Button]
        public void LogTasks()
        {
            Debug.Log($"Total Tasks: {Tasks.Count}");
            foreach (var task in Tasks)
            {
                Debug.Log($"Task: {task}, Assigned to: {AssignedTasks.FirstOrDefault(x => x.Value == task).Key}");
            }
        }
        
        public void RemoveTask(ITask task)
        {
            if (Tasks.Contains(task))
            {
                Tasks.Remove(task);
            }

            var assignedColonist = AssignedTasks.FirstOrDefault(x => x.Value == task).Key;
            if (assignedColonist != null)
            {
                AssignedTasks.Remove(assignedColonist);
            }
            
            CheckTaskAssignments();
        }
    }
}
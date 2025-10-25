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
            colonists.ForEach(AssignTaskForColonist);
        }

        public ITask GetBestTaskForColonist(Colonist colonist)
        {
            var availableTask = Tasks.Where(task => !task.AssignedColonist || task.AssignedColonist == colonist)
                .ToList();
            if (availableTask.Count <= 0)
                return null;
            var priorityMatrix = TaskPriorityMatrix.Instance;
            var priorityRow = priorityMatrix.GetRow(colonist);
            // sort task based on priority in priorityRow, higher first then distance to colonist, closest first
            var taskList = availableTask.OrderByDescending(task => priorityRow.GetPriorityForTaskType(task.TaskType))
                .ThenBy(task => Vector3.Distance(colonist.transform.position, task.Transform.position)).ToList();
            Debug.Log($"Checking task for colonist: {colonist.ColonistSo.NPCName}\n" +
                      $"Available Tasks: {string.Join(", ", availableTask.Select(t => t.TaskType.ToString()))}\n" +
                      $"Sorted Tasks: {string.Join(", ", taskList.Select(t => t.TaskType.ToString()))}");

            return taskList[0];
        }

        public void AssignTaskForColonist(Colonist colonist)
        {
            var task = GetBestTaskForColonist(colonist);
            if (task != null)
            {
                if (colonist.CurrentTask != null) colonist.CurrentTask.AssignedColonist = null;
                task.AssignedColonist = colonist;
                colonist.CurrentTask = task;
            }
        }

        [Button]
        public void LogTasks()
        {
            Debug.Log($"Total Tasks: {Tasks.Count}");
        }

        public void RemoveTask(ITask task)
        {
            if (Tasks.Contains(task))
            {
                Tasks.Remove(task);
                task.AssignedColonist.CurrentTask = null;
                task.AssignedColonist = null;
            }

            CheckTaskAssignments();
        }
    }
}
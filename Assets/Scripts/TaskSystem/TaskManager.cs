using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
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
            float reachRadius =
                GameManager.Instance.GeneralNumberSO.ConstructionRange; // distance within which colonist can work

            var availableTasks = Tasks
                .Where(task =>
                {
                    // Task must be unassigned or assigned to this colonist
                    bool canTakeTask = !task.AssignedColonist || task.AssignedColonist == colonist;

                    return /*PathfindingUtility.CanGetCloseEnough(colonist.transform.position, task.Transform.position,
                        GameManager.Instance.GeneralNumberSO.ConstructionRange) && */canTakeTask;
                })
                .ToList();

            if (availableTasks.Count == 0)
                return null;

            var priorityMatrix = TaskPriorityMatrix.Instance;
            var priorityRow = priorityMatrix.GetRow(colonist);

            var taskList = availableTasks
                .OrderByDescending(task => priorityRow.GetPriorityForTaskType(task.TaskType))
                .ThenBy(task => Vector3.Distance(colonist.transform.position, task.Transform.position))
                .ToList();

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
                if (task.AssignedColonist) task.AssignedColonist.CurrentTask = null;
                task.AssignedColonist = null;
            }

            CheckTaskAssignments();
        }
    }
}
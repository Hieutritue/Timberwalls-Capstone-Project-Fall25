using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using DefaultNamespace.ScheduleSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class TaskManager : MonoSingleton<TaskManager>
    {
        private List<ITask> _tasks = new();

        public void AddTask(ITask task)
        {
            if (!_tasks.Contains(task))
            {
                _tasks.Add(task);
                // CheckTaskAssignments();
            }
        }

        private float _timer = 0f;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.2f)
            {
                CheckTaskAssignments();
                _timer = 0f;
            }
        }

        [Button]
        public void CheckTaskAssignments()
        {
            // Tasks.ForEach(AssignColonistToTask);
            ColonistManager.Instance.Colonists.ForEach(AssignTaskForColonist);
        }

        private ITask GetBestTaskForColonist(Colonist colonist)
        {
            if (!colonist)
                return null;

            var currentSchedule = ScheduleMenu.Instance.ScheduleOfColonists
                .FirstOrDefault(sc => sc.Colonist == colonist)?
                .HourBoxes[GameTimeManager.Instance.CurrentHour]
                .ScheduleType;

            if (currentSchedule == null) return null;

            var availableTasks = _tasks
                .Where(task =>
                {
                    // If colonist can't work, they can only do personal tasks
                    if (!colonist.CanWork && task is not APersonalActionTask)
                        return false;
                    // Task must be of the type allowed by the colonist's current schedule
                    if (!currentSchedule.Value.GetAssociatedTaskTypes().Contains(task.TaskType))
                        return false;
                    // If task is a resource gathering task, check if resource has reached max capacity or not enough resource to produce
                    if (task is ResourceGatheringTask resourceTask 
                        && (resourceTask.ResourceReachedMaxCapacity()
                            || resourceTask.NotEnoughResourceRequiredToProduce()))
                        return false;

                    // Task must be unassigned or assigned to this colonist
                    bool canTakeTask = !task.AssignedColonist || task.AssignedColonist == colonist;

                    return canTakeTask;
                })
                .ToList();

            if (availableTasks.Count == 0)
                return null;

            var priorityMatrix = TaskPriorityMatrix.Instance;
            var priorityRow = priorityMatrix.GetRow(colonist);

            var taskList = availableTasks
                .OrderByDescending(task => priorityRow.GetPriorityForTaskType(task.TaskType))
                .ThenBy(task => Vector3.Distance(colonist.transform.position, task.GetBuildingProgressPoint().position))
                .ToList();

            return taskList[0];
        }


        private void AssignTaskForColonist(Colonist colonist)
        {
            var task = GetBestTaskForColonist(colonist);

            if (task != colonist.CurrentTask)
            {
                colonist.TransitionToIdleState();
            }

            if (task != null)
            {
                if (colonist.CurrentTask != null) colonist.CurrentTask.AssignedColonist = null;
                task.AssignedColonist = colonist;
                colonist.CurrentTask = task;
            }
            else
            {
                colonist.CurrentTask = task;
            }
        }
        
        [Button]
        public void LogTasks()
        {
            Debug.Log($"Total Tasks: {_tasks.Count}");
        }

        public void RemoveTask(ITask task)
        {
            if (_tasks.Contains(task))
            {
                _tasks.Remove(task);
                if (task.AssignedColonist) task.AssignedColonist.CurrentTask = null;
                task.AssignedColonist = null;
            }

            // CheckTaskAssignments();
        }
    }
}
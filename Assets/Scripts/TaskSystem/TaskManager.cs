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
        public List<ITask> Tasks = new();

        public void AddTask(ITask task)
        {
            if (!Tasks.Contains(task))
            {
                Tasks.Add(task);
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

        public ITask GetBestTaskForColonist(Colonist colonist)
        {
            if (!colonist)
                return null;

            var currentSchedule = ScheduleMenu.Instance.ScheduleOfColonists
                .FirstOrDefault(sc => sc.Colonist == colonist)?
                .HourBoxes[GameTimeManager.Instance.CurrentHour]
                .ScheduleType;

            if (currentSchedule == null) return null;

            var availableTasks = Tasks
                .Where(task =>
                {
                    // Task must be of the type allowed by the colonist's current schedule
                    if (!currentSchedule.Value.GetAssociatedTaskTypes().Contains(task.TaskType))
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

        public Colonist GetBestColonistForTask(ITask task)
        {
            var availableColonists = ColonistManager.Instance.Colonists
                .Where(colonist =>
                {
                    // Colonist must be unassigned or assigned to this task
                    bool canTakeTask = colonist.CurrentTask == null || colonist.CurrentTask == task;

                    return /*PathfindingUtility.CanGetCloseEnough(colonist.transform.position, task.Transform.position,
                        GameManager.Instance.GeneralNumberSO.ConstructionRange) && */canTakeTask;
                })
                .ToList();

            if (availableColonists.Count == 0)
                return null;

            var priorityMatrix = TaskPriorityMatrix.Instance;

            var colonistList = availableColonists
                .OrderByDescending(colonist => priorityMatrix.GetRow(colonist).GetPriorityForTaskType(task.TaskType))
                .ThenBy(colonist =>
                    Vector3.Distance(colonist.transform.position, task.GetBuildingProgressPoint().position))
                .ToList();

            return colonistList[0];
        }

        public void AssignTaskForColonist(Colonist colonist)
        {
            var task = GetBestTaskForColonist(colonist);

            if (task != colonist.CurrentTask)
            {
                colonist.StateMachine.TransitionTo(colonist.IdleState);
            }

            if (task != null)
            {
                if (colonist.CurrentTask != null) colonist.CurrentTask.AssignedColonist = null;
                task.AssignedColonist = colonist;
                colonist.CurrentTask = task;
            }
        }

        public void AssignColonistToTask(ITask task)
        {
            var colonist = GetBestColonistForTask(task);
            if (colonist != null)
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

            // CheckTaskAssignments();
        }
    }
}
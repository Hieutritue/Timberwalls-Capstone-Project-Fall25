using System;
using BuildingSystem;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public abstract class ATask : ITask
    {
        protected Building _building;
        public Colonist AssignedColonist { get; set; }
        public Building Building => _building;
        public TaskType TaskType { get;}
        public Transform Transform => _building?.ProgressPoint;

        public ATask(Building building, TaskType taskType)
        {
            _building = building;
            TaskType = taskType;
            Create();
        }
        public void Create()
        {
            TaskManager.Instance.AddTask(this);
        }

        public virtual void Complete()
        {
            OnComplete?.Invoke();
            RemoveTask();
        }

        public virtual void RemoveTask()
        {
            OnRemove?.Invoke();
            TaskManager.Instance.RemoveTask(this);
            _building?.ActiveTasks.Remove(this);
        }

        public abstract void UpdateProgress(Colonist colonist);
        public Action OnComplete { get; set; }
        public Action OnRemove { get; set; }
        public abstract void ColonistStartWork(Colonist colonist);
        public abstract void ColonistStopWork(Colonist colonist);
    }
}
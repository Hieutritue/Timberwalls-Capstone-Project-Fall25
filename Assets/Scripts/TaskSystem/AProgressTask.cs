using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.WorldSpaceUISystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.TaskSystem
{
    public abstract class AProgressTask : ITask
    {
        protected Building _building;
        protected float _progress;
        protected ProgressBarUI _progressBarUI;
        public Building Building => _building;
        public TaskType TaskType { get;}
        public Transform Transform => _building?.ProgressPoint;

        public AProgressTask(Building building, TaskType taskType)
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
            RewardComplete();
            if (_progressBarUI)
                GameObject.Destroy(_progressBarUI.gameObject);
            OnComplete?.Invoke();
            TaskManager.Instance.RemoveTask(this);
        }

        public virtual void UpdateProgress(Colonist colonist)
        {
            if (!_progressBarUI)
            {
                var prefab = WorldSpaceUIManager.Instance.ProgressBarUIPrefab;
                var barObj = Object.Instantiate(prefab, WorldSpaceUIManager.Instance.transform);
                _progressBarUI = barObj.GetComponent<ProgressBarUI>();
                _progressBarUI.transform.position = Transform.position;
            }

            _progress += Time.deltaTime / ProgressPerFrame(colonist);
            _progressBarUI.SetProgress(_progress);
            
            if (_progress >= 1)
            {
                Complete();
            }
        }
        
        public abstract float ProgressPerFrame(Colonist colonist);
        public abstract void RewardComplete();

        public Action OnComplete { get; set; }
    }
}
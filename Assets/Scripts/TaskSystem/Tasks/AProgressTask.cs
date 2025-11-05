
using BuildingSystem;
using DefaultNamespace.WorldSpaceUISystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.TaskSystem
{
    public abstract class AProgressTask : ATask
    {
        protected float _progress;
        protected ProgressBarUI _progressBarUI;

        public AProgressTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }

        public override void Complete()
        {
            RewardComplete();
            base.Complete();
        }

        public override void RemoveTask()
        {
            base.RemoveTask();
            
            if (_progressBarUI)
                Object.Destroy(_progressBarUI.gameObject);
        }
        
        public abstract void RewardComplete();

        public override void UpdateProgress(Colonist colonist)
        {
            if (!_progressBarUI)
            {
                var prefab = WorldSpaceUIManager.Instance.ProgressBarUIPrefab;
                var barObj = Object.Instantiate(prefab, WorldSpaceUIManager.Instance.transform);
                _progressBarUI = barObj.GetComponent<ProgressBarUI>();
                _progressBarUI.transform.position = Transform.position;
            }

            _progress += Time.deltaTime / TotalProgress(colonist);
            _progressBarUI.SetProgress(_progress);
            
            if (_progress >= 1)
            {
                Complete();
            }
        }

        public abstract float TotalProgress(Colonist colonist);
    }
}
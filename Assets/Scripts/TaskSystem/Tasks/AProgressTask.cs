
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

            var totalProgress = TotalProgress(colonist);
            _progress += Time.deltaTime / totalProgress;
            _progressBarUI.SetProgress(_progress);
            
            if (_progress >= 1)
            {
                Debug.Log($"{_progress}");
                Complete();
                // RewardComplete();
                _progress = 0;
                _progressBarUI.SetProgress(0);
            }
        }

        public abstract float TotalProgress(Colonist colonist);
    }
}
using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.WorldSpaceUISystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.TaskSystem
{
    [Serializable]
    public class ConstructRoomTask : ITask
    {
        private Room _room;
        private float _progress;
        private ProgressBarUI _progressBarUI;

        public ConstructRoomTask(Room room)
        {
            _room = room;
            Create();
        }

        public Transform Transform => _room?.ProgressPoint;

        public void Create()
        {
            TaskManager.Instance.AddTask(this);
        }

        public void Complete()
        {
            if (_progressBarUI != null)
                     GameObject.Destroy(_progressBarUI.gameObject);
            OnComplete?.Invoke();
            TaskManager.Instance.RemoveTask(this);
        }

        public void UpdateProgress(Colonist colonist)
        {
            if (_progressBarUI == null)
            {
                var prefab = WorldSpaceUIManager.Instance.ProgressBarUIPrefab;
                var barObj = Object.Instantiate(prefab, WorldSpaceUIManager.Instance.transform);
                _progressBarUI = barObj.GetComponent<ProgressBarUI>();
                _progressBarUI.transform.position = Transform.position; // or a construction site transform
            }
            
            _progress += Time.deltaTime / ProgressPerFrame(colonist);
            _progressBarUI.SetProgress(_progress);
            
            if (_progress >= 1)
            {
                _room.SwitchStateToIdle();
                Complete();
            }
        }

        public Action OnComplete { get; set; }

        public float ProgressPerFrame(Colonist colonist)
        {
            var adjustedDuration = _room.BuildingSo.BaseBuildTime *
                                   (1 - colonist.ColonistSo.Skills[SkillType.Engineering] * 0.05f);
            adjustedDuration = Mathf.Max(0.5f, adjustedDuration); // prevent zero or negative times
            return adjustedDuration;
        }
    }
}
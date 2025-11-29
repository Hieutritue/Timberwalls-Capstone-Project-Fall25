using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace TaskSystem.Tasks.SpecificTask.PersonalTask
{
    public class PlayingTask : APersonalActionTask
    {
        private Transform _actionPoint;
        private Building _building;

        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Playing);
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            base.ColonistStartWork(colonist);
            colonist.animator.SetTrigger(ColonistAnimationString.PLAYING);
            var tag = _building.tag;
            var animString = FurnitureTag.GetAnimStringBaseOnFurniture(tag);
            if (!string.IsNullOrEmpty(animString))
                colonist.animator.SetTrigger(animString);
            else
            {
                Debug.LogWarning("No Anim String Found For" + tag);
            }
            _building.TransitionToWorking();
        }

        public PlayingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint,
            taskType)
        {
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_PLAYING);
            _building.TransitionToIdle();
            
        }
    }
}
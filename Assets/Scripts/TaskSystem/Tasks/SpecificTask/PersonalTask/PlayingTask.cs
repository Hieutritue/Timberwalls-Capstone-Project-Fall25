using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace TaskSystem.Tasks.SpecificTask.PersonalTask
{
    public class PlayingTask : APersonalActionTask
    {
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Playing);
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            base.ColonistStartWork(colonist);
            colonist.animator.ResetTrigger(ColonistAnimationString.PLAYING);
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_PLAYING);
            colonist.animator.SetTrigger(ColonistAnimationString.PLAYING);
            var tag = _building.tag;
            var animString = FurnitureTag.GetAnimStringBaseOnFurniture(tag);

            string loopSound = GlobalSoundNameHolder.GetLoopSoundForAnimation(animString);
            if (!string.IsNullOrEmpty(animString))
            {
                colonist.animator.ResetTrigger(animString);
                colonist.animator.SetTrigger(animString);
                colonist.vfx_source.Play(loopSound, fadeIn: false, fadeOut: false, crossfade: true);

            }

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
            colonist.vfx_source.StopImmediate();
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_PLAYING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_PLAYING);
            _building.TransitionToIdle();

        }
    }
}
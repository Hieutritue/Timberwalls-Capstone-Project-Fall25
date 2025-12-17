using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace TaskSystem.Tasks.SpecificTask.PersonalTask
{
    public class WashingTask : APersonalActionTask
    {
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Washing);
        }

        public WashingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            base.ColonistStartWork(colonist);
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.animator.ResetTrigger(ColonistAnimationString.SELF_CARING);
            colonist.animator.SetTrigger(ColonistAnimationString.SELF_CARING);
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
        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.animator.ResetTrigger(ColonistAnimationString.SELF_CARING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.vfx_source.StopImmediate();
            _building.TransitionToIdle();
        }

    }
}
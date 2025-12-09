using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class DemolishingTask : AProgressTask
    {
        public DemolishingTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }

        public override float TotalProgress(Colonist colonist)
        {
            return FormulaCollection.ProgressPerFrameBasedOnSkillLevel(_building.PlaceableSo.BaseBuildTime,
                colonist.ColonistSo.Skills[SkillType.Engineering],
                colonist.TaskCompletionSpeedMultiplier) / 2;
        }

        public override void RewardComplete()
        {
            RemoveTask();
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            var lookDir = Building.ProgressPoint.position - colonist.transform.position;
            lookDir.y = 0;
            colonist.transform.rotation = Quaternion.LookRotation(lookDir);
            
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.BREAKING_WORK);

            string loopSound = GlobalSoundNameHolder.GetLoopSoundForAnimation(ColonistAnimationString.BREAKING_WORK);

            if (!string.IsNullOrEmpty(loopSound))
                colonist.vfx_source.Play(loopSound, fadeIn: false, fadeOut: false, crossfade: true);
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.vfx_source.FadeOutAndStop();
        }
    }
}
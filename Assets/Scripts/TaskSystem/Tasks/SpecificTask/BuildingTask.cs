using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    [Serializable]
    public class BuildingTask : AProgressTask
    {
        public override void RewardComplete()
        {
            RemoveTask();
            _building.TransitionToIdle();
        }

        public override float TotalProgress(Colonist colonist)
        {
            return FormulaCollection.ProgressPerFrameBasedOnSkillLevel(_building.PlaceableSo.BaseBuildTime,
                colonist.ColonistSo.Skills[SkillType.Engineering],
                colonist.TaskCompletionSpeedMultiplier);
        }

        public BuildingTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            // TODO: Animation
            var lookDir = Building.ProgressPoint.position - colonist.transform.position;
            lookDir.y = 0;
            colonist.transform.rotation = Quaternion.LookRotation(lookDir);

            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.BUILDING_WORK);

            string loopSound = GlobalSoundNameHolder.GetLoopSoundForAnimation(ColonistAnimationString.BUILDING_WORK);

            if (!string.IsNullOrEmpty(loopSound))
                colonist.vfx_source.Play(loopSound, fadeIn: false, fadeOut: false, crossfade: true);
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            // TODO: Animation
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.vfx_source.FadeOutAndStop();
        }
    }
}
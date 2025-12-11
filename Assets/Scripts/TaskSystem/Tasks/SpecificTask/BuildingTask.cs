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
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.BUILDING_WORK);
            var lookDir = Building.ProgressPoint.position - colonist.transform.position;
            lookDir.y = 0;
            colonist.transform.rotation = Quaternion.LookRotation(lookDir);
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.BUILDING_WORK);
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            // TODO: Animation
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            //colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
        }
    }
}
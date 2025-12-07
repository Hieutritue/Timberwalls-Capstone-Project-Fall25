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
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.BREAKING_WORK);
            var lookDir = Building.ProgressPoint.position - colonist.transform.position;
            lookDir.y = 0;
            colonist.transform.rotation = Quaternion.LookRotation(lookDir);
            
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.BREAKING_WORK);
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
        }
    }
}
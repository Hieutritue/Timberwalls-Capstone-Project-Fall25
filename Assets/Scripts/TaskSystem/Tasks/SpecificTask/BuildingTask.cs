using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;

namespace DefaultNamespace.TaskSystem
{
    [Serializable]
    public class BuildingTask : AProgressTask
    {
        public override void RewardComplete()
        {
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
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.BUILDING_WORK);
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            // TODO: Animation
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
        }
    }
}
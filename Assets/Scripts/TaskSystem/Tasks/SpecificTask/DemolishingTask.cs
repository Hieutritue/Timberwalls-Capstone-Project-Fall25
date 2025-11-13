using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;

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
        }
        
        public override void ColonistStartWork(Colonist colonist)
        {
            // TODO: Animation
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            // TODO: Animation
        }
    }
}
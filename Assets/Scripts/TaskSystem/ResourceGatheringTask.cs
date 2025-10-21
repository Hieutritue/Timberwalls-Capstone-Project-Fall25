using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;

namespace DefaultNamespace.TaskSystem
{
    public class ResourceGatheringTask : AProgressTask
    {
        public ResourceGatheringTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }

        public override float ProgressPerFrame(Colonist colonist)
        {
            if (_building is ResourceGatheringFurniture gatheringFurniture)
            {
                return FormulaCollection.ProgressPerFrameBasedOnSkillLevel(
                    gatheringFurniture.GatheringFurnitureSo.BaseTimeToProduce,
                    colonist.ColonistSo.Skills[SkillType.Metallurgy]);
            }

            return 0;
        }

        public override void RewardComplete()
        {
            if (_building is ResourceGatheringFurniture gatheringFurniture)
            {
                gatheringFurniture.Work();
            }
        }
    }
}
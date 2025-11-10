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

        public override float TotalProgress(Colonist colonist)
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
                gatheringFurniture.CreateTask();
            }
        }

        public override void UpdateProgress(Colonist colonist)
        {
            SetColonistPosition(colonist);
            base.UpdateProgress(colonist);
        }
        
        
        private void SetColonistPosition(Colonist colonist)
        {
            if (Building is ResourceGatheringFurniture furniture)
            {
                colonist.transform.position = furniture.ActionPoint.position;
                colonist.transform.LookAt(furniture.ActionPoint.position + furniture.ActionPoint.forward);
            }
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
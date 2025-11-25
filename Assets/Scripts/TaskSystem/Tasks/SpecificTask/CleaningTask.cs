using System;
using BuildingSystem;
using BuildingSystem.CleanObjects;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using ResourceSystem;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class CleaningTask : AProgressTask
    {
        public ICleanable CleanableObject { get; }

        public override Transform Transform => CleanableObject.CleanPoint;
        public override string LocationName => "Poop";

        public CleaningTask(Building building, ICleanable cleanable, TaskType taskType) : base(building, taskType)
        {
            CleanableObject = cleanable;
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            // throw new NotImplementedException();
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            // throw new NotImplementedException();
        }

        public override void RewardComplete()
        {
            ResourceManager.Instance.Set(ResourceType.Biomass, ResourceManager.Instance.Get(ResourceType.Biomass) + 10);
            if (CleanableObject is MonoBehaviour mb)
            {
                UnityEngine.Object.Destroy(mb.gameObject);
            }
        }

        public override float TotalProgress(Colonist colonist)
        {
            return FormulaCollection.ProgressPerFrameBasedOnSkillLevel(CleanableObject.TimeToClean,
                colonist.ColonistSo.Skills[SkillType.Housekeeping],
                colonist.TaskCompletionSpeedMultiplier);
        }
    }
}
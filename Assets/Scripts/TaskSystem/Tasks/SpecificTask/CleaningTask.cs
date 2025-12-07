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
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.BUILDING_WORK);
            var lookDir = Transform.position - colonist.transform.position;
            lookDir.y = 0;
            colonist.transform.rotation = Quaternion.LookRotation(lookDir);
            // throw new NotImplementedException();
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.FURNITURE_WORK);
            colonist.animator.SetTrigger(ColonistAnimationString.CLEANING);
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
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
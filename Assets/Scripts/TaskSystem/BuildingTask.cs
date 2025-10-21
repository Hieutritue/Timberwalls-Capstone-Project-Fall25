using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using DefaultNamespace.WorldSpaceUISystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.TaskSystem
{
    [Serializable]
    public class BuildingTask : AProgressTask
    {
        public override void RewardComplete()
        {
            _building.TransitionToIdle();
        }

        public override float ProgressPerFrame(Colonist colonist)
        {
            return FormulaCollection.ProgressPerFrameBasedOnSkillLevel(_building.PlaceableSo.BaseBuildTime,
                colonist.ColonistSo.Skills[SkillType.Engineering]);
        }

        public BuildingTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }
    }
}
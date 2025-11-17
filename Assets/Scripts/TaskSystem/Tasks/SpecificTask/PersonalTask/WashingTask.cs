using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace TaskSystem.Tasks.SpecificTask.PersonalTask
{
    public class WashingTask : APersonalActionTask
    {

        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Washing);
        }

        public WashingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }
    }
}
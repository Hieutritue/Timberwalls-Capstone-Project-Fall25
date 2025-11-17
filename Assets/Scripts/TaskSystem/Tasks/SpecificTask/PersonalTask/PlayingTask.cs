using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace TaskSystem.Tasks.SpecificTask.PersonalTask
{
    public class PlayingTask : APersonalActionTask
    {

        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Playing);
        }

        public PlayingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }
    }
}
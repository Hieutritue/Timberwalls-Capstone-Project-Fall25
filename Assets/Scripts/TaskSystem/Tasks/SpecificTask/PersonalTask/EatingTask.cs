using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.General;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class EatingTask : APersonalActionTask
    {
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Eating);
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            base.ColonistStartWork(colonist);
        }

        public EatingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }
    }
}
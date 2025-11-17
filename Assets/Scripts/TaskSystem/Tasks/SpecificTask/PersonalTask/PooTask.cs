using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.General;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class PooTask : APersonalActionTask
    {
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Pooping);
        }

        public PooTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }
    }
}
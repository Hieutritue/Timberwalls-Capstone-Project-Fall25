using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public abstract class AInfiniteTask : ATask
    {
        public AInfiniteTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }
    }
}
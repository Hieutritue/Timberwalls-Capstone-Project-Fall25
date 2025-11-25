using System;
using BuildingSystem;

namespace DefaultNamespace.TaskSystem
{
    public interface ITask
    {
        Colonist AssignedColonist { get; set; }
        Building Building { get; }
        TaskType TaskType { get; }
        void RemoveTask();
        void UpdateProgress(Colonist colonist);
        Action OnComplete { get; set; }
        Action OnRemove { get; set; }
        void ColonistStartWork(Colonist colonist);
        void ColonistStopWork(Colonist colonist);
    }

    public enum TaskType
    {
        Mining,
        Smithing,
        Farming,
        Building,
        Fixing,
        Demolishing,
        Cleaning,
        Cooking,
        Research,
        ManufacturingMeds,
        ManningTurrets,

        Sleeping,
        Eating,
        Pooping,
        Playing,
        Washing,
    }
}
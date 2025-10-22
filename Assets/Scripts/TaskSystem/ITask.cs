using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public interface ITask
    {
        Building Building { get; }
        TaskType TaskType { get;}
        Transform Transform { get; }
        void Create();
        void Complete();
        void RemoveTask();
        void UpdateProgress(Colonist colonist);
        Action OnComplete { get; set; }
        Action OnRemove { get; set; }
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
    }
}
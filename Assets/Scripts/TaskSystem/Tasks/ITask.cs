using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;

namespace DefaultNamespace.TaskSystem
{
    public interface ITask
    {
        string LocationName { get; }
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
        Refining,
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
    
    public static class TaskTypeExtensions
    {
        public static SkillType SkillForTask(this TaskType taskType)
        {
            return taskType switch
            {
                TaskType.Mining => SkillType.Metallurgy,
                TaskType.Refining => SkillType.Metallurgy,
                TaskType.Farming => SkillType.Farming,
                TaskType.Building => SkillType.Engineering,
                TaskType.Fixing => SkillType.Engineering,
                TaskType.Demolishing => SkillType.Engineering,
                TaskType.Cleaning => SkillType.Housekeeping,
                TaskType.Cooking => SkillType.Housekeeping,
                TaskType.Research => SkillType.Scholarship,
                TaskType.ManufacturingMeds => SkillType.Scholarship,
                TaskType.ManningTurrets => SkillType.Marksmanship,
            };
        }
    } 
}
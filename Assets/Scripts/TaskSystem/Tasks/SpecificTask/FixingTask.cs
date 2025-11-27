using System;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using ShieldSystem;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class FixingTask : ITask
    {
        public string LocationName => "Shield Generator";
        public Colonist AssignedColonist { get; set; }
        public Building Building { get; }
        public TaskType TaskType { get; }

        public ShieldGenerator ShieldGenerator { get; }

        public Transform ActionPoint { get; }

        public FixingTask(ShieldGenerator shieldGenerator, Transform actionPoint, TaskType taskType)
        {
            ShieldGenerator = shieldGenerator;
            TaskType = taskType;
            ActionPoint = actionPoint;
            Create();
        }

        public void RemoveTask()
        {
            TaskManager.Instance.RemoveTask(this);
        }

        public void UpdateProgress(Colonist colonist)
        {
        }

        public Action OnComplete { get; set; }
        public Action OnRemove { get; set; }

        public void ColonistStartWork(Colonist colonist)
        {
            ShieldGenerator.FixerSkillCount += colonist.ColonistSo.Skills[SkillType.Engineering];
            colonist.transform.position = ActionPoint.position;
            colonist.transform.rotation = ActionPoint.rotation;
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.BUILDING_WORK);
        }

        public void ColonistStopWork(Colonist colonist)
        {
            ShieldGenerator.FixerSkillCount -= colonist.ColonistSo.Skills[SkillType.Engineering];
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
        }

        public void Create()
        {
            TaskManager.Instance.AddTask(this);
        }
    }
}
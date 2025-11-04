using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.General;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class SleepingTask : APersonalActionTask
    {
        public override void UpdateProgress(Colonist colonist)
        {
            Timer += Time.deltaTime;
            if (Timer >= 1)
            {
                DataTable.Instance.ColonistActionCollectionSo.PersonalActionsWithEffects[TaskType.Sleeping].ForEach(
                    effect =>
                    {
                        var furnitureMultiplier =
                            PersonalActionFurniture.PersonalActionFurnitureSo.StatMultipliers.GetValueOrDefault(
                                effect.Key, 1f);
                        colonist.SetStat(effect.Key, colonist.StatDict[effect.Key] +
                                                     FormulaCollection.GetRateOfIncrease(
                                                         effect.Value,
                                                         furnitureMultiplier, 1)); // Clamp stat
                        // TODO: only heal if bed is medical bed
                    });
                Timer = 0f;
            }
        }

        public SleepingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }
    }
}
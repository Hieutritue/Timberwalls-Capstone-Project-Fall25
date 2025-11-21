using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
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
            if (!(Timer >= 1)) return;
            DataTable.Instance.ColonistActionCollectionSo.PersonalActionsWithEffects[TaskType.Sleeping].ForEach(
                effect =>
                {
                    var roomMultiplier = 1f;
                    if (PersonalActionFurniture.ContainingRoom.CurrentSpecificRoomSo)
                    {
                        roomMultiplier =
                            PersonalActionFurniture.ContainingRoom.CurrentSpecificRoomSo.StatMultipliers
                                .GetValueOrDefault(effect.Key, 1f);
                    }

                    var furnitureMultiplier =
                        PersonalActionFurniture.PersonalActionFurnitureSo
                            .StatMultipliers.GetValueOrDefault(effect.Key, 1f);

                    // if health, only increase if furniture is medical
                    if (effect.Key == StatType.Health)
                    {
                        if (PersonalActionFurniture.PlaceableSo.Category == BuildingCategory.Medical)
                            colonist.SetStat(effect.Key, colonist.StatDict[effect.Key] +
                                                         FormulaCollection.GetRateOfIncrease(
                                                             effect.Value,
                                                             furnitureMultiplier, roomMultiplier)); // Clamp stat
                    }
                    else
                    {
                        colonist.SetStat(effect.Key, colonist.StatDict[effect.Key] +
                                                     FormulaCollection.GetRateOfIncrease(
                                                         effect.Value,
                                                         furnitureMultiplier, roomMultiplier)); // Clamp stat
                    }
                });
            Timer = 0f;
        }

        public SleepingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint,
            taskType)
        {
        }
    }
}
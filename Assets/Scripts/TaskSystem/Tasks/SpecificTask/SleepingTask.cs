using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.General;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class SleepingTask : AInfiniteTask
    {
        public BedFurniture BedFurniture => (BedFurniture)Building;

        public SleepingTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }

        private float _timer = 0f;

        public override void UpdateProgress(Colonist colonist)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1)
            {
                DataTable.Instance.ColonistActionCollectionSo.PersonalActionsWithEffects[TaskType.Sleeping].ForEach(
                    effect =>
                    {
                        var furnitureMultiplier =
                            BedFurniture.PersonalActionFurnitureSo.StatMultipliers.GetValueOrDefault(effect.Key, 1f);
                        colonist.SetStat(effect.Key, colonist.StatDict[effect.Key] +
                                                     FormulaCollection.GetRateOfIncrease(
                                                         effect.Value,
                                                         furnitureMultiplier, 1)); // Clamp stat
                        // TODO: only heal if bed is medical bed
                    });
                _timer = 0f;
            }
        }

        public void SetColonistOnBed(Colonist colonist)
        {
            if (Building is Furniture furniture)
            {
                colonist.transform.position = furniture.ActionPoint.position;
                // Colonist looks upwards while sleeping
                colonist.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            }
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            SetColonistOnBed(colonist);
            colonist.AutoDecreaseStatsEnabled = false;
            // TODO: Animation
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            // TODO: Animation
        }
    }
}
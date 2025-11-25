using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using ResourceSystem;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class SleepingTask : APersonalActionTask
    {
        protected override void SetStat(Colonist colonist, KeyValuePair<StatType, float> effect, float furnitureMultiplier, float roomMultiplier)
        {
            // if health, only increase if furniture is medical
            if (effect.Key == StatType.Health)
            {
                if (PersonalActionFurniture.PlaceableSo.Category != BuildingCategory.Medical) return;
                bool isEnoughPills = effect.Value <= ResourceManager.Instance.Get(ResourceType.Pills);
                if (isEnoughPills)
                {
                    ResourceManager.Instance.Set(ResourceType.Pills,
                        ResourceManager.Instance.Get(ResourceType.Pills) - Mathf.CeilToInt(effect.Value));
                    base.SetStat(colonist, effect, furnitureMultiplier, roomMultiplier);
                }
            }
            else
            {
                base.SetStat(colonist, effect, furnitureMultiplier, roomMultiplier);
            }
        }

        public SleepingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint,
            taskType)
        {
        }

        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist,TaskType.Sleeping);
        }
    }
}
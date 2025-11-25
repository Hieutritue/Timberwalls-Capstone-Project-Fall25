using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using ResourceSystem;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class EatingTask : APersonalActionTask
    {
        public override void UpdateProgress(Colonist colonist)
        {
            
            AddStat(colonist, TaskType.Eating);
        }

        protected override void SetStat(Colonist colonist, KeyValuePair<StatType, float> effect, float furnitureMultiplier, float roomMultiplier)
        {
            var isEnoughCookedFood = effect.Value <= ResourceManager.Instance.Get(ResourceType.CookedFood);
            if (!isEnoughCookedFood) return;
            ResourceManager.Instance.Set(ResourceType.CookedFood,
                ResourceManager.Instance.Get(ResourceType.CookedFood) - Mathf.CeilToInt(effect.Value));
            base.SetStat(colonist, effect, furnitureMultiplier, roomMultiplier);
        }

        public EatingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }
    }
}
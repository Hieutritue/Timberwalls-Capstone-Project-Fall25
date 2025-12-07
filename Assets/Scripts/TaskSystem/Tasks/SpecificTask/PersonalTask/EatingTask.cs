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
        
        public override void ColonistStartWork(Colonist colonist)
        {
            base.ColonistStartWork(colonist);
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.animator.ResetTrigger(ColonistAnimationString.SELF_CARING);
            colonist.animator.SetTrigger(ColonistAnimationString.SELF_CARING);
            var tag = _building.tag;
            var animString = FurnitureTag.GetAnimStringBaseOnFurniture(tag);
            if (!string.IsNullOrEmpty(animString))
            {
                colonist.animator.ResetTrigger(animString);
                colonist.animator.SetTrigger(animString);
            }
            else
            {
                Debug.LogWarning("No Anim String Found For" + tag);
            }
            _building.TransitionToWorking();
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
        
        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.animator.ResetTrigger(ColonistAnimationString.SELF_CARING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.AutoDecreaseStatsEnabled = true;
            _building.TransitionToIdle();
            
        }
    }
}
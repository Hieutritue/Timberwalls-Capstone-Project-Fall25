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

        public override void ColonistStartWork(Colonist colonist)
        {
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.animator.ResetTrigger(ColonistAnimationString.SELF_CARING);
            Vector3 sleepingPosition = new Vector3(180, 0, 180);
            base.ColonistStartWork(colonist);
            colonist.transform.eulerAngles = sleepingPosition;
            colonist.animator.SetTrigger(ColonistAnimationString.SELF_CARING);
            var tag = _building.tag;
            var animString = FurnitureTag.GetAnimStringBaseOnFurniture(tag);
            string loopSound = GlobalSoundNameHolder.GetLoopSoundForAnimation(animString);

            if (!string.IsNullOrEmpty(animString))
            {
                colonist.animator.ResetTrigger(animString);
                colonist.animator.SetTrigger(animString);
                colonist.vfx_source.Play(loopSound, fadeIn: false, fadeOut: false, crossfade: true);
            }
            else
            {
                Debug.LogWarning("No Anim String Found For" + tag);
            }
        }
        public SleepingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint,
            taskType)
        {
        }

        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Sleeping);
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.vfx_source.StopImmediate();
        }
    }
}
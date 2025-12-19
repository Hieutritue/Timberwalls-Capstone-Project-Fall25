using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.General;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class PooTask : APersonalActionTask
    {
        private Transform _actionPoint;
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Pooping);
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            base.ColonistStartWork(colonist);
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.animator.ResetTrigger(ColonistAnimationString.SELF_CARING);
            colonist.animator.SetTrigger(ColonistAnimationString.SELF_CARING);
            // _building.Animator.SetTrigger(BuildingAnimationString.IS_ACTIVE);
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
            _building.TransitionToWorking();

        }

        public PooTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.animator.ResetTrigger(ColonistAnimationString.SELF_CARING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
            colonist.vfx_source.StopImmediate();
            if (_building.IsFurnitureActive())
            _building.TransitionToIdle();
        }
    }
}
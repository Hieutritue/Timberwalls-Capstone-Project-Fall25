using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace TaskSystem.Tasks.SpecificTask.PersonalTask
{
    public class WashingTask : APersonalActionTask
    {
        private Transform _actionPoint;
        private Building _building;
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Washing);
        }

        public WashingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
            _actionPoint = actionPoint;
            _building = building;
        }
        
        public override void ColonistStartWork(Colonist colonist)
        {
            colonist.transform.position = _actionPoint.position;
            colonist.transform.rotation = _actionPoint.rotation;
            colonist.AutoDecreaseStatsEnabled = false;
            colonist.animator.SetTrigger(ColonistAnimationString.SELF_CARING);
            // _building.Animator.SetTrigger(BuildingAnimationString.IS_ACTIVE);
            var tag = _building.tag;
            var animString = FurnitureTag.GetAnimStringBaseOnFurniture(tag);
            if(!string.IsNullOrEmpty(animString))
                colonist.animator.SetTrigger(animString);
            else
            {
                Debug.LogWarning("No Anim String Found For" + tag);
            }
            
        }
        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
        }
        
    }
}
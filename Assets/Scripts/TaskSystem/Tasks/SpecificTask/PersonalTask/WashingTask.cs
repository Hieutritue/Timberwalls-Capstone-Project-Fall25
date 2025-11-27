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
            if (_building.tag == "WaterTap")
            {
                colonist.animator.SetTrigger(ColonistAnimationString.WASHING_TAP);
            }
            else
            {
                colonist.animator.SetTrigger(ColonistAnimationString.BATHING);
            }
            
        }
        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
        }
        
    }
}
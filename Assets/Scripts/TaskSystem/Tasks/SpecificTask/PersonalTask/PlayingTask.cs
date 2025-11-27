using BuildingSystem;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace TaskSystem.Tasks.SpecificTask.PersonalTask
{
    public class PlayingTask : APersonalActionTask
    {
        private Transform _actionPoint;
        private Building _building;
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Playing);
        }
        
        public override void ColonistStartWork(Colonist colonist)
        {
            colonist.transform.position = _actionPoint.position;
            colonist.transform.rotation = _actionPoint.rotation;
            colonist.AutoDecreaseStatsEnabled = false;
            colonist.animator.SetTrigger(ColonistAnimationString.PLAYING);
           // _building.Animator.SetTrigger(BuildingAnimationString.IS_ACTIVE);
           if (_building.gameObject.tag == "Speaker")
           {
               colonist.animator.SetTrigger(ColonistAnimationString.DANCING);
           }
           else
           {
               colonist.animator.SetTrigger(ColonistAnimationString.PLAYING_POKER);
           }
        }

        public PlayingTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint,
            taskType)
        {
            _actionPoint = actionPoint;
            _building = building;
        }
        
        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_PLAYING);
        }
    }
}
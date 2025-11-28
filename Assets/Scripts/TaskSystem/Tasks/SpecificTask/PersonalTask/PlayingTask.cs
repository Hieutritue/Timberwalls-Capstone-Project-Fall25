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
           var tag = _building.tag;
           var animString = FurnitureTag.GetAnimStringBaseOnFurniture(tag);
           if(!string.IsNullOrEmpty(animString))
               colonist.animator.SetTrigger(animString);
           else
           {
               Debug.LogError("No Anim String Found For" + tag);
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
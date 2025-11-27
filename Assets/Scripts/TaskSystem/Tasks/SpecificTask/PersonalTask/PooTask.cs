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
        private Building _building;
        public override void UpdateProgress(Colonist colonist)
        {
            AddStat(colonist, TaskType.Pooping);
        }
        
        public override void ColonistStartWork(Colonist colonist)
        {
            colonist.transform.position = _actionPoint.position;
            colonist.transform.rotation = _actionPoint.rotation;
            colonist.AutoDecreaseStatsEnabled = false;
            colonist.animator.SetTrigger(ColonistAnimationString.SELF_CARING);
            // _building.Animator.SetTrigger(BuildingAnimationString.IS_ACTIVE);
            if (_building.tag != "SitToilet")
            {
                colonist.animator.SetTrigger(ColonistAnimationString.SQUAT_POOPING);
            }
            else
            {
                colonist.animator.SetTrigger(ColonistAnimationString.SIT_POOPING);
            }
           
        }

        public PooTask(Building building, Transform actionPoint, TaskType taskType) : base(building, actionPoint, taskType)
        {
            _actionPoint = actionPoint;
            _building = building;
        }
        
        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_SELF_CARING);
        }
    }
}
using System;
using BuildingSystem;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class ManningTurretTask : ATask
    {
        private Transform _actionPoint;
        public ManningTurretTask(Building building, Transform actionPoint, TaskType taskType) : base(building, taskType)
        {
            _actionPoint = actionPoint;
        }

        private TurretFurniture TurretFurniture => (TurretFurniture)Building;
        public override void UpdateProgress(Colonist colonist)
        {
            TurretFurniture.UpdateInWorkingState();
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.FURNITURE_WORK);
            TurretFurniture.ColonistAssignedToTurret = colonist;
            colonist.transform.position = _actionPoint.position;
            colonist.transform.LookAt(_actionPoint.position + _actionPoint.forward);
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.FURNITURE_WORK);
            colonist.animator.SetTrigger(ColonistAnimationString.TYPING);
            
            if (Building.Animator)
                Building.Animator.SetBool(BuildingAnimationString.IS_ACTIVE, true);
            // colonist.transform.position = _actionPoint.position;
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            TurretFurniture.ColonistAssignedToTurret = null;
            TurretFurniture.StopShooting();
            colonist.animator.ResetTrigger(ColonistAnimationString.EXIT_WORKING);
            colonist.animator.ResetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
            
            if (Building.Animator)
                Building.Animator.SetBool(BuildingAnimationString.IS_ACTIVE, false);
        }
    }
}
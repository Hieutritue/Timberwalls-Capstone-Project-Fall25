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
            TurretFurniture.ColonistAssignedToTurret = colonist;
            // colonist.transform.position = _actionPoint.position;
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            TurretFurniture.ColonistAssignedToTurret = null;
        }
    }
}
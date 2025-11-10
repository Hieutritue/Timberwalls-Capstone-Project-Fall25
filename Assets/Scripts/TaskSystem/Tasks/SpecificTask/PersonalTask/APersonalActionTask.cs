using System;
using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using Sirenix.Utilities;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public abstract class APersonalActionTask : ATask
    {
        
        private Transform _actionPoint;
        public PersonalActionFurniture PersonalActionFurniture => (PersonalActionFurniture) Building;
        public APersonalActionTask(Building building, Transform actionPoint, TaskType taskType) : base(building, taskType)
        {
            _actionPoint = actionPoint;
        }
        
        protected float Timer = 0f;

        public void AddStat(Colonist colonist, TaskType taskType)
        
        {
            Timer += Time.deltaTime;
            if (Timer >= 1)
            {
                DataTable.Instance.ColonistActionCollectionSo.PersonalActionsWithEffects[taskType].ForEach(
                    effect =>
                    {
                        var furnitureMultiplier =
                            PersonalActionFurniture.PersonalActionFurnitureSo.StatMultipliers.GetValueOrDefault(effect.Key, 1f);
                        colonist.SetStat(effect.Key, colonist.StatDict[effect.Key] +
                                                     FormulaCollection.GetRateOfIncrease(
                                                         effect.Value,
                                                         furnitureMultiplier, 1)); // Clamp stat
                    });
                Timer = 0f;
            }
        }

        public override void ColonistStartWork(Colonist colonist)
        {
            colonist.transform.position = _actionPoint.position;
            colonist.transform.rotation = _actionPoint.rotation;
            colonist.AutoDecreaseStatsEnabled = false;
        }
        public override void ColonistStopWork(Colonist colonist)
        {
            colonist.AutoDecreaseStatsEnabled = true;
        }
    }
}
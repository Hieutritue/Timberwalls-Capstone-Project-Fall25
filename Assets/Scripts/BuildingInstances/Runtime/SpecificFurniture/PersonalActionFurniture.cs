using System;
using System.Collections.Generic;
using DefaultNamespace.TaskSystem;
using TaskSystem.Tasks.SpecificTask.PersonalTask;
using UnityEngine;

namespace BuildingSystem
{
    public class PersonalActionFurniture : Furniture, ITaskCreator
    {
        public TaskType TaskType;
        public List<Transform> ActionPoints;
        public PersonalActionFurnitureSo PersonalActionFurnitureSo => (PersonalActionFurnitureSo)PlaceableSo;

        public void CreateTask()
        {
            ActionPoints.ForEach(ap =>
            {
                switch (TaskType)
                {
                    case TaskType.Sleeping:
                        AddTask(new SleepingTask(this, ap, TaskType));
                        break;
                    case TaskType.Eating:
                        AddTask(new EatingTask(this, ap, TaskType));
                        break;
                    case TaskType.Pooping:
                        AddTask(new PooTask(this, ap, TaskType));
                        break;
                    case TaskType.Playing:
                        AddTask(new PlayingTask(this, ap, TaskType));
                        break;
                    case TaskType.Washing:
                        AddTask(new WashingTask(this, ap, TaskType));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}
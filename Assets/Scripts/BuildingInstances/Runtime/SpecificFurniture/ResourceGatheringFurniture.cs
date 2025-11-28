using System;
using System.Linq;
using DefaultNamespace.TaskSystem;
using ResourceSystem;
using UnityEditor.VersionControl;
using UnityEngine;

namespace BuildingSystem
{
    public class ResourceGatheringFurniture : Furniture, IWorkable, ITaskCreator
    {
        public Transform ActionPoint;
        public ResourceGatheringFurnitureSo GatheringFurnitureSo => (ResourceGatheringFurnitureSo)PlaceableSo;

        public override void Start()
        {
            base.Start();
            Animator = GetComponent<Animator>();
        }

        public override void TransitionToIdle()
        {
            _stateMachine.TransitionTo(_idleBuildingState);
            if (Animator)
                Animator.SetBool(BuildingAnimationString.IS_ACTIVE, false);
        }

        public override void TransitionToWorking()
        {
            _stateMachine.TransitionTo(_workingBuildingState);
            if (Animator)
                Animator.SetBool(BuildingAnimationString.IS_ACTIVE, true);
        }

        public void Work()
        {
            // if not enough resources, return
            if ((from resourceWithAmount in GatheringFurnitureSo.Consumption
                    let resourceType = resourceWithAmount.Resource.ResourceType
                    where ResourceManager.Instance.Get(resourceType) < resourceWithAmount.Amount
                    select resourceWithAmount).Any())
            {
                return;
            }

            GatheringFurnitureSo.Consumption.ForEach(resourceWithAmount =>
            {
                var resourceType = resourceWithAmount.Resource.ResourceType;
                ResourceManager.Instance.Set(resourceType,
                    ResourceManager.Instance.Get(resourceType) - resourceWithAmount.Amount);
            });
            GatheringFurnitureSo.OutputResource.ForEach(resourceWithAmount =>
            {
                var resourceType = resourceWithAmount.Resource.ResourceType;
                ResourceManager.Instance.Set(resourceType,
                    ResourceManager.Instance.Get(resourceType) + resourceWithAmount.Amount);
            });
        }

        public void CreateTask()
        {
            var task = GatheringFurnitureSo.TaskType switch
            {
                TaskType.Mining => new ResourceGatheringTask(this, TaskType.Mining),
                TaskType.Cooking => new ResourceGatheringTask(this, TaskType.Cooking),
                TaskType.Farming => new ResourceGatheringTask(this, TaskType.Farming),
                TaskType.Research => new ResourceGatheringTask(this, TaskType.Research),
                TaskType.Refining => new ResourceGatheringTask(this, TaskType.Refining),
                TaskType.ManufacturingMeds => new ResourceGatheringTask(this, TaskType.ManufacturingMeds),
                _ => throw new ArgumentOutOfRangeException()
            };
            AddTask(task);
        }
    }
}
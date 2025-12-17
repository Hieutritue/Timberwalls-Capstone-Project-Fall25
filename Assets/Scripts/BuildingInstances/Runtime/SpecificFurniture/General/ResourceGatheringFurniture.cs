using Assets.Scripts.UI.WorldSpaceUISystem.ConstructTooltip;
using DefaultNamespace.TaskSystem;
using MoreMountains.Feedbacks;
using System;
using System.Linq;
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

            var resourceGatheredString = "";
            GatheringFurnitureSo.Consumption.ForEach(resourceWithAmount =>
            {
                var resourceType = resourceWithAmount.Resource.ResourceType;
                resourceGatheredString += $"<color=red>- {resourceWithAmount.Amount} {resourceWithAmount.Resource.ResourceName}</color>\n";
                ResourceManager.Instance.Set(resourceType,
                    ResourceManager.Instance.Get(resourceType) - resourceWithAmount.Amount);
            });
            GatheringFurnitureSo.OutputResource.ForEach(resourceWithAmount =>
            {
                var resourceType = resourceWithAmount.Resource.ResourceType;
                resourceGatheredString += $"<color=white>+ {resourceWithAmount.Amount} {resourceWithAmount.Resource.ResourceName}</color>\n";
                ResourceManager.Instance.Set(resourceType,
                    ResourceManager.Instance.Get(resourceType) + resourceWithAmount.Amount);
            });

            var resourceGatheredFeedback = FeedbackManager.Instance.ResourceGatheredFeedback;
            var floatingText = resourceGatheredFeedback.GetFeedbackOfType<MMF_FloatingText>();
            floatingText.TargetTransform = ProgressPoint;
            floatingText.Value = resourceGatheredString;
            resourceGatheredFeedback.PlayFeedbacks();
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

        public override FurnitureTooltipData GetTooltipData()
        {
            return new ResourceGatheringTooltipData
            {
                Name = PlaceableSo.Name,
                Description = PlaceableSo.Description,
                Consumption = GatheringFurnitureSo.Consumption,
                OutputResource = GatheringFurnitureSo.OutputResource
            };
        }
    }
}
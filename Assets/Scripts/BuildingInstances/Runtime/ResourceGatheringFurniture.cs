using System;
using DefaultNamespace.TaskSystem;
using ResourceSystem;
using UnityEditor.VersionControl;
using UnityEngine;

namespace BuildingSystem
{
    public class ResourceGatheringFurniture : Furniture, IWorkable
    {
        public ResourceGatheringFurnitureSo GatheringFurnitureSo => (ResourceGatheringFurnitureSo)PlaceableSo;
        private ITask _gatheringTask;
        public void Work()
        {
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
            if(_gatheringTask != null) return;
            _gatheringTask = new ResourceGatheringTask(this, TaskType.Mining);
        }

        public void ClearTask()
        {
            _gatheringTask = null;
        }
    }
}
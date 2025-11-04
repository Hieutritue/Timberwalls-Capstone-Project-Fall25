using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace BuildingSystem
{
    public abstract class PersonalActionFurniture : Furniture, ITaskCreator
    {
        public PersonalActionFurnitureSo PersonalActionFurnitureSo => (PersonalActionFurnitureSo)PlaceableSo;
        public abstract void CreateTask();
    }
}
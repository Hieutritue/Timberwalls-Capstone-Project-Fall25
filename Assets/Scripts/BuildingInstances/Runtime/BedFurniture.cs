using DefaultNamespace.TaskSystem;

namespace BuildingSystem
{
    public class BedFurniture : PersonalActionFurniture
    {
        public override void CreateTask()
        {
            AddTask(new SleepingTask(this,TaskType.Sleeping));
        }
    }
}
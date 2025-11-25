using DefaultNamespace.TaskSystem;
using UnityEngine;

public static class TaskExtension
{
    public static Transform GetBuildingProgressPoint(this ITask task)
    {
        if (task == null)
        {
            Debug.LogWarning("Tried to get Transform from a null task.");
            return null;
        }

        if (task.Building == null)
        {
            if (task is FixingTask fixingTask && fixingTask.ShieldGenerator != null)
            {
                return fixingTask.ActionPoint;
            }
            
            else if (task is CleaningTask { CleanableObject: not null } cleaningTask)
            {
                return cleaningTask.CleanableObject.CleanPoint;
            }
            
            return null;
        }

        return task.Building.ProgressPoint;
    }
}
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
            Debug.LogWarning($"Task of type {task.TaskType} has no assigned building.");
            return null;
        }

        return task.Building.ProgressPoint;
    }
}
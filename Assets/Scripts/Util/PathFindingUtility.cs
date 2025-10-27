using Pathfinding;
using UnityEngine;

public static class PathfindingUtility
{
    public static bool CanGetCloseEnough(Vector3 agentPos, Vector3 targetPos, float radius)
    {
        var graph = AstarPath.active;

        // Get the nearest walkable nodes
        var startNode = graph.GetNearest(agentPos, NNConstraint.Default).node;
        var targetNode = graph.GetNearest(targetPos, NNConstraint.Default).node;
        
        Debug.Log($"Start Node Area: {startNode.Area}, Target Node Area: {targetNode.Area}");

        // If the target node is in the same connected area, easy check
        if (startNode.Area == targetNode.Area)
        {
            // If they’re in the same area, reachable
            return true;
        }

        // Otherwise, find the closest *reachable* node near the target that shares the same area
        var constraint = NNConstraint.Default;
        constraint.constrainArea = true;
        constraint.area = (int)startNode.Area;
        constraint.constrainWalkability = true;
        constraint.walkable = true;

        var nearestReachable = graph.GetNearest(targetPos, constraint);

        // Measure how close that reachable point is to the target
        float dist = Vector3.Distance((Vector3)nearestReachable.position, targetPos);

        return dist <= radius;
    }
}
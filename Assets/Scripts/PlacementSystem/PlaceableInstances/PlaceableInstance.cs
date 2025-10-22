using System;
using System.Collections.Generic;
using System.Linq;
using BuildingSystem;
using DefaultNamespace.PlaceableInstances;
using UnityEngine;

public class PlaceableInstance : MonoBehaviour
{
    public List<Vector3Int> OccupiedCells;
    public PlaceableSO PlaceableSo;

    public Building Building;

    private void Start()
    {
        Building = GetComponent<Building>();
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
    // Get all bottom-most cells (floor edge)
    public List<Vector3Int> GetFloorCells()
    {
        int minY = OccupiedCells.Min(c => c.y);
        return OccupiedCells.Where(c => c.y == minY).ToList();
    }

    // Get all top-most cells (ceiling edge)
    public List<Vector3Int> GetCeilingCells()
    {
        int maxY = OccupiedCells.Max(c => c.y);
        return OccupiedCells.Where(c => c.y == maxY).ToList();
    }

    // Get all edge cells (left, right, bottom, top edges)
    public List<Vector3Int> GetEdgeCells()
    {
        int minX = OccupiedCells.Min(c => c.x);
        int maxX = OccupiedCells.Max(c => c.x);

        return OccupiedCells.Where(c =>
            c.x == minX || c.x == maxX
        ).ToList();
    }

    // Get all interior cells (not edges)
    public List<Vector3Int> GetInteriorCells()
    {
        int minX = OccupiedCells.Min(c => c.x);
        int maxX = OccupiedCells.Max(c => c.x);
        int minY = OccupiedCells.Min(c => c.y);
        int maxY = OccupiedCells.Max(c => c.y);

        return OccupiedCells.Where(c =>
            c.x > minX && c.x < maxX &&
            c.y > minY && c.y < maxY
        ).ToList();
    }
}
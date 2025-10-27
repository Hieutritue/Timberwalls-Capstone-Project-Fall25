using System;
using System.Collections.Generic;
using BuildingSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableSO", menuName = "ScriptableObjects/PlaceableSO", order = 1)]
public class PlaceableSO : ScriptableObject
{
    [Header("Placement Info")]
    public Vector2Int Size;
    public GameObject Prefab;
    public PlaceableType Type;
    public List<PlacementConditionType> PlacementConditions;
    
    [Header("Basic Info")]
    public string Name;
    public Sprite Icon;
    public BuildingCategory Category;
    [TextArea] public string Description;

    [Header("Construction")]
    public List<ResourceWithAmount> Costs;
    public float BaseBuildTime;
}

public enum PlaceableType
{
    Room,
    Furniture,
    Decoration
}
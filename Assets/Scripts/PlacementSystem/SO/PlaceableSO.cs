using System;
using System.Collections.Generic;
using BuildingSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlaceableSO", menuName = "ScriptableObjects/PlaceableSO", order = 1)]
public class PlaceableSO : SerializedScriptableObject
{
    [Header("Placement Info")]
    public Vector2Int Size;
    public GameObject Prefab;
    public PlaceableType Type;
    public List<PlacementConditionType> PlacementConditions;
    public bool IsStair;
    
    [Header("Basic Info")]
    public string Name;
    public Sprite Icon;
    public BuildingCategory Category;
    public BuildingSubCategory SubCategory;
    
    [TextArea] public string Description;

    [Header("Construction")]
    public List<ResourceWithAmount> Costs;
    public float BaseBuildTime;
}

public enum PlaceableType
{
    Room,
    Furniture,
    Stair
}
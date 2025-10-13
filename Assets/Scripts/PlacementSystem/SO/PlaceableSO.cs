using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableSO", menuName = "ScriptableObjects/PlaceableSO", order = 1)]
public class PlaceableSO : ScriptableObject
{
    public string Name;
    public Vector2Int Size;
    public GameObject Prefab;
    public PlaceableType Type;
    public List<PlacementConditionType> PlacementConditions;
}

public enum PlaceableType
{
    Room,
    Furniture,
    Decoration
}
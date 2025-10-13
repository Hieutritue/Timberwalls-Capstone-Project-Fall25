using DefaultNamespace;
using UnityEngine;

public interface IPlacementRule
{
    bool IsValid(GridData gridData);
}


public enum PlacementConditionType
{
    None,
    NotOccupied,
    OnFloor,
    OnEdge,
    OnCeiling
}

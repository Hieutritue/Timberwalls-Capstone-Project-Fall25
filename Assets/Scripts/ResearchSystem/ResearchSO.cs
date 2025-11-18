using BuildingSystem;
using System.Collections.Generic;
using UnityEngine;

public enum ResearchCategory { Living, Science, Resource, Defense, Base }

[CreateAssetMenu(menuName = "ScriptableObjects/Research/ResearchSO", fileName = "ResearchSO")]
public class ResearchSO : ScriptableObject
{
    public string researchName;
    public string description;
    public ResearchCategory category;

    public ResearchSO[] prerequisites;
    public List<ResourceWithAmount> Costs;
    public List<PlaceableSO> unlocksBuildings;
}

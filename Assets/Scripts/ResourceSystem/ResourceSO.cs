using System.Collections.Generic;
using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(fileName = "ResourceSO", menuName = "ScriptableObjects/ResourceSO", order = 1)]
    public class ResourceSO : ScriptableObject
    {
        public Sprite Icon;
        public ResourceType ResourceType;
        public string ResourceName;
        public ItemTooltipSO TooltipSO;
    }
    
    public enum ResourceType
    {
        Wood,
        Stone,
        Iron,
        Copper,
        Biomass,
        RefinedIron,
        RefinedCopper,
        Oil,
        Steel,
        Plastic,
        Circuits,
        Niobium,
        BatteryCell,
        Bonium,
        SuperCoolant,
        RawFood,
        CookedFood,
        Pills,
        ResearchPointI,
        ResearchPointII,
        ResearchPointIII,
        ContactPoint
    }
}
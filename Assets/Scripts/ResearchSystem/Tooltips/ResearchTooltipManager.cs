using DefaultNamespace;
using DefaultNamespace.ResearchSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchTooltipManager : MonoSingleton<ResearchTooltipManager>
{
    private static ResearchTooltipManager instance;

    [SerializeField] private Camera camera;
    [SerializeField] private ResearchTooltip researchTooltip;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        RectTransform parentRect = transform.parent as RectTransform;
        if (parentRect == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            Input.mousePosition,
            null,
            out var localPoint
        );

        transform.localPosition = localPoint;
    }

    public static void ShowResearchTooltip(ResearchSO research)
    {
        instance.InternalShowResearchTooltip(research);
    }

    public static void ShowConstructTooltip(PlaceableSO construct)
    {
        instance.InternalShowConstructTooltip(construct);
    }

    public static void ShowResearchPointTooltip(ResearchSO research)
    {
        throw new NotImplementedException();
    }

    private void InternalShowResearchTooltip(ResearchSO research)
    {
        //string unlockStatus = ResearchManager.Instance.IsUnlocked(research) ? "Unlocked" : "Locked";
        string unlocks = "";
        foreach (var building in research.unlocksBuildings)
        {
            unlocks += $"{building.Name}, ";
        }
        
        if (unlocks.Length > 2)
        {
            unlocks = unlocks.Substring(0, unlocks.Length - 2);
        }

        string header = $"{research.researchName}";
        string body1 = $"- Description: {research.description}";
        string body2 = $"- Unlocks: {unlocks}";

        researchTooltip.ShowTooltip(header, body1, body2);
    }

    private void InternalShowConstructTooltip(PlaceableSO construct)
    {
        string constructionCosts = "";
        foreach (var cost in construct.Costs)
        {
            constructionCosts += $"\n+ {cost.Amount} {cost.Resource.ResourceName}";
        }

        string header = $"{construct.Name}";
        string body1 = $"- Description: {construct.Description}";
        string body2 = $"- Construction costs: {constructionCosts}";

        researchTooltip.ShowTooltip(header, body1, "");
    }

    public static void Hide()
    {
        instance.InternalHide();
    }

    public void InternalHide() 
    {
        researchTooltip.HideTooltip();
    }
}

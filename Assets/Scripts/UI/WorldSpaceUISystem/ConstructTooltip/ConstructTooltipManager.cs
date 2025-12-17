using Assets.Scripts.UI.WorldSpaceUISystem.ConstructTooltip;
using BuildingSystem;
using DefaultNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstructTooltipManager : MonoSingleton<ConstructTooltipManager>
{
    [SerializeField] private ConstructTooltip constructTooltip;

    private RectTransform parentRect;

    protected override void Awake()
    {
        base.Awake();

        parentRect = transform.parent as RectTransform;

        if (parentRect == null)
        {
            Debug.LogError("ConstructTooltipManager must be under a RectTransform.");
        }

        if (constructTooltip == null)
        {
            Debug.LogError("ConstructTooltipManager: constructTooltip is NULL at runtime.", this);
        }
    }

    void Update()
    {
        if (!constructTooltip.gameObject.activeSelf)
            return;

        if (parentRect == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            Input.mousePosition,
            null,
            out var localPoint
        );

        ((RectTransform)constructTooltip.transform).localPosition = localPoint;
    }

    public static void Show(FurnitureTooltipData data)
    {
        Instance.InternalShow(data);
    }

    public static void Hide()
    {
        Instance.constructTooltip.Hide();
    }

    private void InternalShow(FurnitureTooltipData data)
    {
        if (data == null)
        {
            Debug.LogWarning("ConstructTooltipManager.Show called with null data.");
            return;
        }

        switch (data)
        {
            case ResourceGatheringTooltipData rg:
                ShowResourceGathering(rg);
                break;

            default:
                ShowBasic(data);
                break;
        }

        constructTooltip.Show();
    }

    private void ShowBasic(FurnitureTooltipData data)
    {
        constructTooltip.SetText(
            data.Name,
            data.Description,
            "",
            ""
        );
    }

    private void ShowResourceGathering(ResourceGatheringTooltipData data)
    {
        bool isActive = true;

        string consumptionText = "None";
        if (data.Consumption != null && data.Consumption.Count > 0)
        {
            List<string> parts = new();

            foreach (var entry in data.Consumption)
            {
                int available =
                    ResourceManager.Instance.Get(entry.Resource.ResourceType);

                bool sufficient = available >= entry.Amount;
                if (!sufficient)
                    isActive = false;

                string part = $"{entry.Amount} {entry.Resource.ResourceName}";
                if (!sufficient)
                    part = $"<color=red>{part}</color>";

                parts.Add(part);
            }

            consumptionText = string.Join(", ", parts);
        }

        string productionText = "None";
        if (data.OutputResource != null && data.OutputResource.Count > 0)
        {
            List<string> parts = new();

            foreach (var entry in data.OutputResource)
            {
                parts.Add($"{entry.Amount} {entry.Resource.ResourceName}");
            }

            productionText = string.Join(", ", parts);
        }

        string statusText = isActive
            ? "<color=green>Active</color>"
            : "<color=red>Inactive</color>";

        constructTooltip.SetText(
            data.Name,
            $"Consumes: {consumptionText}",
            $"Produces: {productionText}",
            statusText
        );
    }

}



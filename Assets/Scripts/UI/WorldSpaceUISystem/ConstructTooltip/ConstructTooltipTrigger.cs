using BuildingSystem;
using UnityEngine;

public class ConstructTooltipTrigger : MonoBehaviour
{
    private Furniture furniture;

    void Awake()
    {
        furniture = GetComponentInParent<Furniture>();

        if (furniture == null)
        {
            Debug.LogError(
                $"FurnitureMouseHover on {name} could not find Furniture in parents."
            );
        }
    }

    void OnMouseEnter()
    {
        if (furniture == null) return;

        var data = furniture.GetTooltipData();
        ConstructTooltipManager.Show(data);
        Debug.Log($"Showing tooltip for {furniture.name}");
    }

    void OnMouseExit()
    {
        if (furniture == null) return;

        ConstructTooltipManager.Hide();
        Debug.Log($"Hiding tooltip for {furniture.name}");
    }
}


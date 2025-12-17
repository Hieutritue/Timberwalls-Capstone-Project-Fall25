using BuildingSystem;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

public class FurnitureMouseEventController : MonoBehaviour
{
    private Furniture _furniture;

    public void Setup(Furniture furniture)
    {
        _furniture = furniture;
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!IsPlacementIdleState())
            return;

        ConstructTooltipManager.Show(_furniture.GetTooltipData());

        ChangeFurnitureLayer(LayerMask.NameToLayer("HoveringBuilding"));
    }

    private void OnMouseExit()
    {
        ConstructTooltipManager.Hide();

        ChangeFurnitureLayer(LayerMask.NameToLayer("Building"));
    }

    private void ChangeFurnitureLayer(int layer)
    {
        LayerUtils.SetLayerRecursively(gameObject, layer);
    }

    private bool IsPlacementIdleState()
    {
        return BuildingSystemManager.Instance.PlacementSystem.CurrentState
            is IdlePlacementState;
    }
}

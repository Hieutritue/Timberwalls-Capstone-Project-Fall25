using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PlaceableSO _itemData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_itemData != null)
        {
            ItemTooltipManager.ShowTooltipItemStatic(_itemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipManager.HideTooltipStatic();
    }

    public void SetItem(PlaceableSO placeableTooltipSO)
    {
        _itemData = placeableTooltipSO;
    }
    
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemTooltipSO _itemData;

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

    public void SetItem(ItemTooltipSO itemTooltipSo)
    {
        _itemData = itemTooltipSo;
    }
}

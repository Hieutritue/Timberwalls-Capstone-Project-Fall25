using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemTooltipSO itemData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null)
        {
            TooltipItemManager.ShowTooltipItemStatic(itemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipItemManager.HideTooltipStatic();
    }

    public void SetItem(ItemTooltipSO itemTooltipSo)
    {
        itemData = itemTooltipSo;
    }
}

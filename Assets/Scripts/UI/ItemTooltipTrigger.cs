using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemTooltipSO _itemData;
    private Coroutine hoverCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverCoroutine = StartCoroutine(HoverTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);

        ItemTooltipManager.HideTooltipStatic();
    }

    private IEnumerator HoverTimer()
    {
        float hoverDuration = 0f;

        while (hoverDuration < 1f) 
        {
            hoverDuration += Time.deltaTime;
            yield return null;
            
        }
        if (_itemData != null)
            ItemTooltipManager.ShowTooltipItemStatic(_itemData);
    }

    public void SetItem(ItemTooltipSO itemTooltipSo)
    {
        _itemData = itemTooltipSo;
    }
    
}

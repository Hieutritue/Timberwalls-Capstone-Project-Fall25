using UnityEngine;
using UnityEngine.EventSystems;

public class NormalTooltipTrigger :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private NormalTooltipSO itemData;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && !string.IsNullOrEmpty(itemData.message))
        {
            NormalTooltipManager.ShowTooltipStatic(itemData.message);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NormalTooltipManager.HideTooltipStatic();
    }
}

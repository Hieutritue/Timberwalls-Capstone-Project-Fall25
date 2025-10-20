using UnityEngine;
using UnityEngine.EventSystems;

public class NormalTooltipTrigger :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private string message;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(message))
        {
            NormalTooltipManager.ShowTooltipStatic(message);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NormalTooltipManager.HideTooltipStatic();
    }
}

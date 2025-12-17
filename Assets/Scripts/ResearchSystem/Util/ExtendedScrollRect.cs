using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Extended Scroll Rect")]
[DisallowMultipleComponent]
public class ExtendedScrollRect : ScrollRect
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            eventData.button = PointerEventData.InputButton.Left;
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            eventData.button = PointerEventData.InputButton.Left;
            base.OnEndDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            eventData.button = PointerEventData.InputButton.Left;
            base.OnDrag(eventData);
        }
    }
}
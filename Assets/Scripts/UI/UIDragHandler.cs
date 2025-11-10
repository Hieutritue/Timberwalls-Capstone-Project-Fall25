using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Record offset between mouse position and panel position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Nothing needed here, but required by the interface
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
            // Vector3 pos = rectTransform.localPosition;
            // Vector3 minPos = (canvas.transform as RectTransform).rect.min - rectTransform.rect.min;
            // Vector3 maxPos = (canvas.transform as RectTransform).rect.max - rectTransform.rect.max;
            // pos.x = Mathf.Clamp(pos.x, minPos.x, maxPos.x);
            // pos.y = Mathf.Clamp(pos.y, minPos.y, maxPos.y);
            // rectTransform.localPosition = pos;

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Optional: clamp or do logic after drag ends
    }
}
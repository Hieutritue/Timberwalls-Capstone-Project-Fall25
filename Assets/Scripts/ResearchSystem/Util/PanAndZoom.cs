using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomHandler : MonoBehaviour, IScrollHandler
{
    [Header("Zoom Settings")]
    [SerializeField] private RectTransform content;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float maxZoomOut = .6f;
    [SerializeField] private float maxZoomIn = 2f;

    private Vector2 zoomLimits;

    public void OnScroll(PointerEventData eventData)
    {
        zoomLimits = new Vector2(maxZoomOut, maxZoomIn);

        if (content == null)
            return;

        float currentScale = content.localScale.x;
        float newScale = currentScale + eventData.scrollDelta.y * zoomSpeed;
        newScale = Mathf.Clamp(newScale, zoomLimits.x, zoomLimits.y);

        Vector2 mouseScreenPos = eventData.position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            content,
            mouseScreenPos,
            eventData.pressEventCamera,
            out Vector2 localPointBefore
        );

        content.localScale = Vector3.one * newScale;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            content,
            mouseScreenPos,
            eventData.pressEventCamera,
            out Vector2 localPointAfter
        );

        Vector2 diff = localPointAfter - localPointBefore;
        content.anchoredPosition += diff;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomHandler : MonoBehaviour, IScrollHandler
{
    [Header("Zoom Settings")]
    public RectTransform content;
    public float zoomSpeed = 0.1f;
    public Vector2 zoomLimits = new Vector2(0.5f, 2f);

    public void OnScroll(PointerEventData eventData)
    {
        if (content == null)
            return;

        float currentScale = content.localScale.x;
        float newScale = currentScale + eventData.scrollDelta.y * zoomSpeed;
        newScale = Mathf.Clamp(newScale, zoomLimits.x, zoomLimits.y);
        content.localScale = Vector3.one * newScale;
    }
}

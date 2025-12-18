using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class NormalTooltipManager : MonoBehaviour
{
    private static NormalTooltipManager instance;
    [SerializeField]private TextMeshProUGUI tooltipText;
    [SerializeField]private RectTransform backgroundTransform;
    

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null,
            out Vector2 mouseLocalPos
        );

        Vector2 offset = new Vector2(12f, -12f); // default: right & slightly down
        Vector2 tooltipSize = backgroundTransform.sizeDelta;
        Vector2 canvasSize = canvasRect.rect.size;

        Vector2 finalPos = mouseLocalPos + offset;

        // Right overflow → move tooltip to the left
        if (finalPos.x + tooltipSize.x > canvasSize.x * 0.5f)
        {
            finalPos.x = mouseLocalPos.x - tooltipSize.x - offset.x;
        }

        // Left overflow
        if (finalPos.x < -canvasSize.x * 0.5f)
        {
            finalPos.x = -canvasSize.x * 0.5f;
        }

        // Bottom overflow → move tooltip up
        if (finalPos.y - tooltipSize.y < -canvasSize.y * 0.5f)
        {
            finalPos.y = mouseLocalPos.y + tooltipSize.y;
        }

        // Top overflow
        if (finalPos.y > canvasSize.y * 0.5f)
        {
            finalPos.y = canvasSize.y * 0.5f;
        }

        transform.localPosition = finalPos;
    }



    private void ShowToolTip(string tooltipString)
    {
        gameObject.SetActive(true);
        tooltipText.text = tooltipString;
        float textPaddingSize = 8f;
        Vector2 backgroundSize = new Vector2(tooltipText.rectTransform.rect.width + textPaddingSize,
                                             tooltipText.preferredHeight+ textPaddingSize);
        backgroundTransform.sizeDelta = backgroundSize; //resize the background according to text size
    }
    
    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltipStatic(string tooltipString)
    {
        instance.ShowToolTip(tooltipString);
    }
    
    public static void HideTooltipStatic()
    {
        instance.HideToolTip();
    }


}

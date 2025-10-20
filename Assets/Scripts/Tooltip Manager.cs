using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instance;
    [SerializeField]private TextMeshProUGUI tooltipText;
    [SerializeField]private RectTransform backgroundTransform;
    

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), 
                                                        Input.mousePosition, null, out Vector2 localPoint);
        transform.localPosition = localPoint;
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
    


}

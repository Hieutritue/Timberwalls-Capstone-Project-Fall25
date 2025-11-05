using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ItemTooltipManager : MonoBehaviour
{
    private static ItemTooltipManager instance;
    private ItemTooltipSO displayItemTooltip;
    [SerializeField] private RectTransform backgroundTransform;
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemHowToGetDescription;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), 
                                                        Input.mousePosition, null, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }


    private void ShowToolTipItem()
    {
        if (displayItemTooltip != null)
        {
            itemSprite.sprite = displayItemTooltip.sprite;
            itemName.text = displayItemTooltip.itemName;
            itemHowToGetDescription.text = displayItemTooltip.howToGet;
            itemDescription.text = displayItemTooltip.itemDescription;
            gameObject.SetActive(true);
            float textPaddingSize = 8f;
            Vector2 backgroundSize = new Vector2(itemDescription.rectTransform.rect.width + textPaddingSize,
                                                 itemDescription.preferredHeight + textPaddingSize);
            backgroundTransform.sizeDelta = backgroundSize; //resize the background according to text size
        }
    }

    private void SetItemSO(ItemTooltipSO item)
    {
        displayItemTooltip = item;
    }
    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltipItemStatic(ItemTooltipSO item)
    {
        instance.SetItemSO(item);
        instance.ShowToolTipItem();
    }
    
    public static void HideTooltipStatic()
    {
        instance.HideToolTip();
    }
}

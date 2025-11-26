using System;
using System.Linq;
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
            DisplayTooltipToScreen();
        }
    }

    private void ShowToolTipItem(PlaceableSO placeableSo)
    {
        if (placeableSo != null)
        {
            itemSprite.sprite = placeableSo.Icon;
            itemName.text = placeableSo.Name;
            itemHowToGetDescription.text = string.Join("\n",
                placeableSo.Costs
                    .Where(c => c.Resource != null)
                    .Select(c => $"{c.Amount} - {c.Resource.ResourceName}"));
            itemDescription.text = $"Can be placed on {placeableSo.Size.x}x{placeableSo.Size.y}";
            DisplayTooltipToScreen();
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

    public static void ShowTooltipItemStatic(PlaceableSO item)
    {
        instance.ShowToolTipItem(item);
    }

    public static void HideTooltipStatic()
    {
        instance.HideToolTip();
    }

    private void DisplayTooltipToScreen()
    {
        gameObject.SetActive(true);
        float textPaddingSize = 8f;
        Vector2 backgroundSize = new Vector2(itemDescription.rectTransform.rect.width + textPaddingSize,
            itemDescription.preferredHeight + textPaddingSize);
        backgroundTransform.sizeDelta = backgroundSize; //resize the background according to text size
    }
}
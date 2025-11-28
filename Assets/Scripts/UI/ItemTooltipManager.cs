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
    private Vector2 offset = new Vector2(35f, 100f); 
    [SerializeField] private RectTransform backgroundTransform;
    //[SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemHowToGet;
    [SerializeField] private TextMeshProUGUI itemHowToGetDescription;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(),
        //     Input.mousePosition, null, out Vector2 localPoint);
        // transform.localPosition = localPoint;
        RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null,
            out Vector2 localPoint);

        //Show from top right
        Vector2 pos = localPoint + offset;

        Vector2 size = backgroundTransform.sizeDelta;
        float canvasW = canvasRect.rect.width * 0.5f;
        float canvasH = canvasRect.rect.height * 0.5f;

        // Because pivot is (1,1), x overflow checks change:
        if (pos.x - size.x < -canvasW)
            pos.x = -canvasW + size.x + 5f;

        if (pos.x > canvasW)
            pos.x = canvasW - 5f;

        // Y overflow checks:
        if (pos.y > canvasH)
            pos.y = canvasH - 5f;

        if (pos.y - size.y < -canvasH)
            pos.y = -canvasH + size.y + 5f;

        transform.localPosition = pos;
    }


    private void ShowToolTipItem()
    {
        if (displayItemTooltip != null)
        {
            //itemSprite.sprite = displayItemTooltip.sprite;
            itemName.text = displayItemTooltip.itemName;
            itemHowToGetDescription.text = displayItemTooltip.howToGet;
            itemDescription.text = displayItemTooltip.itemDescription;
            DisplayTooltip();
        }
    }

    private void ShowToolTipItem(PlaceableSO placeableSo)
    {
        itemHowToGet.text = "Resource Required";
        if (placeableSo != null)
        {
           
            itemName.text = placeableSo.Name;
            itemHowToGetDescription.text =
                string.Join("\n",
                    placeableSo.Costs
                        .Where(c => c.Resource != null)
                        .Select(c =>
                        {
                            int cost = c.Amount;
                            var resource = c.Resource;
                            int available = ResourceManager.Instance.Get(resource.ResourceType);
                            string color = cost <= available ? "white" : "red";

                            return $"{resource.ResourceName}: <color=\"{color}\">{cost}</color> ({available})";
                        })
                );

            itemDescription.text = $"Can be placed on {placeableSo.Size.x}x{placeableSo.Size.y}";
            DisplayTooltip();
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

    private void DisplayTooltip()
    {
        gameObject.SetActive(true);
        float textPaddingSize = 8f;
    }
}
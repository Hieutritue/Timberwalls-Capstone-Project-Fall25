using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnlockItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    public PlaceableSO Building { get; private set; }

    public void Setup(PlaceableSO building)
    {
        Building = building;
        icon.sprite = building.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ResearchTooltipManager.ShowConstructTooltip(Building);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResearchTooltipManager.Hide();
    }
}

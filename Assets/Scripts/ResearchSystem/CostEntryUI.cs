using BuildingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costNumber;
    [SerializeField] private Image icon;

    public void Setup(ResourceWithAmount cost)
    {
        costNumber.text = cost.Amount.ToString();
        icon.sprite = cost.Resource.Icon;
    }
}

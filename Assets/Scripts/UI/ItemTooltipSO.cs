using ResourceSystem;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTooltipSO", menuName = "ScriptableObjects/Tooltips/ItemTooltipSO")]
public class ItemTooltipSO : ScriptableObject
{
    public Sprite sprite;
    public string itemName;
    public string howToGet;
    public string itemDescription;
}

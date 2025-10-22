using UnityEngine;

[CreateAssetMenu(fileName = "NormalTooltipSO", menuName = "ScriptableObjects/Tooltips/NormalTooltipSO")]
public class NormalTooltipSO : ScriptableObject
{
    [TextArea(5, 10)] 
    public string message;
}

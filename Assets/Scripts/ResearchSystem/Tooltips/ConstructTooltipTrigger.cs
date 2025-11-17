using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructTooltipTrigger : MonoBehaviour
{
    [SerializeField] private ResearchNode node;  // <-- assigned in Inspector

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (node == null)
        {
            Debug.LogWarning("TooltipTrigger: No ResearchNode assigned.");
            return;
        }

        ResearchTooltipManager.ShowResearchTooltip(node.research);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResearchTooltipManager.Hide();
    }
}

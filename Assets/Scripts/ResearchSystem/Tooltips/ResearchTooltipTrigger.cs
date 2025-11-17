using DefaultNamespace.General;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResearchTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ResearchNode node;

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
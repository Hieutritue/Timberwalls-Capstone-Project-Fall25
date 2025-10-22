using UnityEngine;

public class ResearchTreeUI : MonoBehaviour
{
    [Header("References")]
    public ResearchTreeSO livingTree;
    public ResearchTreeSO scienceTree;
    public ResearchTreeSO resourceTree;
    public ResearchTreeSO defenseTree;
    public ResearchTreeSO baseTree;
    public ResearchNodeUI skillNodePrefab;

    [Header("UI Containers")]
    public RectTransform content; // draggable area
    public Transform livingPanel;
    public Transform sciencePanel;
    public Transform resourcePanel;
    public Transform defensePanel;
    public Transform basePanel;

    void Start()
    {
        PopulateTree(livingTree, livingPanel);
        PopulateTree(scienceTree, sciencePanel);
        PopulateTree(resourceTree, resourcePanel);
        PopulateTree(defenseTree, defensePanel);
        PopulateTree(baseTree, basePanel);
    }

    void PopulateTree(ResearchTreeSO tree, Transform parent)
    {
        foreach (var research in tree.researches)
        {
            var node = Instantiate(skillNodePrefab, parent);
            node.Setup(research);
        }
    }
}


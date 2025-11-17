using BuildingSystem;
using DefaultNamespace.ResearchSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchNode : MonoBehaviour
{
    [SerializeField] private Button unlockButton;
    [SerializeField] public ResearchSO research;
    [SerializeField] private TextMeshProUGUI researchName;
    [SerializeField] private Transform unlocksContainer;
    [SerializeField] private UnlockItemUI unlockItemPrefab;
    [SerializeField] private Transform costContainer;
    [SerializeField] private CostEntryUI costEntryPrefab;
    [SerializeField] private Image lockOverlay;
    [SerializeField] private Image unlockedGlow;

    private readonly List<UnlockItemUI> unlockEntries = new();
    private readonly List<CostEntryUI> costEntries = new();

    public System.Action<ResearchSO> OnResearchUnlocked;

    void Start()
    {
        Setup(research);
        unlockButton.onClick.AddListener(TryUnlock);
    }

    public void Setup(ResearchSO research)
    {
        this.research = research;
        researchName.text = research.researchName;

        BuildUnlockList();
        BuildCostList();
        RefreshVisuals();
    }

    void BuildUnlockList()
    {
        foreach (var entry in unlockEntries)
            Destroy(entry.gameObject);

        unlockEntries.Clear();

        foreach (var building in research.unlocksBuildings)
        {
            var entry = Instantiate(unlockItemPrefab, unlocksContainer);
            entry.Setup(building);
            unlockEntries.Add(entry);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)unlocksContainer);
    }

    void BuildCostList()
    {
        if (costContainer == null)
        {
            Debug.LogWarning($"{name}: costContainer is not assigned.");
            return;
        }

        if (costEntryPrefab == null)
        {
            Debug.LogWarning($"{name}: costEntryPrefab is not assigned.");
            return;
        }

        foreach (var entry in costEntries)
        {
            if (entry != null && entry.gameObject != null)
                Destroy(entry.gameObject);
        }

        costEntries.Clear();

        if (research == null || research.Costs == null || research.Costs.Count == 0)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)costContainer);
            return;
        }

        foreach (var cost in research.Costs)
        {
            var entry = Instantiate(costEntryPrefab, costContainer);
            entry.Setup(cost);
            costEntries.Add(entry);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)costContainer);
    }

    void TryUnlock()
    {
        if (ResearchManager.Instance.Unlock(research))
        {
            RefreshVisuals();
        }
    }

    bool CanUnlock()
    {
        return ResearchManager.Instance.CanUnlock(research);
    }

    bool IsUnlocked()
    {
        return ResearchManager.Instance.IsUnlocked(research);
    }

    void RefreshVisuals()
    {
        unlockedGlow.color = Color.white;
        unlockedGlow.gameObject.SetActive(false);
        lockOverlay.gameObject.SetActive(false);
        bool unlocked = IsUnlocked();
        bool canUnlock = CanUnlock();

        if (unlocked)
        {
            unlockedGlow.gameObject.SetActive(true);
            lockOverlay.gameObject.SetActive(false);
            unlockButton.interactable = false;
        }
        else if (canUnlock)
        {
            unlockedGlow.gameObject.SetActive(false);
            lockOverlay.gameObject.SetActive(false);
            unlockButton.interactable = true;
        }
        else
        {
            unlockedGlow.gameObject.SetActive(false);
            lockOverlay.gameObject.SetActive(true);
            unlockButton.interactable = false;
        }
    }
}

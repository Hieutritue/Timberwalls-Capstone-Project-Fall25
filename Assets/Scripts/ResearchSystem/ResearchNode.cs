using BuildingSystem;
using DefaultNamespace.ResearchSystem;
using ResourceSystem;
using System;
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

    private void OnEnable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
    }

    void Start()
    {
        Setup(research);
        unlockButton.onClick.AddListener(TryUnlock);

        UpdateVisuals();
    }

    public void Setup(ResearchSO research)
    {
        this.research = research;
        if (researchName != null)
            researchName.text = research != null ? research.researchName : "<No Research>";

        BuildUnlockList();
        BuildCostList();
        UpdateVisuals();
    }

    void BuildUnlockList()
    {
        for (int i = unlockEntries.Count - 1; i >= 0; i--)
        {
            var entry = unlockEntries[i];
            if (entry != null && entry.gameObject != null)
                Destroy(entry.gameObject);
        }

        unlockEntries.Clear();

        if (research == null || unlockItemPrefab == null || unlocksContainer == null)
            return;

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
            if (ResourceManager.Instance != null)
                ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
            UpdateVisuals();
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

    private void HandleResourceChanged(ResourceType type, int amount)
    {
        try
        {
            if (research == null || research.Costs == null) return;

            foreach (var costEntry in research.Costs)
            {
                if (costEntry.Resource == null) continue;
                if (costEntry.Resource.ResourceType == type)
                {
                    UpdateVisuals();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"ResearchNode error: {ex}");
        }
    }


    void UpdateVisuals()
    {
        if (researchName == null || unlockedGlow == null || lockOverlay == null || unlockButton == null)
        {
            return;
        }

        unlockedGlow.color = Color.white;
        unlockedGlow.gameObject.SetActive(false);
        lockOverlay.gameObject.SetActive(false);

        bool unlocked = false;
        bool canUnlock = false;

        if (research != null && ResearchManager.Instance != null)
        {
            unlocked = ResearchManager.Instance.IsUnlocked(research);
            canUnlock = ResearchManager.Instance.CanUnlock(research);
        }

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

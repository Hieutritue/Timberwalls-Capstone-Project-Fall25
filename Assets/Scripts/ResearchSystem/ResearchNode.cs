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
    [SerializeField] private Image lockOverlay;
    [SerializeField] private Image unlockedGlow;

    private readonly List<UnlockItemUI> unlockEntries = new();

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

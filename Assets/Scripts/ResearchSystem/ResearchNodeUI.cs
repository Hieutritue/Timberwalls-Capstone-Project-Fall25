using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text resarchName;
    public Button unlockButton;
    public ResearchSO research;

    public System.Action<ResearchSO> OnResearchUnlocked;

    private bool isUnlocked => research.isUnlocked;

    void Start()
    {
        Setup(research);
        unlockButton.onClick.AddListener(TryUnlock);
    }

    public void Setup(ResearchSO research)
    {
        this.research = research;
        icon.sprite = research.icon;
        resarchName.text = research.researchName;
        RefreshVisuals();
    }

    void TryUnlock()
    {
        if (CanUnlock())
        {
            research.isUnlocked = true;
            RefreshVisuals();
            OnResearchUnlocked?.Invoke(research);
        }
    }

    bool CanUnlock()
    {
        foreach (var prequisite in research.prerequisites)
        {
            if (!prequisite.isUnlocked)
                return false;
        }
        return true;
    }

    void RefreshVisuals()
    {
        icon.color = isUnlocked ? Color.white : Color.gray;
    }
}

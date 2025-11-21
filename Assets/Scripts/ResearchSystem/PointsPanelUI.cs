using ResourceSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsPanelUI : MonoBehaviour
{
    [Header("Research Point Displays")] [SerializeField]
    private TextMeshProUGUI basicCount; // ResearchPointI

    [SerializeField] private TextMeshProUGUI intermediateCount; // ResearchPointII
    [SerializeField] private TextMeshProUGUI advancedCount; // ResearchPointIII

    private readonly Dictionary<ResourceType, TextMeshProUGUI> _uiMap = new();

    private void Start()
    {
        _uiMap[ResourceType.ResearchPointI] = basicCount;
        _uiMap[ResourceType.ResearchPointII] = intermediateCount;
        _uiMap[ResourceType.ResearchPointIII] = advancedCount;

        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
        ResourceManager.Instance.Set(ResourceType.ResearchPointI,
            ResourceManager.Instance.Get(ResourceType.ResearchPointI));
        ResourceManager.Instance.Set(ResourceType.ResearchPointII,
            ResourceManager.Instance.Get(ResourceType.ResearchPointII));
        ResourceManager.Instance.Set(ResourceType.ResearchPointIII,
            ResourceManager.Instance.Get(ResourceType.ResearchPointIII));
    }

    private void HandleResourceChanged(ResourceType type, int newAmount)
    {
        if (_uiMap.TryGetValue(type, out TextMeshProUGUI tmp))
        {
            if (tmp != null)
                tmp.text = newAmount.ToString("N0");
        }
    }
}
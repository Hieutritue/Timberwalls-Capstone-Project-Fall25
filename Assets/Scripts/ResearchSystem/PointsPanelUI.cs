using ResourceSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsPanelUI : MonoBehaviour
{
    [Header("Research Point Displays")]
    [SerializeField] private TextMeshProUGUI basicCount;        // ResearchPointI
    [SerializeField] private TextMeshProUGUI intermediateCount; // ResearchPointII
    [SerializeField] private TextMeshProUGUI advancedCount;     // ResearchPointIII

    private readonly Dictionary<ResourceType, TextMeshProUGUI> _uiMap = new();

    private void Awake()
    {
        _uiMap[ResourceType.ResearchPointI] = basicCount;
        _uiMap[ResourceType.ResearchPointII] = intermediateCount;
        _uiMap[ResourceType.ResearchPointIII] = advancedCount;
    }

    private void OnEnable()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
            RefreshAll();
        }
        else
        {
            ResourceManager.OnInitialized += OnResourceManagerReady;
        }
    }

    private void OnResourceManagerReady()
    {
        ResourceManager.OnInitialized -= OnResourceManagerReady;
        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
        RefreshAll();
    }

    private void OnDisable()
    {
        ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
    }

    private void HandleResourceChanged(ResourceType type, int newAmount)
    {
        if (_uiMap.TryGetValue(type, out TextMeshProUGUI tmp))
        {
            if (tmp != null)
                tmp.text = newAmount.ToString("N0");
        }
    }

    private void RefreshAll()
    {
        foreach (var kvp in ResourceManager.Instance.GetAll())
        {
            HandleResourceChanged(kvp.Key, kvp.Value);
        }
    }
}

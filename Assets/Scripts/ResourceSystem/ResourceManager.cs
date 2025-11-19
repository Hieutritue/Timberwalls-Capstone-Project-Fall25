using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ResourceSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField] private List<ResourceSO> StartingResources; // assign all resources here in Inspector

    private Dictionary<ResourceType, int> _amounts = new();

    public event Action<ResourceType, int> OnResourceChanged; 
    public static event Action OnInitialized;

    private void Start()
    {
        foreach (var res in StartingResources)
            _amounts[res.ResourceType] = 0;
    }

    public void Set(ResourceType resourceType, int amount)
    {
        _amounts[resourceType] = Mathf.Max(0, amount);
        OnResourceChanged?.Invoke(resourceType, _amounts[resourceType]);
    }

    public int Get(ResourceType resourceType)
    {
        return _amounts.GetValueOrDefault(resourceType, 0);
    }
    
    public ResourceSO GetResourceSO(ResourceType resourceType)
    {
        return StartingResources.Find(r => r.ResourceType == resourceType);
    }
    
    public IReadOnlyDictionary<ResourceType, int> GetAll()
    {
        return _amounts;
    }
    
    public List<ResourceSO> GetAllResourceSOs()
    {
        return StartingResources;
    }
    [Button]
    public void AddResouce(ResourceType resourceType, int amount)
    {
        Set(resourceType, Get(resourceType) + amount);
    }
}
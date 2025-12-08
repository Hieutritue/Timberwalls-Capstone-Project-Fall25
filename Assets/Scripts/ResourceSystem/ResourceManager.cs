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
        BuildMenuManager.Instance.OnBuildMenuInitialized += () =>
        {
            Set(ResourceType.Wood, 100);
            Set(ResourceType.Stone, 100);
            Set(ResourceType.CookedFood, 50);
            Set(ResourceType.Iron, 20);
            Set(ResourceType.Copper, 20);
        };
        //testing purposes delete after test
        //BuildMenuManager.Instance.OnBuildMenuInitialized += AddAllResources999;
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
    
    [Button("Add All Resources (100)")]
    public void AddAllResources999()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            Set(type, 100);
        }
    }
    
    [Button("Reduce resources")]
    public void ReduceResources999(ResourceType resourceType, int amount)
    {
        Set(resourceType, Get(resourceType) - amount);
    }
    
    
    public bool HasEnoughResourcesForPlaceable(PlaceableSO objectToPlace)
    {
        foreach (var cost in objectToPlace.Costs)
        {
            int currentAmount = Get(cost.Resource.ResourceType);
            if (currentAmount < cost.Amount)
                return false;
        }

        return true;
    }

    public void DeductResourcesForPlaceable(PlaceableSO getCurrentObjectToPlace)
    {
        foreach (var cost in getCurrentObjectToPlace.Costs)
        {
            Set(cost.Resource.ResourceType, Get(cost.Resource.ResourceType) - cost.Amount);
        }
    }

    public void RefundResourcesForPlaceable(PlaceableSO placeableInstancePlaceableSo)
    {
        foreach (var cost in placeableInstancePlaceableSo.Costs)
        {
            Set(cost.Resource.ResourceType, Get(cost.Resource.ResourceType) + cost.Amount);
        }
    }

    [Button]
    public void AddAllResources(int amount)
    {
        StartingResources.ForEach(r => AddResouce(r.ResourceType,amount));
    }
}
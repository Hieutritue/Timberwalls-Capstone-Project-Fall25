using System;
using System.Collections.Generic;
using System.Linq;
using BuildingSystem;
using DefaultNamespace;
using UnityEngine;

namespace ResourceSystem.Storage
{
    public class ResourceStorageManager : MonoSingleton<ResourceStorageManager>
    {
        [SerializeField] private Transform _supplyParent;
        [SerializeField] private Transform _researchPointParent;
        [SerializeField] private Transform _materialParent;
        
        [SerializeField] private ResourceStorageDetailUI _resourceStorageDetailUIPrefab;
        
        private Dictionary<ResourceType, ResourceStorageDetailUI> _resourceStorageDetailUIs = new ();

        private void Start()
        {
            var manager = ResourceManager.Instance;

            foreach (var resourceSo in manager.GetAllResourceSOs())
            {
                ResourceStorageDetailUI detail;
                if (resourceSo.IsSupply)
                {
                    detail = Instantiate(_resourceStorageDetailUIPrefab, _supplyParent);
                }
                else if (resourceSo.IsResearchPoint)
                {
                    detail = Instantiate(_resourceStorageDetailUIPrefab, _researchPointParent);
                }
                else
                    detail = Instantiate(_resourceStorageDetailUIPrefab, _materialParent);

                detail.Setup(resourceSo,999);
                _resourceStorageDetailUIs[resourceSo.ResourceType] = detail;
            }
            
            gameObject.SetActive(false);
        }

        public int GetMaxCapacityForResourceType(ResourceType resourceType)
        {
            return _resourceStorageDetailUIs.TryGetValue(resourceType, out var detailUI)
                ? detailUI.MaxAmount
                : 999;
        }
    }
}
using System.Collections.Generic;
using DefaultNamespace.ResourceSystem;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceSystem
{
    public class ResourceInterface : MonoBehaviour
    {
        [Header("UI References")] [SerializeField]
        private ScrollRect ScrollRect;

        [SerializeField] private Transform ContentParent;
        [SerializeField] private ResourceDetail resourceDetailPrefab;
        [SerializeField] private Transform _supplyParent;
        [SerializeField] private Transform _researchPointParent;
        private readonly Dictionary<ResourceType, ResourceDetail> _details = new();

        private void Start()
        {
            var manager = ResourceManager.Instance;

            foreach (var resourceSo in manager.GetAllResourceSOs())
            {
                ResourceDetail detail;
                if (resourceSo.IsSupply)
                {
                    detail = Instantiate(resourceDetailPrefab, _supplyParent);
                }
                else if (resourceSo.IsResearchPoint)
                {
                    detail = Instantiate(resourceDetailPrefab, _researchPointParent);
                }
                else
                    detail = Instantiate(resourceDetailPrefab, ContentParent);

                detail.Setup(resourceSo.ResourceType, 0);
                _details.Add(resourceSo.ResourceType, detail);
            }

            manager.OnResourceChanged += HandleResourceChanged;
        }

        private void OnDestroy()
        {
            if (ResourceManager.Instance != null)
                ResourceManager.Instance.OnResourceChanged -= HandleResourceChanged;
        }

        private void HandleResourceChanged(ResourceType resourceType, int newAmount)
        {
            if (_details.TryGetValue(resourceType, out var detail))
                detail.UpdateAmount(newAmount);
        }
    }
}
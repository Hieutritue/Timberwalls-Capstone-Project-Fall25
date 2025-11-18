using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ResourceSystem
{
    public class ResourceDetail : MonoBehaviour
    {
        public Image Icon;
        public TMP_Text NameText;
        public TMP_Text AmountText;
        private readonly string UNKNOWN_RESOURCE = "Unknown resource";
        private ResourceSO _resource;

        public void Setup(ResourceType resourceType, int amount)
        {
            var resource = ResourceManager.Instance.GetResourceSO(resourceType);
            ItemTooltipSO _itemTooltip = resource.TooltipSO;
            var itemTooltipTrigger = gameObject.GetComponent<ItemTooltipTrigger>();
            itemTooltipTrigger.SetItem(_itemTooltip);
            _resource = resource;
            if (Icon)
            {
                if(Icon.sprite != null)
                {
                    Icon.sprite = resource.Icon;
                }
                else
                {
                    Debug.LogError("No icon found for resource");
                }
                
            }

            if (NameText)
            {
                NameText.text = !string.IsNullOrEmpty(resource.ResourceName) ? resource.ResourceName : UNKNOWN_RESOURCE;
            }
            
            UpdateAmount(amount);
        }

        public void UpdateAmount(int newAmount)
        {
            if (AmountText)
                AmountText.text = newAmount.ToString();
            gameObject.SetActive(newAmount != 0);
        }
    }
}
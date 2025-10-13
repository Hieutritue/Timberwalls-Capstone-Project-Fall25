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

        private ResourceSO _resource;

        public void Setup(ResourceType resourceType, int amount)
        {
            var resource = ResourceManager.Instance.GetResourceSO(resourceType);
            _resource = resource;
            if (Icon) Icon.sprite = resource.Icon;
            if (NameText) NameText.text = resource.ResourceType.ToString();
            UpdateAmount(amount);
        }

        public void UpdateAmount(int newAmount)
        {
            if (AmountText)
                AmountText.text = newAmount.ToString();
        }
    }
}
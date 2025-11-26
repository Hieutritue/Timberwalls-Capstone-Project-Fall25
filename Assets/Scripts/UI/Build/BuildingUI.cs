using System;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.Build
{
    public class BuildingUI : MonoBehaviour
    {
        public PlaceableSO PlaceableData;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _nameText;

        public void Init(PlaceableSO placeable)
        {
            PlaceableData = placeable;
            var placeableTooltipTrigger = gameObject.GetComponent<PlaceableTooltipTrigger>();
            if (placeableTooltipTrigger != null)
            {
                //if(placeable.ItemTooltipSO != null)
                placeableTooltipTrigger.SetItem(placeable);
            }
            _iconImage.sprite = placeable.Icon;
            _button.onClick.AddListener(() =>
            {
                BuildingSystemManager.Instance.PlacementSystem.EnterPlacementMode(placeable);
            });
            RegisterResourceEvents();
            _nameText.text = placeable.Name;
            
            OnResourceChanged(ResourceType.Wood,0);
        }

        private void OnDestroy()
        {
            var manager = ResourceManager.Instance;
            if (manager != null)
            {
                manager.OnResourceChanged -= OnResourceChanged;
            }
        }

        private bool IsEnoughResources()
        {
            foreach (var cost in PlaceableData.Costs)
            {
                int currentAmount = ResourceManager.Instance.Get(cost.Resource.ResourceType);
                if (currentAmount < cost.Amount)
                    return false;
            }

            return true;
        }

        private void RegisterResourceEvents()
        {
            var manager = ResourceManager.Instance;
            manager.OnResourceChanged += OnResourceChanged;
        }

        private void OnResourceChanged(ResourceType resourceType, int amount)
        {
            bool isEnough = IsEnoughResources();
            SetColorActiveIcon(isEnough);
            _button.interactable = isEnough;
        }

        private void SetColorActiveIcon(bool isActive)
        {
            Color color = isActive ? Color.white : Color.black;
            _iconImage.color = color;
        }
    }
}
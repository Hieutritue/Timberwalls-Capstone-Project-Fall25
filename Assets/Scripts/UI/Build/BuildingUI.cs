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
            var itemTooltipTrigger = gameObject.GetComponent<ItemTooltipTrigger>();
            if (itemTooltipTrigger != null)
            {
                if(placeable.ItemTooltipSO != null)
                itemTooltipTrigger.SetItem(placeable.ItemTooltipSO);
            }
            _iconImage.sprite = placeable.Icon;
            _button.onClick.AddListener(() =>
            {
                BuildingSystemManager.Instance.PlacementSystem.EnterPlacementMode(placeable);
            });
            _nameText.text = placeable.Name;
        }
    }
}
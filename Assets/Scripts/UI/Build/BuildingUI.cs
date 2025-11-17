using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.Build
{
    public class BuildingUI : MonoBehaviour
    {
        public PlaceableSO PlaceableData;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _button;
        public void Init(PlaceableSO placeable)
        {
            PlaceableData = placeable;
            _iconImage.sprite = placeable.Icon;
            _button.onClick.AddListener(() =>
            {
                BuildingSystemManager.Instance.PlacementSystem.EnterPlacementMode(placeable);
            });
        }
    }
}
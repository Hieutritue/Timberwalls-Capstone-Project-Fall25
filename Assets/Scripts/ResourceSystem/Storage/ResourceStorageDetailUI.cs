using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceSystem.Storage
{
    public class ResourceStorageDetailUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _resourceNameText;
        [SerializeField] private TMP_Text _maxAmountText;
        private ResourceSO _resourceSO;

        private int _maxAmount;
        public int MaxAmount
        {
            get => _maxAmount;
            set
            {
                _maxAmount = Mathf.Clamp(value, 0, 999);
                _maxAmountText.text = _maxAmount.ToString();
            }
        }

        public void Setup(ResourceSO resourceSO, int maxAmount)
        {
            _resourceSO = resourceSO;
            _iconImage.sprite = resourceSO.Icon;
            _resourceNameText.text = resourceSO.ResourceName;
            // Use the property to ensure clamping and text update
            MaxAmount = maxAmount;
        }

        public void IncreaseMaxAmount()
        {
            int delta = 1;
            delta = IsCtrlHeld() ? 10 : delta;
            delta = IsShiftHeld() ? 100 : delta;
            MaxAmount += delta;
        }

        public void DecreaseMaxAmount()
        {
            int delta = 1;
            delta = IsCtrlHeld() ? 10 : delta;
            delta = IsShiftHeld() ? 100 : delta;
            MaxAmount -= delta;
        }

        private bool IsShiftHeld()
        {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }

        private bool IsCtrlHeld()
        {
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        }
    }
}
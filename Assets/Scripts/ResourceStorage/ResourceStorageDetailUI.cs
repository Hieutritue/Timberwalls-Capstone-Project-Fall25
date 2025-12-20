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
        [SerializeField] private Image _background;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _reachedCapColor;
        private ResourceSO _resourceSO;

        private int _maxAmount;

        public int MaxAmount
        {
            get => _maxAmount;
            set
            {
                _maxAmount = Mathf.Clamp(value, 0, 999);
                _maxAmountText.text = _maxAmount.ToString();
                CheckChangeColorResourceCap(_resourceSO.ResourceType,
                    ResourceManager.Instance.Get(_resourceSO.ResourceType));
            }
        }

        public void Setup(ResourceSO resourceSO, int maxAmount)
        {
            _resourceSO = resourceSO;
            _iconImage.sprite = resourceSO.Icon;
            _resourceNameText.text = resourceSO.ResourceName;
            // Use the property to ensure clamping and text update
            MaxAmount = maxAmount;
            ResourceManager.Instance.OnResourceChanged += CheckChangeColorResourceCap;
        }

        private void CheckChangeColorResourceCap(ResourceType resourceType, int value)
        {
            if (resourceType != _resourceSO.ResourceType) return;
            _background.color = value >= MaxAmount ? _reachedCapColor : _normalColor;
        }

        public void IncreaseMaxAmount()
        {
            FeedbackManager.Instance.ButtonClickSmallFeedback.PlayFeedbacks();
            int delta = 1;
            delta = IsCtrlHeld() ? 10 : delta;
            delta = IsShiftHeld() ? 100 : delta;
            MaxAmount += delta;
        }

        public void DecreaseMaxAmount()
        {
            FeedbackManager.Instance.ButtonClickSmallFeedback.PlayFeedbacks();
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